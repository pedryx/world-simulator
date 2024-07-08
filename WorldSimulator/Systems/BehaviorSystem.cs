using BehaviourTree;

using System.Runtime.CompilerServices;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Level;
using WorldSimulator.ManagedDataManagers;

namespace WorldSimulator.Systems;
/// <summary>
/// A system that handles the behavior of villagers.
/// </summary>
internal readonly struct BehaviorSystem : IEntityProcessor<Behavior, Owner>
{
    private readonly GameWorld gameWorld;
    private readonly BehaviorTrees behaviorTrees;
    private readonly Game game;
    private readonly ManagedDataManager<IEntity> manager;

    public BehaviorSystem(GameWorld gameWorld, BehaviorTrees behaviorTrees, Game game)
    {
        this.gameWorld = gameWorld;
        this.behaviorTrees = behaviorTrees;
        this.game = game;
        manager = game.GetManagedDataManager<IEntity>();
    }

    [MethodImpl(Game.EntityProcessorInline)]
    public void Process(ref Behavior behavior, ref Owner owner, float deltaTime)
    {
        if (behavior.BehaviorTreeIndex == -1)
            return;

        IEntity entity = manager[owner.EntityID];

        BehaviourStatus result;
        IBehaviour<BehaviorContext> behaviorTree = behaviorTrees.GetBehaviorTree(behavior.BehaviorTreeIndex);

        do
        {
            result = behaviorTree.Tick(new BehaviorContext()
            {
                Entity = entity,
                GameWorld = gameWorld,
                DeltaTime = deltaTime,
                Game = game,
            });
        }
        while (result == BehaviourStatus.Succeeded);
    }
}
