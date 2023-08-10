namespace WorldSimulator.BehaviorTrees.Nodes.Composite;
/// <summary>
/// Behavior tree node (composite), which traverses its children from left to right and fails if any child fails and
/// succeeds when all children succeeds.
/// </summary>
internal static class SequenceNode<TContext>
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

            if (nodeState == BehaviorTreeNodeState.Failure)
                tree.ResetNode(state);
            if (nodeState != BehaviorTreeNodeState.Success)
                return nodeState;
        }

        tree.ResetNode(state);
        return BehaviorTreeNodeState.Success;
    }
}
