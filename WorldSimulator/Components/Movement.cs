using Microsoft.Xna.Framework;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Entities with this component moves towards <see cref="Destination"/>.
/// </summary>
[Component]
internal struct Movement
{
    /// <summary>
    /// Movement speed in pixels per second.
    /// </summary>
    public float Speed;
    /// <summary>
    /// Destination towards which will entitiy move.
    /// </summary>
    public Vector2 Destination;
}
