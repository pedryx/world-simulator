using System;

namespace WorldSimulator.BehaviorTrees.Nodes.Leaf;

/// <summary>
/// Behavior tree leaf node which do some action.
/// </summary>
internal static class ActionNode<TContext>
{
    public static BehaviorTreeNodeState Update(BehaviorTree<TContext> tree, TContext context, float deltaTime)
        => ((Func<TContext, float, BehaviorTreeNodeState>)tree.GetData()).Invoke(context, deltaTime);
}
