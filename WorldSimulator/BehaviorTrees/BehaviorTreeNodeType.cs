namespace WorldSimulator.BehaviorTrees;
/// <summary>
/// Type of behavior tree node.
/// </summary>
internal enum BehaviorTreeNodeType
{
    None,

    Sequence,
    Selector,

    Inverter,

    Action,
    Condition,
    Wait,
}
