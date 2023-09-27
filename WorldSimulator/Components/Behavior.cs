using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
[Component]
internal struct Behavior
{
    public int BehaviorTreeIndex = -1;

    public Behavior() { }
}
