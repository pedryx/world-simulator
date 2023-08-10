using Microsoft.Xna.Framework;

using WorldSimulator.BehaviorTrees;
using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Level;

namespace WorldSimulator.Systems;
/// <summary>
/// Handles AI behavior of villagers.
/// </summary>
internal readonly struct VillagerBehaviorSystem : IEntityProcessor<VillagerBehavior, Owner>
{
    private readonly GameWorld gameWorld;
    private readonly BehaviorTree<VillagerContext> tree;

    public VillagerBehaviorSystem(GameWorld gameWorld)
    {
        this.gameWorld = gameWorld;

        tree = CreateTree();
    }

    public void Process(ref VillagerBehavior behavior, ref Owner owner, float deltaTime)
    {
        behavior.TreeState ??= new int[tree.NodeCount];
        tree.Update(new VillagerContext(owner.Entity, gameWorld), behavior.TreeState, deltaTime);
    }

    private BehaviorTree<VillagerContext> CreateTree()
    {
        return BehaviorTreeBuilder<VillagerContext>.Create()
             .Sequence()
                 .Action(FindNearestResource)
                 .Action(MoveTo)
                 .Wait(5.0f)
                 .Action(HarvestResource)
             .End()
             .Build();
    }

    #region behavior tree actions and conditions
    private static BehaviorTreeNodeState FindNearestResource(VillagerContext context, float deltaTime)
    {
        ref Position position = ref context.Entity.GetComponent<Position>();
        ref VillagerBehavior behavior = ref context.Entity.GetComponent<VillagerBehavior>();

        IEntity resource = context.GameWorld.GetAndRemoveNearestResource(ResourceTypes.Tree, position.Coordinates);

        if (resource == null)
            return BehaviorTreeNodeState.Failure;

        behavior.MovementTarget = resource;
        return BehaviorTreeNodeState.Success;
    }

    private static BehaviorTreeNodeState MoveTo(VillagerContext context, float deltaTime)
    {
        ref Position position = ref context.Entity.GetComponent<Position>();
        ref Movement movement = ref context.Entity.GetComponent<Movement>();
        ref VillagerBehavior behavior = ref context.Entity.GetComponent<VillagerBehavior>();

        movement.Destination = behavior.MovementTarget.GetComponent<Position>().Coordinates;

        if (Vector2.Distance(position.Coordinates, movement.Destination) < movement.Speed * deltaTime)
        {
            return BehaviorTreeNodeState.Success;
        }

        return BehaviorTreeNodeState.Running;
    }

    private static BehaviorTreeNodeState HarvestResource(VillagerContext context, float deltaTime)
    {
        ref VillagerBehavior behavior = ref context.Entity.GetComponent<VillagerBehavior>();

        behavior.MovementTarget.Destroy();

        return BehaviorTreeNodeState.Success;
    }
    #endregion

    /// <summary>
    /// Context used for passing arguments to behavior tree.
    /// </summary>
    internal class VillagerContext
    {
        public IEntity Entity { get; private set; }
        public GameWorld GameWorld { get; private set; }

        public VillagerContext(IEntity entity, GameWorld gameWorld)
        {
            Entity = entity;
            GameWorld = gameWorld;
        }
    }
}
