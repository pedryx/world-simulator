using Microsoft.Xna.Framework;

using Priority_Queue;

using System;
using System.Collections.Generic;
using System.Linq;

namespace WorldSimulator.Level;
internal class GameWorldGrid
{
    /// <summary>
    /// Maximum number of nodes that can be in a priority queue at one time.
    /// </summary>
    private static readonly int maxPriorityQueueSize = GameWorld.TotalSize / Distance;

    /// <summary>
    /// Calculate the Manhattan distance between two positions.
    /// </summary>
    private static float ManhattanDistance(Vector2 a, Vector2 b)
        => (MathF.Abs(a.X - b.X) + MathF.Abs(a.Y - b.Y)) / Distance;

    /// <summary>
    /// Distance between neighbor grid nodes.
    /// </summary>
    public const int Distance = 16;

    public static Vector2 GetClosestPoint(Vector2 position)
    {
        List<Vector2> points = new();

        Vector2 topLeft = new
        (
            (int)position.X - (int)position.X % Distance,
            (int)position.Y - (int)position.Y % Distance
        );
        Vector2 topRight = topLeft + new Vector2(Distance, 0);
        Vector2 bottomLeft = topLeft + new Vector2(0, Distance);
        Vector2 bottomRight = topLeft + new Vector2(Distance);

        if (GameWorld.Bounds.Contains(topLeft)) points.Add(topLeft);
        if (GameWorld.Bounds.Contains(topRight)) points.Add(topRight);
        if (GameWorld.Bounds.Contains(bottomLeft)) points.Add(bottomLeft);
        if (GameWorld.Bounds.Contains(bottomRight)) points.Add(bottomRight);

        return points
            .Select(p => new { Point = p, Distance = Vector2.DistanceSquared(p, position) })
            .Aggregate((p1, p2) => p1.Distance < p2.Distance ? p1 : p2)
            .Point;
    }

    private readonly GameWorld gameWorld;

    public GameWorldGrid(GameWorld gameWorld)
    {
        this.gameWorld = gameWorld;
    }

    /// <summary>
    /// Find a path from start to end using A* algorithm.
    /// </summary>
    public Vector2[] FindPath(Vector2 start, Vector2 end)
    {
        Vector2 gridStart = GetClosestPoint(start);
        Vector2 gridEnd = GetClosestPoint(end);

        if (gridStart == gridEnd)
            return new Vector2[] { start, gridStart, end };

        FastPriorityQueue<GridNode> frontier = new(maxPriorityQueueSize);
        Dictionary<Vector2, Vector2?> cameFrom = new();
        Dictionary<Vector2, float> cost = new();

        frontier.Enqueue(new GridNode(gridStart), 0.0f);
        cameFrom[gridStart] = null;
        cost[gridStart] = 0.0f;

        while (frontier.Any())
        {
            Vector2 current = frontier.Dequeue().Position;

            if (current == gridEnd)
                break;

            foreach (var neighbor in GetNeighbors(current))
            {
                float newCost = cost[current] + 1.0f;
                if (!cost.ContainsKey(neighbor) || newCost < cost[neighbor])
                {
                    cost[neighbor] = newCost;
                    frontier.Enqueue(new GridNode(neighbor), newCost + ManhattanDistance(current, end));
                    cameFrom[neighbor] = current;
                }
            }
        }

        if (frontier.Count == 0)
            return Array.Empty<Vector2>();

        List<Vector2> path = new();
        Vector2? currentPoint = gridEnd;
        while (currentPoint != null)
        {
            path.Add(currentPoint.Value);
            currentPoint = cameFrom[currentPoint.Value];
        }

        return SimplifyPath
        (
            //Actual start and end don't have to be points of the grid.
            new Vector2[] { start }
                .Concat(((IEnumerable<Vector2>)path).Reverse())
                .Concat(new Vector2[] { end })
                .ToArray()
        );
    }

    private Vector2[] SimplifyPath(Vector2[] path)
    {
        List<Vector2> simplifiedPath = new() { path.First() };
        Vector2 last = path[1];

        for (int i = 1; i < path.Length; i++)
        {
            if (gameWorld.RayCast(simplifiedPath.Last(), path[i]))
            {
                last = path[i];
            }
            else
            {
                simplifiedPath.Add(last);
                if (i + 1 != path.Length)
                    last = path[i + 1];
            }
        }

        simplifiedPath.Add(last);
        return simplifiedPath.ToArray();
    }

    /// <summary>
    /// Get all neighbor grid points.
    /// </summary>
    private IEnumerable<Vector2> GetNeighbors(Vector2 position)
    {
        List<Vector2> neighbors = new();

        Vector2 left = position + Vector2.UnitX * -Distance;
        Vector2 right = position + Vector2.UnitX * Distance;
        Vector2 top = position + Vector2.UnitY * -Distance;
        Vector2 down = position + Vector2.UnitY * Distance;

        if (gameWorld.IsWalkable(left)) neighbors.Add(left);
        if (gameWorld.IsWalkable(right)) neighbors.Add(right);
        if (gameWorld.IsWalkable(top)) neighbors.Add(top);
        if (gameWorld.IsWalkable(down)) neighbors.Add(down);

        return neighbors;
    }

    /// <summary>
    /// Fast priority queue node. Used in path-finding for frontier.
    /// </summary>
    private class GridNode : FastPriorityQueueNode
    {
        public readonly Vector2 Position;

        public GridNode(Vector2 position)
        {
            Position = position;
        }
    }
}
