using Microsoft.Xna.Framework;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Enables entities to move toward a specified destination.
/// </summary>
[Component]
internal struct Movement
{
    /// <summary>
    /// The movement speed in pixels per second.
    /// </summary>
    public float Speed;
    /// <summary>
    /// Destination towards which the entity will move.
    /// </summary>
    public Vector2 Destination;
}
