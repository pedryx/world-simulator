using Microsoft.Xna.Framework;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Systems;
internal readonly struct MovementSystem : IEntityProcessor<Position, Movement>
{
    public void Process(ref Position position, ref Movement movement, float deltaTime)
    {
        if (movement.Destination == position.Coordinates)
            return;

        position.Coordinates += Vector2.Normalize(movement.Destination - position.Coordinates) 
            * movement.Speed * deltaTime;
    }
}
