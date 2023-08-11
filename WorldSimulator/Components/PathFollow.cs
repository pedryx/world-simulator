using Microsoft.Xna.Framework;

using System;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Entities with this component will follow specific path.
/// </summary>
[Component]
internal struct PathFollow
{
    public Vector2[] Path = Array.Empty<Vector2>();
    /// <summary>
    /// Index of current path node (element of <see cref="Path"/> array).
    /// </summary>
    public int PathIndex;

    public PathFollow() { }
}
