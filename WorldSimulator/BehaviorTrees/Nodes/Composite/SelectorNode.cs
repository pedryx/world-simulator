namespace WorldSimulator.BehaviorTrees.Nodes.Composite;
/// <summary>
/// Behavior tree node (composite), which traverses its children from left to right and succeeds if any child succeeds
/// and fails when all children fails.
/// </summary>
internal static class SelectorNode<TContext>
{
    public static BehaviorTreeNodeState Update
    (
        BehaviorTree<TContext> tree,
        TContext context,
        int[] state,
        float deltaTime
    )
    {
        int childrenCount = tree.GetChildrenCount();

        for (int i = tree.GetChildIndex(state); i < childrenCount; i++)
        {
            BehaviorTreeNodeState nodeState = tree.UpdateChild(i, context, state, deltaTime);

            if (nodeState == BehaviorTreeNodeState.Success)
                tree.ResetNode(state);
            if (nodeState != BehaviorTreeNodeState.Failure)
                return nodeState;
        }

        tree.ResetNode(state);
        return BehaviorTreeNodeState.Success;
    }
}
