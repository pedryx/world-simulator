using KdTree;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;
using WorldSimulator.Villages;

namespace WorldSimulator.Level;
internal class LegacyGameWorld
{
    // TODO: change back to original
    /// <summary>
    /// Width and height of the game world.
    /// </summary>
    public const int Size = 8192 / 4;

    /// <summary>
    /// Maps coordinates of each pixel to its terrain type.
    /// </summary>
    private readonly TerrainType[][] terrainMap;
    /// <summary>
    /// Contains mapping between resource types and kd-trees of resources of corresponding resource entities. These
    /// kd-trees are used for finding nearest resource (<see cref="GetNearestAndRemoveResource(ResourceType, Vector2)"/>).
    /// </summary>
    private readonly IDictionary<ResourceType, KdTree<float, IEntity>> resources;
    private readonly IList<Village> villages;
    private readonly LegacyGrid grid;

    /// <summary>
    /// Bounding rectangle of the game world.
    /// </summary>
    public Rectangle Bounds { get; private set; }
    /// <summary>
    /// Contains chunk entities.
    /// </summary>
    public IEnumerable<IEntity> Chunks { get; private set; }

    public LegacyGameWorld
    (
        IEnumerable<IEntity> chunks,
        TerrainType[][] terrainMap,
        IDictionary<ResourceType, KdTree<float, IEntity>> resources,
        IList<Village> villages
    )
    {
        this.terrainMap = terrainMap;
        this.resources = resources;
        this.villages = villages;
        
        Chunks = chunks;
        Bounds = new Rectangle(Point.Zero, new Point(Size));
        grid = new LegacyGrid(this);
    }

    public Village GetVillage(int id)
        => villages[id];

    /// <summary>
    /// Get resource nearest to specific position and remove it from kd-tree.
    /// </summary>
    /// <param name="type">Type of resource to get.</param>
    public IEntity GetAndRemoveNearestResource(ResourceType type, Vector2 position)
    {
        var node = resources[type].GetNearestNeighbours(position.ToFloat(), 1).FirstOrDefault();

        if (node != null)
        {
            resources[type].RemoveAt(node.Point);
        }

        return node?.Value;
    }

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
    public bool IsWalkableForAnimals(Vector2 position)
        => IsWalkable(position) && terrainMap[(int)position.Y][(int)position.X] == TerrainTypes.Plain;

    /// <summary>
    /// Find path from start to end.
    /// </summary>
    public Vector2[] FindPath(Vector2 start, Vector2 end)
    {
        if (RayCast(start, end))
            return new Vector2[] { start, end };

        return grid.FindPath(start, end);
    }

    public bool RayCast(Vector2 start, Vector2 end)
    {
        const float sampleDistance = 16.0f;

        float distance = Vector2.Distance(start, end);

        if (distance <= sampleDistance)
            return true;

        int sampleCount = (int)(distance / sampleDistance);
        Vector2 direction = sampleDistance * Vector2.Normalize(end - start);

        for (int i = 1; i <= sampleCount; i++)
        {
            Vector2 position = start + direction * i;

            if (!IsWalkable(position))
                return false;
        }

        return true;
    }
}
