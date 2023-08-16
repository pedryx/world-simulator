using KdTree;

using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;

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

    private readonly int[] terrains;
    private readonly IDictionary<ResourceType, KdTree<float, IEntity>> resources;

    public GameWorld(int[] terrains, IDictionary<ResourceType, KdTree<float, IEntity>> resources)
    {
        this.terrains = terrains;
        this.resources = resources;
    }

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

        return GetTerrainType(position).Walkable;
    }

    /// <summary>
    /// Determine if specific position is walkable for animals. Animals can walk only on plains.
    /// </summary>
    public bool IsWalkableForAnimals(Vector2 position)
        => IsWalkable(position) && GetTerrainType(position) == TerrainTypes.Plain;

    /// <summary>
    /// Get terrain type at specific position.
    /// </summary>
    private TerrainType GetTerrainType(Vector2 position)
    {
        int index = ((int)position.Y * GameWorld.Size.X + (int)position.X) / GameWorldGrid.Distance;
        return TerrainTypes.GetTerrainType(terrains[index]);
    }
}
