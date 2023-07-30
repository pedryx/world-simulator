using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Systems;
internal readonly struct MovementSystem : IEntityProcessor<Transform, Movement>
{
    public void Process(ref Transform transform, ref Movement movement, float deltaTime)
    { 
        transform.Position += movement.Direction * movement.Speed * deltaTime;
    }
}
