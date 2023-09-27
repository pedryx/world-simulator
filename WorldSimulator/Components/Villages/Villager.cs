using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components.Villages;
[Component]
internal struct Villager
{
    /// <summary>
    /// The village to which villager belongs.
    /// </summary>
    public IEntity Village;
}
