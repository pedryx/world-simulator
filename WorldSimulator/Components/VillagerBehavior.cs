using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Adds behavior of villager to entity.
/// </summary>
[Component]
internal struct VillagerBehavior
{
    /// <summary>
    /// ID of village the villager belongs to.
    /// </summary>
    public int VillageID;
    /// <summary>
    /// Time elapsed from waiting begins. Used by wait villager behavior node.
    /// </summary>
    public float elapsedWait;
    public IEntity Target;
}
