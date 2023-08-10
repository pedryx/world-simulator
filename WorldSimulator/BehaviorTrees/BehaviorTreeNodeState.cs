namespace WorldSimulator.BehaviorTrees;
/// <summary>
/// Represent current state of behavior tree node.
/// </summary>
internal enum BehaviorTreeNodeState
{
    /// <summary>
    /// Node successfully finished.
    /// </summary>
    Success,
    /// <summary>
    /// Node failed to finish.
    /// </summary>
    Failure,
    /// <summary>
    /// Node is still running.
    /// </summary>
    Running,
}
