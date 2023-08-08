using KdTree;

using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;

namespace WorldSimulator.Level;
internal class GameWorld
{
    // TODO: change back to original
    /// <summary>
    /// Width and heigt of world in pixels.
    /// </summary>
    public const int Size = 8192 / 4;

    /// <summary>
    /// Contains terrain information for each pixel of the map.
    /// </summary>
    private readonly TerrainType[][] terrainMap;
    /// <summary>
    /// Grid used for path-finding of moving entities.
    /// </summary>
    private readonly Graph graph;
    /// <summary>
    /// Contains mapping between resource types and kd-trees of resources of corresponding resource type. These
    /// kd-trees are used for finding nearest resource (<see cref="GetNearestResource(ResourceType, Vector2)"/>).
    /// </summary>
    private readonly IDictionary<ResourceType, KdTree<float, IEntity>> resources;

    public Rectangle Bounds { get; private set; }

    public IEntity[][] Chunks { get; private set; }

    public GameWorld
    (
        IEntity[][] chunks,
        TerrainType[][] terrainMap,
        Graph graph,
        IDictionary<ResourceType, KdTree<float, IEntity>> resources
    )
    {
        this.terrainMap = terrainMap;
        this.graph = graph;
        this.resources = resources;
        
        Chunks = chunks;
        Bounds = new Rectangle(Point.Zero, new Point(Size));
    }

    /// <summary>
    /// Get resource nearest to specific position.
    /// </summary>
    /// <param name="type">Type of resource to get.</param>
    public IEntity GetNearestResource(ResourceType type, Vector2 position)
        => resources[type].GetNearestNeighbours(position.ToFloat(), 1).FirstOrDefault()?.Value;

    /// <summary>
    /// Update resource's position in kd-tree.
    /// </summary>
    /// <param name="type">Type of resource to update.</param>
    /// <param name="entity">Entity representing resource to update.</param>
    /// <param name="oldPosition">Old position of entity to update.</param>
    /// <param name="newPosition">New position of entity to update.</param>
    public void UpdateResourcePosition(ResourceType type, IEntity entity, Vector2 oldPosition, Vector2 newPosition)
    {
        var resourceTree = resources[type];

        if (resourceTree.TryFindValueAt(oldPosition.ToFloat(), out _))
        {
            resourceTree.RemoveAt(oldPosition.ToFloat());
        }
        resourceTree.Add(newPosition.ToFloat(), entity);
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
        if (!Bounds.Contains(position))
            return false;

        return terrainMap[(int)position.Y][(int)position.X].Walkable;
    }

    /// <summary>
    /// Determine if specific position is walkable for animals. Animals can walk only on plains.
    /// </summary>
    public bool IsAnimalWalkable(Vector2 position)
        => IsWalkable(position) && terrainMap[(int)position.Y][(int)position.X] == TerrainTypes.Plain;
}
