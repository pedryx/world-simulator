using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Provides owner information to an entity.
/// </summary>
[Component]
internal struct Owner
{
    /// <summary>
    /// The entity which owns this component.
    /// </summary>
    public IEntity Entity;
}
