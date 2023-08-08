using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Contains information about owner of this component.
/// </summary>
[Component]
internal struct Owner
{
    /// <summary>
    /// Entity which owns this component.
    /// </summary>
    public IEntity Entity;
}
