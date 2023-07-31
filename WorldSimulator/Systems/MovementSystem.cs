using Microsoft.Xna.Framework;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Systems;
internal readonly struct MovementSystem : IEntityProcessor<Transform, Movement>
{
    public void Process(ref Transform transform, ref Movement movement, float deltaTime)
    {
        if (movement.Destination == transform.Position)
            return;

        transform.Position += Vector2.Normalize(movement.Destination - transform.Position) 
            * movement.Speed * deltaTime;
    }
}
