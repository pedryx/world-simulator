using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components.Villages;
[Component]
internal struct VillagerBehavior
{
    public int TargetID = -1;

    public VillagerBehavior() { }
}
