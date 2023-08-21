using Microsoft.Xna.Framework;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Systems;
internal readonly struct MovementSystem : IEntityProcessor<Location, Movement>
{
    public void Process(ref Location location, ref Movement movement, float deltaTime)
    {
        if (movement.Destination == location.Position)
            return;

        location.Position += Vector2.Normalize(movement.Destination - location.Position) 
            * movement.Speed * deltaTime;
    }
}
