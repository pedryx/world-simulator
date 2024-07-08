using Microsoft.Xna.Framework;

using System.Runtime.CompilerServices;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;
using WorldSimulator.ManagedDataManagers;

namespace WorldSimulator.Systems;
internal readonly struct PathFollowSystem : IEntityProcessor<Location, Movement, PathFollow>
{
    private readonly ManagedDataManager<Vector2[]> pathManager;

    public PathFollowSystem(Game game)
    {
        pathManager = game.GetManagedDataManager<Vector2[]>();
    }

    [MethodImpl(Game.EntityProcessorInline)]
    public void Process(ref Location location, ref Movement movement, ref PathFollow pathFollow, float deltaTime)
    {
        Vector2[] path = pathManager[pathFollow.PathID];

        if (pathFollow.PathNodeIndex == path.Length)
            return;

        if (location.Position.IsCloseEnough(path[pathFollow.PathNodeIndex], movement.Speed * deltaTime))
        {
            location.Position = movement.Destination;

            pathFollow.PathNodeIndex++;
            if (pathFollow.PathNodeIndex != path.Length)
            {
                movement.Destination = path[pathFollow.PathNodeIndex];
            }
        }
    }
}
