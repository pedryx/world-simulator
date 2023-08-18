using KdTree;

using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;
using WorldSimulator.Villages;

namespace WorldSimulator.Level;
internal class GameWorld
{
    /// <summary>
    /// Name of the terrain draw shader.
    /// </summary>
    public const string TerrainDrawShader = "terrainDraw";
    /// <summary>
    /// Name of the terrain generation shader.
    /// </summary>
    public const string TerrainGenShader = "terrainGen";

    /// <summary>
    /// Width and height of game world in pixels.
    /// </summary>
    public static readonly Point Size = new(8192);
    public static readonly Rectangle Bounds = new(Point.Zero, Size);
    /// <summary>
    /// Total size (width * height) of game world in pixels.
    /// </summary>
    public static readonly int TotalSize = Size.X * Size.Y;

    private readonly int[] terrains;
    private readonly IDictionary<Resource, KdTree<float, IEntity>> resources;
    private readonly IList<Village> villages;
    private readonly GameWorldGrid grid;

    public GameWorld
    (
        int[] terrains,
        IDictionary<Resource, KdTree<float, IEntity>> resources,
        IList<Village> villages
    )
    {
        this.terrains = terrains;
        this.resources = resources;
        this.villages = villages;

        grid = new GameWorldGrid(this);
    }

    public Village GetVillage(int id)
        => villages[id];

    /// <summary>
    /// Get resource nearest to specific position and remove it from kd-tree.
    /// </summary>
    /// <param name="type">Type of resource to get.</param>
    public IEntity GetAndRemoveNearestResource(Resource type, Vector2 position)
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
    public void UpdateResourcePosition(Resource type, IEntity entity, Vector2 oldPosition, Vector2 newPosition)
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

        return GetTerrainType(position).Walkable;
    }

    /// <summary>
    /// Determine if specific position is walkable for animals. Animals can walk only on plains.
    /// </summary>
    public bool IsWalkableForAnimals(Vector2 position)
        => IsWalkable(position) && GetTerrainType(position) == Terrain.Plain;

    public Vector2[] FindPath(Vector2 start, Vector2 end)
    {
        if (RayCast(start, end))
            return new Vector2[] { start, end };

        return grid.FindPath(start, end);
    }

    /// <summary>
    /// Ray trace from the start point to the end point. Determine if there is non-walkable terrain which intersect
    /// with the ray.
    /// </summary>
    /// <param name="start">Start point of ray cast.</param>
    /// <param name="end">End pop int of ray cast.</param>
    /// <returns>True if there is no non-walkable terrain which intersect with ray cast, otherwise false.</returns>
    public bool RayCast(Vector2 start, Vector2 end)
    {
        float distance = Vector2.Distance(start, end);

        if (distance <= GameWorldGrid.Distance)
            return true;

        int sampleCount = (int)(distance / GameWorldGrid.Distance);
        Vector2 direction = GameWorldGrid.Distance * Vector2.Normalize(end - start);

        for (int i = 0; i < sampleCount; i++)
        {
            Vector2 position = start + direction * i;

            if (!IsWalkable(position))
                return false;
        }

        return true;
    }


    /// <summary>
    /// Get terrain type at specific position.
    /// </summary>
    private Terrain GetTerrainType(Vector2 position)
    {
        Vector2 point = GameWorldGrid.GetClosestPoint(position);
        int index = ((int)point.Y * Size.X + (int)point.X) / GameWorldGrid.Distance;

        return Terrain.Get(terrains[index]);
    }
}
