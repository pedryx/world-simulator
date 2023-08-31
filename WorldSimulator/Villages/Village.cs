using BehaviourTree;
using BehaviourTree.FluentBuilder;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;

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
    private const float minDistance = 150.0f;
    /// <summary>
    /// Maximal distance of a building to any other building.
    /// </summary>
    private const float maxDistance = 200.0f;
    /// <summary>
    /// Second power of maximal distance.
    /// </summary>
    private const float minDistanceSquared = minDistance * minDistance;

    /// <summary>
    /// Map villagers to their behavior trees
    /// </summary>
    /// <returns></returns>
    private readonly Dictionary<IEntity, IBehaviour<VillagerContext>> behaviorTrees = new();
    private readonly Game game;
    private readonly List<IEntity> buildings = new();
    private readonly Dictionary<ResourceType, IEntity> resourceProcessingBuildings = new();
    private readonly Random random;
    private readonly GameWorld gameWorld;

    // TODO: Resolve the issue with stockpile position, when the stockpile gets destroyed.
    private IEntity stockpile;

    public Village(Game game, GameWorld gameWorld)
    {
        this.game = game;
        this.gameWorld = gameWorld;

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
            if (!query.Any() && gameWorld.GetTerrain(buildingPosition).Buildable)
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

    public void AddResourceProcessingBuilding(ResourceType resource, IEntity entity)
    {
        resourceProcessingBuildings.Add(resource, entity);
    }

    public void AddVillager(IEntity entity)
    {
        ResourceType resourceType = ResourceType.Get(behaviorTrees.Count);
        behaviorTrees.Add(entity, CreateBehaviorTree(resourceType, resourceProcessingBuildings[resourceType]));
    }

    public IBehaviour<VillagerContext> GetBehaviorTree(IEntity entity)
        => behaviorTrees[entity];

    private IBehaviour<VillagerContext> CreateBehaviorTree(ResourceType resource, IEntity workplace)
    {
        return FluentBuilder.Create<VillagerContext>()
            .Sequence("villager job sequence")
                .Do("find nearest resource", FindNearestResource(resource))
                .Do("move to nearest resource", MoveTo(null))
                .Do("harvest resource", HarvestResource(resource))
                .Do("move to workplace", MoveTo(workplace))
                .Do("wait until resource is processed", Wait(resource.HarvestItem.TimeToProcess))
                .Do("process the resource", ProcessResource(resource))
                .Do("move to stockpile", MoveTo(stockpile))
                .Do("store items", StoreItems())
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

    private Func<VillagerContext, BehaviourStatus> Wait(float waitTime)
    {
        return (context) =>
        {
            ref VillagerBehavior behavior = ref context.Entity.GetComponent<VillagerBehavior>();

            behavior.ElapsedWait += context.DeltaTime * game.Speed;

            if (behavior.ElapsedWait >= waitTime)
            {
                behavior.ElapsedWait = 0.0f;
                return BehaviourStatus.Succeeded;
            }

            return BehaviourStatus.Running;
        };
    }

    private static Func<VillagerContext, BehaviourStatus> ProcessResource(ResourceType resource)
    {
        return (context) =>
        {
            ref Inventory inventory = ref context.Entity.GetComponent<Inventory>();

            inventory.Items.Transform(resource.HarvestItem, resource.HarvestItem.ProcessedItem);
            return BehaviourStatus.Succeeded;
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

    private static Func<VillagerContext, BehaviourStatus> HarvestResource(ResourceType resourceType)
    {
        return (context) =>
        {
            ref DamageDealer damageDealer = ref context.Entity.GetComponent<DamageDealer>();
            ref VillagerBehavior behavior = ref context.Entity.GetComponent<VillagerBehavior>();

            damageDealer.Target = behavior.Target;
          
            return damageDealer.Target.IsDestroyed() ? BehaviourStatus.Succeeded : BehaviourStatus.Running;
        };
    }

    private static Func<VillagerContext, BehaviourStatus> StoreItems()
    {
        return (context) =>
        {
            ref Inventory stockpileInventory = ref context.Entity.GetComponent<Inventory>();
            // TODO: dont transfer all items
            context.Entity.GetComponent<Inventory>().Items.TransferTo(ref stockpileInventory.Items);

            return BehaviourStatus.Succeeded;
        };
    }
    #endregion
}
