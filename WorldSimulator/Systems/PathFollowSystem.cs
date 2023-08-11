using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;

namespace WorldSimulator.Systems;
internal struct PathFollowSystem : IEntityProcessor<Position, Movement, PathFollow>
{
    public void Process(ref Position position, ref Movement movement, ref PathFollow pathFollow, float deltaTime)
    {
        if (pathFollow.PathIndex == pathFollow.Path.Length)
            return;

        if (position.Coordinates.IsCloseEnough(pathFollow.Path[pathFollow.PathIndex], movement.Speed * deltaTime))
        {
            position.Coordinates = movement.Destination;

            pathFollow.PathIndex++;
            if (pathFollow.PathIndex != pathFollow.Path.Length)
            {
                movement.Destination = pathFollow.Path[pathFollow.PathIndex];
            }
        }
    }
}
