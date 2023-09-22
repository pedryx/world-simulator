using BehaviourTree;

using WorldSimulator.Components;
using WorldSimulator.Components.Villages;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Level;
using WorldSimulator.Villages;

namespace WorldSimulator.Systems.Villaages;
/// <summary>
/// A system that handles the behavior of villagers.
/// </summary>
internal readonly struct VillagerBehaviorSystem : IEntityProcessor<VillagerBehavior, Owner>
{
    private readonly GameWorld gameWorld;
    private readonly BehaviorTrees behaviorTrees;

    public VillagerBehaviorSystem(GameWorld gameWorld, BehaviorTrees behaviorTrees)
    {
        this.gameWorld = gameWorld;
        this.behaviorTrees = behaviorTrees;
    }

    public void Process(ref VillagerBehavior behavior, ref Owner owner, float deltaTime)
    {
        if (behavior.BehaviorTreeIndex == -1)
            return;

        BehaviourStatus result;
        IBehaviour<VillagerContext> behaviorTree = behaviorTrees.GetBehaviorTree(behavior.BehaviorTreeIndex);

        do
        {
            result = behaviorTree.Tick(new VillagerContext(owner.Entity, gameWorld, deltaTime));
        }
        while (result == BehaviourStatus.Succeeded);
    }
}
