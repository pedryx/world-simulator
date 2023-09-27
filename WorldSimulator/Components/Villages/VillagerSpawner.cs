using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components.Villages;
/// <summary>
/// Entities with this component will periodically spawn a villager. Only one villager can be spawned at a time.
/// </summary>
internal struct VillagerSpawner
{
    /// <summary>
    /// The amount of time, in seconds, that has passed since the last villager was spawned.
    /// </summary>
    public float Elapsed = float.MaxValue;
    /// <summary>
    /// The currently spawned villager.
    /// </summary>
    public IEntity Villager;
    /// <summary>
    /// The ID of village where the villager will be spawned.
    /// </summary>
    public IEntity Village;
    public VillagerProfession Profession;

    public VillagerSpawner() { }
}
