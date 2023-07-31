using Microsoft.Xna.Framework;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Systems;
internal struct PathFollowSystem : IEntityProcessor<Transform, Movement, PathFollow>
{
    public void Process(ref Transform transform, ref Movement movement, ref PathFollow pathFollow, float deltaTime)
    {
        // check if we are clone enough to next path node
        if (Vector2.Distance(transform.Position, pathFollow.Path[pathFollow.Current]) <= movement.Speed * deltaTime)
        {
            // advance to the next path node
            pathFollow.Current++;
            // set new destination only if we are not at the end of path
            if (pathFollow.Current < pathFollow.Path.Length)
            {
                movement.Destination = pathFollow.Path[pathFollow.Current];
            }
        }
    }
}
