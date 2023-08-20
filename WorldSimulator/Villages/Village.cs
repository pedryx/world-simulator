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
    /// Minimal distance of building to all other buildings.
    /// </summary>
    private const float minDistance = 150.0f;
    /// <summary>
    /// Maximal distance of building to any other building.
    /// </summary>
    private const float maxDistance = 200.0f;
    private const float minDistanceSquared = minDistance * minDistance;

    private readonly Dictionary<IEntity, IBehaviour<VillagerContext>> behaviorTrees = new();
    private readonly Game game;
    private readonly List<IEntity> buildings = new();
    private readonly Dictionary<Resource, IEntity> resourceProcessingBuildings = new();
    private readonly Random random;
    private readonly GameWorld gameWorld;

    // TODO: resolve issue with stockpile position, when stockpile gets destroyed
    private IEntity stockpile;

    public Village(Game game, GameWorld gameWorld)
    {
        this.game = game;
        this.gameWorld = gameWorld;

        random = new Random(game.GenerateSeed());
    }

    public Vector2 GetNextBuildingPosition()
    {
        while (true)
        {
            Vector2 center = buildings[random.Next(buildings.Count)].GetComponent<Position>().Coordinates;
            Vector2 buildingPosition = random.NextPointInRing(center, minDistance, maxDistance);

            var query = buildings
                .Select(b => b.GetComponent<Position>().Coordinates)
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

    public void AddResourceProcessingBuilding(Resource resource, IEntity entity)
    {
        resourceProcessingBuildings.Add(resource, entity);
    }

    public void AddVillager(IEntity entity)
    {
        behaviorTrees.Add(entity, CreateBehaviorTree(Resource.Tree, resourceProcessingBuildings[Resource.Tree]));
    }

    public IBehaviour<VillagerContext> GetBehaviorTree(IEntity entity)
        => behaviorTrees[entity];

    private IBehaviour<VillagerContext> CreateBehaviorTree(Resource resource, IEntity workplace)
    {
        return FluentBuilder.Create<VillagerContext>()
            .Sequence("villager job sequence")
                .Do("find nearest resource", FindNearestResource(resource))
                .Do("move to nearest resource", MoveTo(null))
                .Do("wait until resource is harvested", Wait(resource.HarvestTime))
                .Do("harvest resource", HarvestResource(resource))
                .Do("move to workplace", MoveTo(workplace))
                .Do("wait until resource is processed", Wait(resource.HarvestItem.TimeToProcess))
                .Do("move to stockpile", MoveTo(stockpile))
                .Do("store items", StoreItems(resource))
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
                    .GetComponent<Position>().Coordinates + Vector2.UnitY;
                Vector2 position = context.Entity.GetComponent<Position>().Coordinates;
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

            behavior.elapsedWait += context.DeltaTime * game.Speed;

            if (behavior.elapsedWait >= waitTime)
            {
                behavior.elapsedWait = 0.0f;
                return BehaviourStatus.Succeeded;
            }

            return BehaviourStatus.Running;
        };
    }

    private static Func<VillagerContext, BehaviourStatus> FindNearestResource(Resource resourceType)
    {
        return (context) =>
        {
            Vector2 position = context.Entity.GetComponent<Position>().Coordinates;
            IEntity resource = context.GameWorld.GetAndRemoveNearestResource(resourceType, position);

            if (resource == null)
                return BehaviourStatus.Failed;

            context.Entity.GetComponent<VillagerBehavior>().Target = resource;

            if (resource.HasComponent<AnimalController>())
            {
                resource.GetComponent<AnimalController>().UpdateEnabled = false;
                resource.GetComponent<Movement>().Speed = 0.0f;
            }

            return BehaviourStatus.Succeeded;
        };
    }

    private static Func<VillagerContext, BehaviourStatus> HarvestResource(Resource resourceType)
    {
        return (context) =>
        {
            context.Entity.GetComponent<VillagerBehavior>().Target.Destroy();
            context.Entity.GetComponent<Inventory>().Slots[resourceType.HarvestItem.ID]++;

            return BehaviourStatus.Succeeded;
        };
    }

    private Func<VillagerContext, BehaviourStatus> StoreItems(Resource resourceType)
    {
        return (context) =>
        {
            context.Entity.GetComponent<Inventory>().Slots[resourceType.HarvestItem.ID]--;
            stockpile.GetComponent<Inventory>().Slots[resourceType.HarvestItem.ID]++;

            return BehaviourStatus.Succeeded;
        };
    }
    #endregion
}
