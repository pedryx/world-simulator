using Microsoft.Xna.Framework;

using System;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Enables an entity to follow a specified path.
/// </summary>
[Component]
internal struct PathFollow
{
    /// <summary>
    /// The specified path which will be followed by the entity.
    /// </summary>
    public Vector2[] Path = Array.Empty<Vector2>();
    /// <summary>
    /// The index of current path node (element of the <see cref="Path"/> array).
    /// </summary>
    public int PathIndex;

    public PathFollow() { }
}
