using System;

namespace WorldSimulator.BehaviorTrees.Nodes.Leaf;

/// <summary>
/// Behavior tree leaf node which checks some condition
/// </summary>
/// <typeparam name="TContext"></typeparam>
internal static class ConditionNode<TContext>
{
    public static BehaviorTreeNodeState Update(BehaviorTree<TContext> tree, TContext context)
    {
        return ((Func<TContext, bool>)tree.GetData()).Invoke(context)
            ? BehaviorTreeNodeState.Success 
            : BehaviorTreeNodeState.Failure;
    }
}
