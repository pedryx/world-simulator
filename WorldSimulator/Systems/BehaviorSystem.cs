using BehaviourTree;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Level;

namespace WorldSimulator.Systems;
/// <summary>
/// A system that handles the behavior of villagers.
/// </summary>
internal readonly struct BehaviorSystem : IEntityProcessor<Behavior, Owner>
{
    private readonly GameWorld gameWorld;
    private readonly BehaviorTrees behaviorTrees;

    public BehaviorSystem(GameWorld gameWorld, BehaviorTrees behaviorTrees)
    {
        this.gameWorld = gameWorld;
        this.behaviorTrees = behaviorTrees;
    }

    public void Process(ref Behavior behavior, ref Owner owner, float deltaTime)
    {
        if (behavior.BehaviorTreeIndex == -1)
            return;

        BehaviourStatus result;
        IBehaviour<BehaviorContext> behaviorTree = behaviorTrees.GetBehaviorTree(behavior.BehaviorTreeIndex);

        do
        {
            result = behaviorTree.Tick(new BehaviorContext()
            {
                Entity = owner.Entity,
                GameWorld = gameWorld,
                DeltaTime = deltaTime,
            });
        }
        while (result == BehaviourStatus.Succeeded);
    }
}
