using KdTree;
using KdTree.Math;

using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;

namespace WorldSimulator;
/// <summary>
/// Represent a graph at 2D plane.
/// </summary>
internal class Graph
{
    /// <summary>
    /// Mapping between 2D plane positions and graph nodes.
    /// </summary>
    private readonly Dictionary<Vector2, Node> nodeDictionary = new();
    /// <summary>
    /// Tree used for finding nearest node.
    /// </summary>
    private readonly KdTree<float, Node> nodeTree = new(2, new FloatMath());

    private void AddNode(Vector2 position)
    {
        if (!nodeDictionary.ContainsKey(position)) 
        {
            Node node = new(position);

            nodeDictionary.Add(position, node);
            nodeTree.Add(new float[] { position.X, position.Y }, node);
        }
    }

    private void RemoveNode(Vector2 position)
    {
        nodeDictionary.Remove(position);
        nodeTree.RemoveAt(new float[] { position.X, position.Y });
    }

    /// <summary>
    /// Get position of node nearest to cpecific position.
    /// </summary>
    public Vector2 GetNearest(Vector2 position)
        => nodeTree.GetNearestNeighbours(new float[] { position.X, position.Y }, 1).First().Value.Position;

    /// <summary>
    /// Add new edge to the graph, also adds nodes for each position.
    /// </summary>
    /// <param name="position1">Position of first node</param>
    /// <param name="position2">Position of second node.</param>
    public void AddEdge(Vector2 position1, Vector2 position2)
    {
        AddNode(position1);
        AddNode(position2);

        Node node1 = nodeDictionary[position1];
        Node node2 = nodeDictionary[position2];

        node1.Neighbors.Add(node2);
        node2.Neighbors.Add(node1);
    }

    /// <summary>
    /// Remove edge from the graph, also removes nodes with zero neighbors.
    /// </summary>
    public void RemoveEdge(Vector2 position1, Vector2 position2)
    {
        Node node1 = nodeDictionary[position1];
        Node node2 = nodeDictionary[position2];

        node1.Neighbors.Remove(node2);
        node2.Neighbors.Remove(node1);

        if (!node1.Neighbors.Any())
        {
            RemoveNode(position1);
        }
        if (!node2.Neighbors.Any())
        {
            RemoveNode(position2);
        }
    }

    // TODO: change to A*
    /// <summary>
    /// Find path in graph using BFS alghoritm.
    /// </summary>
    public IEnumerable<Vector2> FindPath(Vector2 startPosition, Vector2 endPosition)
    {
        // initialize
        Node start = nodeDictionary[startPosition];
        Node end = nodeDictionary[endPosition];

        Queue<Node> frontier = new();
        HashSet<Node> visited = new();
        Dictionary<Node, Node> cameFrom = new();

        frontier.Enqueue(start);
        visited.Add(start);
        cameFrom.Add(start, null);

        // bfs
        while (frontier.Any())
        {
            Node current = frontier.Dequeue();

            if (current == end)
                break;

            foreach (var neighbor in current.Neighbors)
            {
                if (visited.Contains(neighbor))
                    continue;

                frontier.Enqueue(neighbor);
                visited.Add(neighbor);
                cameFrom.Add(neighbor, start);
            }
        }

        // construct path
        List<Vector2> path = new();
        Node node = end;
        while (node != null)
        {
            path.Add(node.Position);
            node = cameFrom[node];
        }
        path.Reverse();

        return path;
    }

    private class Node
    {
        public Vector2 Position { get; private set; }
        public HashSet<Node> Neighbors { get; private set; } = new();

        public Node(Vector2 position)
        {
            Position = position;
        }
    }
}
