using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Adds villager behavior to an entity.
/// </summary>
[Component]
internal struct VillagerBehavior
{
    /// <summary>
    /// The ID of the village to which the villager belongs.
    /// </summary>
    public int VillageID;
    /// <summary>
    /// The time elapsed from waiting begins. Used by a wait villager behavior node.
    /// </summary>
    public float ElapsedWait;
    /// <summary>
    /// The current target that the villager is focusing on. This target can be utilized for various purposes such as
    /// interaction, attack, harvest, and more.
    /// </summary>
    public IEntity Target;
    /// <summary>
    /// The place where the villager works.
    /// </summary>
    public IEntity WorkPlace;
}
