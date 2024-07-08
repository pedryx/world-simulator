using Microsoft.Xna.Framework;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Enables an entity to follow a specified path.
/// </summary>
[Component]
internal struct PathFollow
{
    /// <summary>
    /// ID of the specified path which will be followed by the entity.
    /// </summary>
    public int PathID = -1;
    /// <summary>
    /// The index of current path node (element of the <see cref="Path"/> array).
    /// </summary>
    public int PathNodeIndex;

    public PathFollow(Game game)
    {
        PathID = game.GetManagedDataManager<Vector2[]>().Create();
    }
}
