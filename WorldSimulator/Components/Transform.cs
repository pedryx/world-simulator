using Microsoft.Xna.Framework;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Transform component.
/// </summary>
[Component]
public struct Transform
{
    /// <summary>
    /// Position of the entity.
    /// </summary>
    public Vector2 Position;
    /// <summary>
    /// Scale of the entity.
    /// </summary>
    public float Scale = 1.0f;
    /// <summary>
    /// Rotation of the entity.
    /// </summary>
    public float Rotation;

    public Transform() { }

    public Transform(Vector2 position)
    {
        Position = position;
    }
}
