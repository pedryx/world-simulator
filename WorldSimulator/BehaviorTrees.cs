using BehaviourTree;

using System.Collections.Generic;
using System.Diagnostics;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator;
/// <summary>
/// Manages behavior trees for game state.
/// </summary>
internal class BehaviorTrees
{
    private readonly List<Pair> pairs = new();

    /// <summary>
    /// Set a behavior tree to an entity.
    /// </summary>
    public void SetBehaviorTree(IEntity entity, IBehaviour<BehaviorContext> behaviorTree)
    {
        ref Behavior behavior = ref entity.GetComponent<Behavior>();

        if (behavior.BehaviorTreeIndex != -1)
        {
            RemoveBehaviorTree(entity);
        }

        entity.GetComponent<Behavior>().BehaviorTreeIndex = pairs.Count;
        pairs.Add(new Pair()
        {
            Entity = entity,
            BehaviorTree = behaviorTree
        });
    }

    /// <summary>
    /// Remove the behavior tree of an entity.
    /// </summary>
    public void RemoveBehaviorTree(IEntity entity)
    {
        Debug.Assert(entity.HasComponent<Behavior>());

        ref Behavior behavior = ref entity.GetComponent<Behavior>();
        int index = behavior.BehaviorTreeIndex;

        pairs[index] = pairs[^1];
        pairs[index].Entity.GetComponent<Behavior>().BehaviorTreeIndex = index;

        pairs.RemoveAt(pairs.Count - 1);
        behavior.BehaviorTreeIndex = -1;
    }

    /// <summary>
    /// Get the behavior tree with a specified index.
    /// </summary>
    public IBehaviour<BehaviorContext> GetBehaviorTree(int index)
        => pairs[index].BehaviorTree;

    private record struct Pair(IEntity Entity, IBehaviour<BehaviorContext> BehaviorTree);
}
