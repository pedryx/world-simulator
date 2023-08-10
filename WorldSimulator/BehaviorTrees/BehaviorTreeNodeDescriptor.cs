using System;

namespace WorldSimulator.BehaviorTrees;
/// <summary>
/// Describes behavior tree node.
/// </summary>
internal struct BehaviorTreeNodeDescriptor<TContext>
{
    /// <summary>
    /// Type of node.
    /// </summary>
    public BehaviorTreeNodeType Type;
    /// <summary>
    /// ID used as internal index in <see cref="BehaviorTree{TContext}"/>.
    /// </summary>
    public int ID;
    /// <summary>
    /// Data of current node. (Can be IDs of children, action callback, ...)
    /// </summary>
    public object Data;
}
