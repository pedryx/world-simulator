namespace WorldSimulator.BehaviorTrees.Nodes.Leaf;

/// <summary>
/// Leaf node which returns success after waiting for <see cref="WaitNodeData.WaitTime"/> seconds.
/// </summary>
/// <typeparam name="TContext"></typeparam>
internal static class WaitNode<TContext>
{
    public static BehaviorTreeNodeState Update(BehaviorTree<TContext> tree, float deltaTime)
    {
        var data = (WaitNodeData)tree.GetData();

        data.Elapsed += deltaTime;
        if (data.Elapsed >= data.WaitTime)
        {
            data.Elapsed = 0.0f;
            return BehaviorTreeNodeState.Success;
        }

        return BehaviorTreeNodeState.Running;
    }
}

/// <summary>
/// Data for <see cref="WaitNode{TContext}"/>.
/// </summary>
internal class WaitNodeData
{
    /// <summary>
    /// How much time to wait in seconds.
    /// </summary>
    public float WaitTime { get; private set; }
    /// <summary>
    /// How much time elapsed in seconds.
    /// </summary>
    public float Elapsed { get; set; }

    public WaitNodeData(float waitTime)
    {
        WaitTime = waitTime;
    }
}
