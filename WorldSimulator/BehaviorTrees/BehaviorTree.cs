using System;
using System.Collections.Generic;
using WorldSimulator.BehaviorTrees.Nodes.Composite;
using WorldSimulator.BehaviorTrees.Nodes.Decorator;
using WorldSimulator.BehaviorTrees.Nodes.Leaf;

namespace WorldSimulator.BehaviorTrees;
/// <summary>
/// Represent stateless behavior tree. State of behavior tree node is represented as <see cref="int[]"/>, where each
/// element is index of current child of node with ID same as index of element.
/// </summary>
/// <typeparam name="TContext">Type used for arguments for actions on leaf nodes.</typeparam>
internal class BehaviorTree<TContext>
{
    private readonly BehaviorTreeNodeDescriptor<TContext>[] descriptors;
    private readonly Stack<int> stack = new();

    public BehaviorTree(BehaviorTreeNodeDescriptor<TContext>[] descriptors)
    {
        this.descriptors = descriptors;
    }

    public BehaviorTreeNodeState Update(TContext context, int[] state, float deltaTime)
    {
        // 0 is ID of root node
        stack.Push(0);
        BehaviorTreeNodeState nodeState = UpdateNode(context, state, deltaTime);
        stack.Pop();

        return nodeState;
    }

    /// <summary>
    /// Call update method on child node.
    /// </summary>
    /// <param name="childIndex">Index of child on which to call update method.</param>
    public BehaviorTreeNodeState UpdateChild(int childIndex, TContext context, int[] state, float deltaTime)
    {
        int currentID = stack.Peek();
        int childID = ((int[])descriptors[currentID].Data)[childIndex];

        state[currentID] = childIndex;
        stack.Push(childID);

        BehaviorTreeNodeState nodeState = UpdateNode(context, state, deltaTime);

        stack.Pop();

        return nodeState;
    }

    /// <summary>
    /// Get index of current child (used for indexing childs of current node).
    /// </summary>
    public int GetChildIndex(int[] state)
        => state[stack.Peek()];

    /// <summary>
    /// Get number of children of current node.
    /// </summary>
    public int GetChildrenCount()
        => ((int[])descriptors[stack.Peek()].Data).Length;

    /// <summary>
    /// Reset state of current node.
    /// </summary>
    public void ResetNode(int[] state)
        => state[stack.Peek()] = 0;

    /// <summary>
    /// Get data of current node.
    /// </summary>
    public object GetData()
        => descriptors[stack.Peek()].Data;

    /// <summary>
    /// Set data for current node.
    /// </summary>
    /// <param name="data">Data to set.</param>
    public void SetData(object data)
        => descriptors[stack.Peek()].Data = data;

    /// <summary>
    /// Call update method on current node.
    /// </summary>
    private BehaviorTreeNodeState UpdateNode(TContext context, int[] state, float deltaTime)
    {
        return descriptors[stack.Peek()].Type switch
        {
            BehaviorTreeNodeType.Sequence  => SequenceNode<TContext> .Update(this, context, state, deltaTime),
            BehaviorTreeNodeType.Selector  => SelectorNode<TContext> .Update(this, context, state, deltaTime),

            BehaviorTreeNodeType.Inverter  => InverterNode<TContext> .Update(this, context, state, deltaTime),

            BehaviorTreeNodeType.Action    => ActionNode<TContext>   .Update(this, context,        deltaTime),
            BehaviorTreeNodeType.Condition => ConditionNode<TContext>.Update(this, context,        deltaTime),
            BehaviorTreeNodeType.Wait      => WaitNode<TContext>     .Update(this,                 deltaTime),

            _ => throw new InvalidOperationException("Unsupported node type!"),
        };
    }
}