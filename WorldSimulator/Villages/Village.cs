using BehaviourTree;
using BehaviourTree.FluentBuilder;

using Microsoft.Xna.Framework;

using System.Collections.Generic;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;

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
        ref Position position = ref context.Entity.GetComponent<Position>();
        ref VillagerBehavior behavior = ref context.Entity.GetComponent<VillagerBehavior>();

        IEntity resource = context.GameWorld.GetAndRemoveNearestResource(ResourceTypes.Tree, position.Coordinates);

        if (resource == null)
            return BehaviourStatus.Failed;

        behavior.MovementTarget = resource;
        return BehaviourStatus.Succeeded;
    }

    private static BehaviourStatus MoveTo(VillagerContext context)
    {
        ref Position position = ref context.Entity.GetComponent<Position>();
        ref Movement movement = ref context.Entity.GetComponent<Movement>();
        ref VillagerBehavior behavior = ref context.Entity.GetComponent<VillagerBehavior>();

        movement.Destination = behavior.MovementTarget.GetComponent<Position>().Coordinates + new Vector2(0.0f, 1.0f);

        if (position.Coordinates.IsCloseEnough(movement.Destination, movement.Speed * context.DeltaTime))
        {
            position.Coordinates = movement.Destination;
            return BehaviourStatus.Succeeded;
        }

        return BehaviourStatus.Running;
    }

    private static BehaviourStatus HarvestResource(VillagerContext context)
    {
        ref VillagerBehavior behavior = ref context.Entity.GetComponent<VillagerBehavior>();

        behavior.MovementTarget.Destroy();

        return BehaviourStatus.Succeeded;
    }
    #endregion
}
