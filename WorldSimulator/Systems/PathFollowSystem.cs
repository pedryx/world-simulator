using System.Runtime.CompilerServices;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;

namespace WorldSimulator.Systems;
internal readonly struct PathFollowSystem : IEntityProcessor<Location, Movement, PathFollow>
{
    [MethodImpl(Game.EntityProcessorInline)]
    public void Process(ref Location location, ref Movement movement, ref PathFollow pathFollow, float deltaTime)
    {
        if (pathFollow.PathIndex == pathFollow.Path.Length)
            return;

        if (location.Position.IsCloseEnough(pathFollow.Path[pathFollow.PathIndex], movement.Speed * deltaTime))
        {
            location.Position = movement.Destination;

            pathFollow.PathIndex++;
            if (pathFollow.PathIndex != pathFollow.Path.Length)
            {
                movement.Destination = pathFollow.Path[pathFollow.PathIndex];
            }
        }
    }
}
