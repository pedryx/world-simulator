using System;

namespace WorldSimulator.BehaviorTrees.Nodes.Decorator;
/// <summary>
/// Behavior tree node (decorator) which succeeds if child fails and fails if child succeeds.
/// </summary>
internal static class InverterNode<TContext>
{
    public static BehaviorTreeNodeState Update
    (
        BehaviorTree<TContext> tree,
        TContext context,
        int[] state,
        float deltaTime
    )
    {
        BehaviorTreeNodeState nodeState = tree.UpdateChild(0, context, state, deltaTime);

        return nodeState switch
        {
            BehaviorTreeNodeState.Success => BehaviorTreeNodeState.Failure,
            BehaviorTreeNodeState.Failure => BehaviorTreeNodeState.Success,
            BehaviorTreeNodeState.Running => BehaviorTreeNodeState.Running,

            _ => throw new InvalidOperationException("Unsupported node state!"),
        };
    }
}
