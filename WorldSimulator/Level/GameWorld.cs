using Microsoft.Xna.Framework;

using System.Collections.Generic;

namespace WorldSimulator.Level;
internal class GameWorld
{
    /// <summary>
    /// Contains terrain information for each pixel of the map.
    /// </summary>
    private readonly Terrain[][] terrainMap;
    /// <summary>
    /// Grid used for path-finding of moving entities.
    /// </summary>
    private readonly Graph graph;
    private readonly Rectangle worldBounds;

    public GameWorld(Terrain[][] terrainMap, Graph graph)
    {
        this.terrainMap = terrainMap;
        this.graph = graph;
        worldBounds = new Rectangle(Point.Zero, new Point(GameWorldGenerator.WorldSize));
    }

    /// <summary>
    /// Find walable path from start to end.
    /// </summary>
    public IEnumerable<Vector2> FindPath(Vector2 start, Vector2 end)
    {
        Vector2 nearestStart = graph.GetNearest(start);
        Vector2 nearestEnd = graph.GetNearest(end);

        graph.AddEdge(start, nearestStart);
        graph.AddEdge(end, nearestEnd);

        IEnumerable<Vector2> path = graph.FindPath(start, end);

        graph.RemoveEdge(start, nearestStart);
        graph.RemoveEdge(end, nearestEnd);

        return path;
    }

    /// <summary>
    /// Determine if specific position is walkable.
    /// </summary>
    public bool IsWalkable(Vector2 position)
    {
        if (!worldBounds.Contains(position))
            return false;

        return terrainMap[(int)position.Y][(int)position.X].Walkable;
    }

    /// <summary>
    /// Determine if specific position is walkable for animals. Animals can walk only on plains.
    /// </summary>
    public bool IsAnimalWalkable(Vector2 position)
        => IsWalkable(position) && terrainMap[(int)position.Y][(int)position.X] == Terrains.Plain;
}
