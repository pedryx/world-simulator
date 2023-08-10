using System;

namespace WorldSimulator.BehaviorTrees.Nodes.Leaf;

/// <summary>
/// Behavior tree leaf node which checks some condition
/// </summary>
/// <typeparam name="TContext"></typeparam>
internal static class ConditionNode<TContext>
{
    public static BehaviorTreeNodeState Update(BehaviorTree<TContext> tree, TContext context, float deltaTime)
    {
        return ((Func<TContext, float, bool>)tree.GetData()).Invoke(context, deltaTime)
            ? BehaviorTreeNodeState.Success 
            : BehaviorTreeNodeState.Failure;
    }
}
