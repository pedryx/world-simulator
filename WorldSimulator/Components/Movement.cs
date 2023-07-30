using Microsoft.Xna.Framework;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
[Component]
internal struct Movement
{
    /// <summary>
    /// Movement speed in pixels per second.
    /// </summary>
    public float Speed;
    /// <summary>
    /// Unit vector, which represent movement direction.
    /// </summary>
    public Vector2 Direction;

    public Movement(float speed = 0.0f, Vector2 direction = default) 
    {
        Speed = speed;
        Direction = direction;
    }
}
