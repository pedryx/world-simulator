using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components.Villages;
/// <summary>
/// Adds villager behavior to an entity.
/// </summary>
[Component]
internal struct VillagerBehavior
{
    /// <summary>
    /// The village to which villager belongs.
    /// </summary>
    public IEntity Village;
    public int BehaviorTreeIndex = -1;
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
    /// <summary>
    /// Stockpile into which villager should store harvested resources.
    /// </summary>
    public IEntity Stockpile;

    public VillagerBehavior() { }
}
