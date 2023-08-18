using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Adds behavior of villager to entity.
/// </summary>
[Component]
internal struct VillagerBehavior
{
    public int VillageID;
    public IEntity HarvestedResource;
    public float ellapsedWait;
}
// 8162 6050