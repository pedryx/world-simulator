using Microsoft.Xna.Framework;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Transform component representing the position of an entity in 2D space.
/// </summary>
[Component]
internal struct Position
{
    /// <summary>
    /// Coordinates of the entity's position.
    /// </summary>
    public Vector2 Coordinates;
}
