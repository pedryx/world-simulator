using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WorldSimulator.BehaviorTrees.Nodes.Leaf;

namespace WorldSimulator.BehaviorTrees;
internal class BehaviorTreeBuilder<TContext>
{
    private readonly List<RefWrapper<BehaviorTreeNodeDescriptor<TContext>>> nodes = new();
    private readonly Stack<NodeData> stack = new();

    private BehaviorTreeBuilder() { }

    public static BehaviorTreeBuilder<TContext> Create()
        => new();

    public BehaviorTree<TContext> Build()
    {
        if (stack.Any())
            throw new InvalidOperationException("There was still some composite node which has not been ended!");

        return new(nodes.Select(w => w.Value).ToArray());
    }

    public BehaviorTreeBuilder<TContext> End()
    {
        nodes[stack.Peek().ID].Value.Data = stack.Peek().Children.ToArray();
        stack.Pop();

        return this;
    }

    public BehaviorTreeBuilder<TContext> Sequence()
    {
        nodes.Add(new RefWrapper<BehaviorTreeNodeDescriptor<TContext>>()
        {
            Value = new BehaviorTreeNodeDescriptor<TContext>()
            {
                Type = BehaviorTreeNodeType.Sequence,
                ID = nodes.Count,
            }
        });

        if (stack.Count > 0)
            stack.Peek().Children.Add(nodes.Count - 1);
        stack.Push(new NodeData(nodes.Count - 1));

        return this;
    }

    public BehaviorTreeBuilder<TContext> Selector()
    {
        nodes.Add(new RefWrapper<BehaviorTreeNodeDescriptor<TContext>>()
        {
            Value = new BehaviorTreeNodeDescriptor<TContext>()
            {
                Type = BehaviorTreeNodeType.Selector,
                ID = nodes.Count,
            }
        });

        if (stack.Count > 0)
            stack.Peek().Children.Add(nodes.Count - 1);
        stack.Push(new NodeData(nodes.Count - 1));

        return this;
    }

    public BehaviorTreeBuilder<TContext> Inverter(Func<TContext, BehaviorTreeNodeState> action)
    {
        nodes.Add(new RefWrapper<BehaviorTreeNodeDescriptor<TContext>>()
        {
            Value = new BehaviorTreeNodeDescriptor<TContext>()
            {
                Type = BehaviorTreeNodeType.Action,
                ID = nodes.Count,
                Data = action,
            }
        });

        if (stack.Count > 0)
            stack.Peek().Children.Add(nodes.Count - 1);

        return this;
    }

    public BehaviorTreeBuilder<TContext> Action(Func<TContext, BehaviorTreeNodeState> action)
    {
        nodes.Add(new RefWrapper<BehaviorTreeNodeDescriptor<TContext>>()
        {
            Value = new BehaviorTreeNodeDescriptor<TContext>()
            {
                Type = BehaviorTreeNodeType.Action,
                ID = nodes.Count,
                Data = action,
            }
        });

        if (stack.Count > 0)
            stack.Peek().Children.Add(nodes.Count - 1);

        return this;
    }

    public BehaviorTreeBuilder<TContext> Condition(Func<TContext, BehaviorTreeNodeState> action)
    {
        nodes.Add(new RefWrapper<BehaviorTreeNodeDescriptor<TContext>>()
        {
            Value = new BehaviorTreeNodeDescriptor<TContext>()
            {
                Type = BehaviorTreeNodeType.Action,
                ID = nodes.Count,
                Data = action,
            }
        });

        if (stack.Count > 0)
            stack.Peek().Children.Add(nodes.Count - 1);

        return this;
    }

    public BehaviorTreeBuilder<TContext> Condition(Func<TContext, bool> predicate)
    {
        nodes.Add(new RefWrapper<BehaviorTreeNodeDescriptor<TContext>>()
        {
            Value = new BehaviorTreeNodeDescriptor<TContext>()
            {
                Type = BehaviorTreeNodeType.Action,
                ID = nodes.Count,
                Data = predicate,
            }
        });

        if (stack.Count > 0)
            stack.Peek().Children.Add(nodes.Count - 1);

        return this;
    }

    public BehaviorTreeBuilder<TContext> Wait(float waitTime)
    {
        nodes.Add(new RefWrapper<BehaviorTreeNodeDescriptor<TContext>>()
        {
            Value = new BehaviorTreeNodeDescriptor<TContext>()
            {
                Type = BehaviorTreeNodeType.Action,
                ID = nodes.Count,
                Data = new WaitNodeData(waitTime),
            }
        });

        if (stack.Count > 0)
            stack.Peek().Children.Add(nodes.Count - 1);

        return this;
    }

    private class NodeData
    {
        public readonly List<int> Children = new();
        public readonly int ID;

        public NodeData(int id)
        {
            ID = id;
        }
    }
}
