using BehaviourTree;
using BehaviourTree.FluentBuilder;

using System.Collections.Generic;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Villages;
internal class Village
{
    private readonly Dictionary<IEntity, IBehaviour<VillagerContext>> behaviorTrees = new();

    public void AddVillager(IEntity entity)
    {
        behaviorTrees.Add(entity, CreateBehaviorTree());
    }

    public IBehaviour<VillagerContext> GetbehaviorTree(IEntity entity)
        => behaviorTrees[entity];

    private static IBehaviour<VillagerContext> CreateBehaviorTree()
    {
        return FluentBuilder.Create<VillagerContext>()
            .Sequence("villager job sequence")
                .Do("find nearest resource", FindNearestResource)
                .Do("move to nearest resource", MoveTo)
                .Do("harvest resource", HarvestResource)
            .End()
            .Build();
    }

    #region behavior tree actions and conditions
    private static BehaviourStatus FindNearestResource(VillagerContext context)
    {
        Position position = context.Entity.GetComponent<Position>();
        ref VillagerBehavior behavior = ref context.Entity.GetComponent<VillagerBehavior>();
        ref PathFollow pathFollow = ref context.Entity.GetComponent<PathFollow>();

        IEntity resource = context.GameWorld.GetAndRemoveNearestResource(ResourceTypes.Tree, position.Coordinates);
        Position resourcePosition = resource.GetComponent<Position>();

        if (resource == null)
            return BehaviourStatus.Failed;

        behavior.HarvestedResource = resource;
        pathFollow.Path = context.GameWorld.FindPath(position.Coordinates, resourcePosition.Coordinates);
        pathFollow.PathIndex = 0;

        return BehaviourStatus.Succeeded;
    }

    private static BehaviourStatus MoveTo(VillagerContext context)
    {
        PathFollow pathFollow = context.Entity.GetComponent<PathFollow>();

        if (pathFollow.PathIndex == pathFollow.Path.Length)
            return BehaviourStatus.Succeeded;

        return BehaviourStatus.Running;
    }

    private static BehaviourStatus HarvestResource(VillagerContext context)
    {
        ref VillagerBehavior behavior = ref context.Entity.GetComponent<VillagerBehavior>();

        behavior.HarvestedResource.Destroy();

        return BehaviourStatus.Succeeded;
    }
    #endregion
}
