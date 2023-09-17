using BehaviourTree;
using BehaviourTree.FluentBuilder;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;
using WorldSimulator.Level;

namespace WorldSimulator.Villages;
internal class Village
{
    /// <summary>
    /// Minimal distance of a building to all other buildings.
    /// </summary>
    private const float minDistance = 140.0f;
    /// <summary>
    /// Maximal distance of a building to any other building.
    /// </summary>
    private const float maxDistance = 220.0f;
    /// <summary>
    /// Second power of maximal distance.
    /// </summary>
    private const float minDistanceSquared = minDistance * minDistance;

    /// <summary>
    /// Map villagers to their behavior trees
    /// </summary>
    /// <returns></returns>
    private readonly Dictionary<IEntity, IBehaviour<VillagerContext>> behaviorTrees = new();
    private readonly List<IEntity> buildings = new();
    private readonly Dictionary<ResourceType, IEntity> resourceProcessingBuildings = new();
    private readonly Random random;
    private readonly GameWorld gameWorld;

    // TODO: Resolve the issue with stockpile position, when the stockpile gets destroyed.
    private IEntity stockpile;

    public int ID { get; private init; }

    public Village(Game game, GameWorld gameWorld)
    {
        this.gameWorld = gameWorld;

        ID = gameWorld.AddVillage(this);
        random = new Random(game.GenerateSeed());
    }

    /// <summary>
    /// Get next valid position where a building could be placed.
    /// </summary>
    public Vector2 GetNextBuildingPosition()
    {
        while (true)
        {
            Vector2 center = buildings[random.Next(buildings.Count)].GetComponent<Location>().Position;
            Vector2 buildingPosition = random.NextPointInRing(center, minDistance, maxDistance);

            var query = buildings
                .Select(b => b.GetComponent<Location>().Position)
                .Where(p => Vector2.DistanceSquared(p, buildingPosition) < minDistanceSquared);
            if (!query.Any() && gameWorld.IsBuildable(buildingPosition))
                return buildingPosition;
        }
    }

    public void AddBuilding(IEntity entity)
    {
        buildings.Add(entity);
    }

    public void AddStockpile(IEntity entity)
    {
        stockpile = entity;
        AddBuilding(entity);
    }

    public void AddHouse(IEntity entity)
    {
        entity.GetComponent<VillagerSpawner>().VillageID = ID;
        AddBuilding(entity);
    }

    public void AddResourceProcessingBuilding(ResourceType resourceType, IEntity entity)
    {
        resourceProcessingBuildings.Add(resourceType, entity);
        AddBuilding(entity);
    }

    public void AddVillager(IEntity entity)
    {
        Debug.Assert(entity.HasComponent<VillagerBehavior>());

        ResourceType resourceType = ResourceType.Get(behaviorTrees.Count);
        IEntity workPlace = resourceProcessingBuildings[resourceType];

        ref VillagerBehavior behavior = ref entity.GetComponent<VillagerBehavior>();

        behavior.WorkPlace = workPlace;
        behavior.VillageID = ID;

        behaviorTrees.Add(entity, CreateBehaviorTree(resourceType, workPlace));
    }

    public IBehaviour<VillagerContext> GetBehaviorTree(IEntity entity)
        => behaviorTrees[entity];

    private IBehaviour<VillagerContext> CreateBehaviorTree(ResourceType resourceType, IEntity workplace)
    {
        return FluentBuilder.Create<VillagerContext>()
            .Sequence("villager job sequence")
                .Do("find nearest resource", FindNearestResource(resourceType))
                .Do("move to nearest resource", MoveTo(null))
                .Do("harvest resource", HarvestResource)
                .Do("move to workplace", MoveTo(workplace))
                .Do("start processing resources", StartResourceProcessing)
                .Do("wait until resources are processed", WaitForResources)
                .Do("move to stockpile", MoveTo(stockpile))
                .Do("store items", StoreItems)
            .End()
            .Build();
    }

    #region behavior tree leaf nodes
    private static Func<VillagerContext, BehaviourStatus> MoveTo(IEntity target)
    {
        return (context) =>
        {
            ref PathFollow pathFollow = ref context.Entity.GetComponent<PathFollow>();

            if (pathFollow.PathIndex == pathFollow.Path.Length)
            {
                // when two entities are at same position, depth layer fighting occur
                Vector2 targetPosition = (target ?? context.Entity.GetComponent<VillagerBehavior>().Target)
                    .GetComponent<Location>().Position + Vector2.UnitY;
                Vector2 position = context.Entity.GetComponent<Location>().Position;
                float speed = context.Entity.GetComponent<Movement>().Speed;

                if (position.IsCloseEnough(targetPosition, speed * context.DeltaTime))
                    return BehaviourStatus.Succeeded;

                pathFollow.Path = context.GameWorld.FindPath(position, targetPosition);
                pathFollow.PathIndex = 0;
            }

            return BehaviourStatus.Running;
        };
    }

    private static Func<VillagerContext, BehaviourStatus> FindNearestResource(ResourceType resourceType)
    {
        return (context) =>
        {
            Vector2 position = context.Entity.GetComponent<Location>().Position;
            IEntity resource = context.GameWorld.GetAndRemoveNearestResource(resourceType, position);

            if (resource == null)
                return BehaviourStatus.Failed;

            context.Entity.GetComponent<VillagerBehavior>().Target = resource;

            if (resource.HasComponent<AnimalBehavior>())
            {
                resource.GetComponent<AnimalBehavior>().UpdateEnabled = false;
                resource.GetComponent<Movement>().Speed = 0.0f;
            }

            return BehaviourStatus.Succeeded;
        };
    }

    private static BehaviourStatus HarvestResource(VillagerContext context)
    {
        ref DamageDealer damageDealer = ref context.Entity.GetComponent<DamageDealer>();
        ref VillagerBehavior behavior = ref context.Entity.GetComponent<VillagerBehavior>();

        damageDealer.Target = behavior.Target;

        return damageDealer.Target.IsDestroyed() ? BehaviourStatus.Succeeded : BehaviourStatus.Running;
    }

    private static BehaviourStatus StartResourceProcessing(VillagerContext context)
    {
        IEntity workPlace = context.Entity.GetComponent<VillagerBehavior>().WorkPlace;

        ref Inventory villagerInventory = ref context.Entity.GetComponent<Inventory>();
        ref Inventory workPlaceInventory = ref workPlace.GetComponent<Inventory>();
        ref ResourceProcessor resourceProcessor = ref workPlace.GetComponent<ResourceProcessor>();

        villagerInventory.Items.TransferTo
        (
            ref workPlaceInventory.Items,
            resourceProcessor.InputItem,
            resourceProcessor.InputQuantity
        );

        return BehaviourStatus.Succeeded;
    }

    private static BehaviourStatus WaitForResources(VillagerContext context)
    {
        IEntity workPlace = context.Entity.GetComponent<VillagerBehavior>().WorkPlace;

        ref Inventory villagerInventory = ref context.Entity.GetComponent<Inventory>();
        ref Inventory workPlaceInventory = ref workPlace.GetComponent<Inventory>();
        ref ResourceProcessor resourceProcessor = ref workPlace.GetComponent<ResourceProcessor>();

        if (workPlaceInventory.Items.Has(resourceProcessor.OutputItem))
        {
            workPlaceInventory.Items.TransferTo(ref villagerInventory.Items);
            return BehaviourStatus.Succeeded;
        }

        return BehaviourStatus.Running;
    }

    private BehaviourStatus StoreItems(VillagerContext context)
    {
        ref Inventory stockpileInventory = ref stockpile.GetComponent<Inventory>();
        context.Entity.GetComponent<Inventory>().Items.TransferTo(ref stockpileInventory.Items);

        return BehaviourStatus.Succeeded;
    }
    #endregion
}
