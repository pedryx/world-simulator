using KdTree;
using KdTree.Math;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;
using WorldSimulator.Villages;

namespace WorldSimulator.Level;
internal class GameWorld
{
    /// <summary>
    /// The name of the shader used for rendering a terrain.
    /// </summary>
    public const string TerrainDrawShader = "terrainDraw";
    /// <summary>
    /// Name of the shader used for the generation of terrain.
    /// </summary>
    public const string TerrainGenShader = "terrainGen";

    /// <summary>
    /// The width and height of the game world in pixels.
    /// </summary>
    public static readonly Point Size = new(8192);
    /// <summary>
    /// The bounding rectangle of the game world.
    /// </summary>
    public static readonly Rectangle Bounds = new(Point.Zero, Size);
    /// <summary>
    /// The total size (width * height) of the game world in pixels.
    /// </summary>
    public static readonly int TotalSize = Size.X * Size.Y;

    /// <summary>
    /// Contain the ID of a terrain type for each grid point.
    /// </summary>
    private readonly int[] terrains;
    /// <summary>
    /// Contains KD-tree for each resource type.
    /// </summary>
    private readonly Dictionary<ResourceType, KdTree<float, IEntity>> resources;
    private readonly GameWorldGrid grid;

    /// <param name="terrains">Contain the ID of a terrain type for each grid point.</param>
    public GameWorld(int[] terrains)
    {
        Debug.Assert(terrains.Length != TotalSize);

        this.terrains = terrains;

        grid = new GameWorldGrid(this);
        resources = ResourceType.GetAll().ToDictionary
        (
            type => type,
            type => new KdTree<float, IEntity>(2, new FloatMath())
        );
    }

    /// <summary>
    /// Add a resource to the game world. The resource will be stored in the corresponding KD-tree.
    /// </summary>
    /// <param name="resourceType">The type of the resource to add.</param>
    /// <param name="entity">The entity representing the resource to add.</param>
    /// <param name="position">The position into which add the resource.</param>
    public void AddResource(ResourceType resourceType, IEntity entity, Vector2 position)
    {
        Debug.Assert(resourceType != null);
        Debug.Assert(resources.ContainsKey(resourceType));
        Debug.Assert(!entity.IsDestroyed());
        Debug.Assert(Bounds.Contains(position));

        resources[resourceType].Add(position.ToFloat(), entity);
    }

    /// <summary>
    /// Get a resource nearest to a specified position and remove it from the KD-tree.
    /// </summary>
    /// <param name="type">The type of the resource to get.</param>
    /// <returns>
    /// The entity representing the resource neared to the specified position, or null if there is no resource of the
    /// specified resource type in the game world.
    /// </returns>
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
    /// Update the position of a resource in the corresponding KD-tree.
    /// </summary>
    /// <param name="type">The type of resource to update.</param>
    /// <param name="entity">The entity representing resource to update.</param>
    /// <param name="oldPosition">The old position of the resource to update.</param>
    /// <param name="newPosition">The new position of the resource to update.</param>
    public void UpdateResourcePosition(ResourceType type, IEntity entity, Vector2 oldPosition, Vector2 newPosition)
    {
        var resourceTree = resources[type];

        if (resourceTree.TryFindValueAt(oldPosition.ToFloat(), out _))
        {
            resourceTree.RemoveAt(oldPosition.ToFloat());
        }
        resourceTree.Add(newPosition.ToFloat(), entity);
    }

    /// <returns>The array representing the path including the specified start and end points.</returns>
    public Vector2[] FindPath(Vector2 start, Vector2 end)
    {
        if (RayCast(start, end))
            return new Vector2[] { start, end };

        return grid.FindPath(start, end);
    }

    /// <summary>
    /// Ray cast from the start location to the end location. Determine if there is non-walkable terrain that
    /// intersects with the ray.
    /// </summary>
    /// <param name="start">The start location of ray cast.</param>
    /// <param name="end">The end location of ray cast.</param>
    /// <returns>True if there is no non-walkable terrain that intersects with ray cast, otherwise false.</returns>
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

            if (!GetTerrain(position).Walkable)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Get terrain type at a specified position.
    /// </summary>
    /// <returns>
    /// The terrain type at the specified position or null if the specified position is outside of the game world
    /// bounds.
    /// </returns>
    public TerrainType GetTerrain(Vector2 position)
    {
        if (!Bounds.Contains(position))
            return null;

        Vector2 point = GameWorldGrid.GetClosestPoint(position);
        int index = ((int)point.Y * Size.X + (int)point.X) / GameWorldGrid.Distance;

        return TerrainType.Get(terrains[index]);
    }

    /// <summary>
    /// Determine if a terrain is walkable at a specified position.
    /// </summary>
    public bool IsWalkable(Vector2 position)
    {
        TerrainType terrain = GetTerrain(position);

        return terrain != null && terrain.Walkable;
    }

    public bool IsBuildable(Vector2 position)
    {
        TerrainType terrain = GetTerrain(position);

        return terrain != null && terrain.Buildable;
    }
}
