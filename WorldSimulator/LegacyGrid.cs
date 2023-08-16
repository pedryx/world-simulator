using Microsoft.Xna.Framework;

using Priority_Queue;

using System;
using System.Collections.Generic;
using System.Linq;

using WorldSimulator.Level;

namespace WorldSimulator;

/// <summary>
/// Utility class for path-finding in grid.
/// </summary>
internal class LegacyGrid
{
    /// <summary>
    /// Distance between grid points.
    /// </summary>
    private const int gridDistance = 16;
    /// <summary>
    /// Maximum number of nodes which could be in priority queue at one time. This value is needed for
    /// <see cref="FastPriorityQueue{T}"/> to work properly.
    /// </summary>
    private const int maxPriorityQueueNodes = (LegacyGameWorld.Size * LegacyGameWorld.Size) / (gridDistance * gridDistance);

    private readonly LegacyGameWorld gameWorld;

    public LegacyGrid(LegacyGameWorld gameWorld)
    {
        this.gameWorld = gameWorld;
    }

    /// <summary>
    /// Find path from start to end using A* alghoritm.
    /// </summary>
    public Vector2[] FindPath(Vector2 start, Vector2 end)
    {
        // points are not aligned to grid so we get closest grid points to them
        Vector2 gridStart = GetClosestGridPoint(start);
        Vector2 gridEnd = GetClosestGridPoint(end);

        if (gridStart == gridEnd)
            return new Vector2[] { start, gridStart, end };

        FastPriorityQueue<GridPoint> frontier = new(maxPriorityQueueNodes);
        Dictionary<Vector2, Vector2?> cameFrom = new();
        Dictionary<Vector2, float> cost = new();

        frontier.Enqueue(new GridPoint(gridStart), 0.0f);
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
                    frontier.Enqueue(new GridPoint(neighbor), newCost + ManhattanDistance(current, end));
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

        // we connect actual start and end because the used ones were only nearest grid points to them
        return SimplifyPath
        (
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
    /// Calculate manhattan distance between two positions.
    /// </summary>
    private static float ManhattanDistance(Vector2 a, Vector2 b)
        => (MathF.Abs(a.X - b.X) + MathF.Abs(a.Y - b.Y)) / gridDistance;

    /// <summary>
    /// Get all neighbor grid points.
    /// </summary>
    private IEnumerable<Vector2> GetNeighbors(Vector2 current)
    {
        List<Vector2> neighbors = new();

        Vector2 left  = current + new Vector2(-gridDistance,  0.0f        );
        Vector2 right = current + new Vector2( gridDistance,  0.0f        );
        Vector2 top   = current + new Vector2( 0.0f        , -gridDistance);
        Vector2 down  = current + new Vector2( 0.0f        ,  gridDistance);

        // we dont need to check if point is in game world bounds because grid distance is smaller than border size
        if (gameWorld.IsWalkable(left )) neighbors.Add(left );
        if (gameWorld.IsWalkable(right)) neighbors.Add(right);
        if (gameWorld.IsWalkable(top  )) neighbors.Add(top  );
        if (gameWorld.IsWalkable(down )) neighbors.Add(down );

        return neighbors;
    }

    /// <summary>
    /// Get grid point closest to specific position.
    /// </summary>
    private Vector2 GetClosestGridPoint(Vector2 position)
    {
        List<Vector2> points = new();

        Vector2 topLeft = new
        (
            (int)position.X - (int)position.X % gridDistance,
            (int)position.Y - (int)position.Y % gridDistance
        );
        Vector2 topRight = topLeft + new Vector2(gridDistance, 0);
        Vector2 bottomLeft = topLeft + new Vector2(0, gridDistance);
        Vector2 bottomRight = topLeft + new Vector2(gridDistance);

        if (gameWorld.IsWalkable(topLeft    )) points.Add(topLeft    );
        if (gameWorld.IsWalkable(topRight   )) points.Add(topRight   );
        if (gameWorld.IsWalkable(bottomLeft )) points.Add(bottomLeft );
        if (gameWorld.IsWalkable(bottomRight)) points.Add(bottomRight);

        return points
            .Select(p => new { Point = p, Distance = Vector2.DistanceSquared(p, position) })
            .Aggregate((p1, p2) => p1.Distance < p2.Distance ? p1 : p2)
            .Point;
    }

    private class GridPoint : FastPriorityQueueNode
    {
        public readonly Vector2 Position;

        public GridPoint(Vector2 position)
        {
            Position = position;
        }
    }
}
