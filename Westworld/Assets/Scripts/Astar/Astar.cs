using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Modified Version for Unity:
 * https://github.com/entdark/ColorLinesNG2/tree/master/ColorLinesNG2/ColorLinesNG2/AStar
 * https://github.com/entdark/ColorLinesNG2/blob/master/LICENSE 
 * */
public struct Point
{ 
    public int x;
    public int y;

    public Point(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
    public Point(float _x, float _y)
    {
        x = (int)_x;
        y = (int)_y;
    }
    public static bool IsEqual(Point a,Point b)
    {
        if (a.x == b.x && a.y == b.y)
            return true;

        return false; 
    }
}

public class SearchParameters
{
    public Point StartLocation { get; set; }

    public Point EndLocation { get; set; }

    public bool[,] Map { get; set; }

    public SearchParameters(Point startLocation, Point endLocation, bool[,] map)
    {
        this.StartLocation = startLocation;
        this.EndLocation = endLocation;
        this.Map = map;
    }
}

public class PathFinder
{
    private int width;
    private int height;
    private Node[,] nodes;
    private Node startNode;
    private Node endNode;
    private SearchParameters searchParameters;

    /// <summary>
    /// Create a new instance of PathFinder
    /// </summary>
    /// <param name="searchParameters"></param>
    public PathFinder(SearchParameters searchParameters)
    {
        this.searchParameters = searchParameters;
        InitializeNodes(searchParameters.Map);
        this.startNode = this.nodes[searchParameters.StartLocation.x, searchParameters.StartLocation.y];
        this.startNode.State = NodeState.Open;
        this.endNode = this.nodes[searchParameters.EndLocation.x, searchParameters.EndLocation.y];
    }

    /// <summary>
    /// Attempts to find a path from the start location to the end location based on the supplied SearchParameters
    /// </summary>
    /// <returns>A List of Points representing the path. If no path was found, the returned list is empty.</returns>
    public List<Point> FindPath()
    {
        // The start node is the first entry in the 'open' list
        List<Point> path = new List<Point>();
        bool success = Search(startNode);
        if (success)
        {
            // If a path was found, follow the parents from the end node to build a list of locations
            Node node = this.endNode;
            while (node.ParentNode != null)
            {
                path.Add(node.Location);
                node = node.ParentNode;
            }

            // Reverse the list so it's in the correct order when returned
            path.Reverse();
        }

        return path;
    }
    public bool HasPath()
    {
        // The start node is the first entry in the 'open' list
        return Search(startNode);
    }

    /// <summary>
    /// Builds the node grid from a simple grid of booleans indicating areas which are and aren't walkable
    /// </summary>
    /// <param name="map">A boolean representation of a grid in which true = walkable and false = not walkable</param>
    private void InitializeNodes(bool[,] map)
    {
        this.width = map.GetLength(0);
        this.height = map.GetLength(1);
        this.nodes = new Node[this.width, this.height];
        for (int y = 0; y < this.height; y++)
        {
            for (int x = 0; x < this.width; x++)
            {
                this.nodes[x, y] = new Node(x, y, map[x, y], this.searchParameters.EndLocation);
            }
        }
    }

    /// <summary>
    /// Attempts to find a path to the destination node using <paramref name="currentNode"/> as the starting location
    /// </summary>
    /// <param name="currentNode">The node from which to find a path</param>
    /// <returns>True if a path to the destination has been found, otherwise false</returns>
    private bool Search(Node currentNode)
    {
        // Set the current node to Closed since it cannot be traversed more than once
        currentNode.State = NodeState.Closed;
        List<Node> nextNodes = GetAdjacentWalkableNodes(currentNode);

        // Sort by F-value so that the shortest possible routes are considered first
        nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
        foreach (var nextNode in nextNodes)
        {
            // Check whether the end node has been reached
            if (Point.IsEqual(nextNode.Location, this.endNode.Location))
            {
                return true;
            }
            else
            {
                // If not, check the next set of nodes
                if (Search(nextNode)) // Note: Recurses back into Search(Node)
                    return true;
            }
        }

        // The method returns false if this path leads to be a dead end
        return false;
    }

    /// <summary>
    /// Returns any nodes that are adjacent to <paramref name="fromNode"/> and may be considered to form the next step in the path
    /// </summary>
    /// <param name="fromNode">The node from which to return the next possible nodes in the path</param>
    /// <returns>A list of next possible nodes in the path</returns>
    private List<Node> GetAdjacentWalkableNodes(Node fromNode)
    {
        List<Node> walkableNodes = new List<Node>();
        IEnumerable<Point> nextLocations = GetAdjacentLocations(fromNode.Location);

        foreach (var location in nextLocations)
        {
            int x = location.x;
            int y = location.y;

            // Stay within the grid's boundaries
            if (x < 0 || x >= this.width || y < 0 || y >= this.height)
                continue;

            Node node = this.nodes[x, y];
            // Ignore non-walkable nodes
            if (!node.IsWalkable)
                continue;

            // Ignore already-closed nodes
            if (node.State == NodeState.Closed)
                continue;

            // Already-open nodes are only added to the list if their G-value is lower going via this route.
            if (node.State == NodeState.Open)
            {
                float traversalCost = Node.GetTraversalCost(node.Location, node.ParentNode.Location);
                float gTemp = fromNode.G + traversalCost;
                if (gTemp < node.G)
                {
                    node.ParentNode = fromNode;
                    walkableNodes.Add(node);
                }
            }
            else
            {
                // If it's untested, set the parent and flag it as 'Open' for consideration
                node.ParentNode = fromNode;
                node.State = NodeState.Open;
                walkableNodes.Add(node);
            }
        }

        return walkableNodes;
    }

    /// <summary>
    /// Returns the eight locations immediately adjacent (orthogonally and diagonally) to <paramref name="fromLocation"/>
    /// </summary>
    /// <param name="fromLocation">The location from which to return all adjacent points</param>
    /// <returns>The locations as an IEnumerable of Points</returns>
    private static IEnumerable<Point> GetAdjacentLocations(Point fromLocation)
    {
        return new Point[]
        {
                new Point(fromLocation.x-1, fromLocation.y-1),
                new Point(fromLocation.x-1, fromLocation.y  ),
                new Point(fromLocation.x-1, fromLocation.y+1),
                new Point(fromLocation.x,   fromLocation.y+1),
                new Point(fromLocation.x+1, fromLocation.y+1),
                new Point(fromLocation.x+1, fromLocation.y  ),
                new Point(fromLocation.x+1, fromLocation.y-1),
                new Point(fromLocation.x,   fromLocation.y-1)
        };
    }
}

public class Node
{
    private Node parentNode;

    /// <summary>
    /// The node's location in the grid
    /// </summary>
    public Point Location { get; private set; }

    /// <summary>
    /// True when the node may be traversed, otherwise false
    /// </summary>
    public bool IsWalkable { get; set; }

    /// <summary>
    /// Cost from start to here
    /// </summary>
    public float G { get; private set; }

    /// <summary>
    /// Estimated cost from here to end
    /// </summary>
    public float H { get; private set; }

    /// <summary>
    /// Flags whether the node is open, closed or untested by the PathFinder
    /// </summary>
    public NodeState State { get; set; }

    /// <summary>
    /// Estimated total cost (F = G + H)
    /// </summary>
    public float F
    {
        get { return this.G + this.H; }
    }

    /// <summary>
    /// Gets or sets the parent node. The start node's parent is always null.
    /// </summary>
    public Node ParentNode
    {
        get { return this.parentNode; }
        set
        {
            // When setting the parent, also calculate the traversal cost from the start node to here (the 'G' value)
            this.parentNode = value;
            this.G = this.parentNode.G + GetTraversalCost(this.Location, this.parentNode.Location);
        }
    }

    /// <summary>
    /// Creates a new instance of Node.
    /// </summary>
    /// <param name="x">The node's location along the X axis</param>
    /// <param name="y">The node's location along the Y axis</param>
    /// <param name="isWalkable">True if the node can be traversed, false if the node is a wall</param>
    /// <param name="endLocation">The location of the destination node</param>
    public Node(int x, int y, bool isWalkable, Point endLocation)
    {
        this.Location = new Point(x, y);
        this.State = NodeState.Untested;
        this.IsWalkable = isWalkable;
        this.H = GetTraversalCost(this.Location, endLocation);
        this.G = 0;
    }

    public override string ToString()
    {
        return string.Format("{0}, {1}: {2}", this.Location.x, this.Location.y, this.State);
    }

    /// <summary>
    /// Gets the distance between two points
    /// </summary>
    internal static float GetTraversalCost(Point location, Point otherLocation)
    {
        float deltaX = otherLocation.x - location.x;
        float deltaY = otherLocation.y - location.y;
        return (float)System.Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }
}

public enum NodeState
{
    /// <summary>
    /// The node has not yet been considered in any possible paths
    /// </summary>
    Untested,
    /// <summary>
    /// The node has been identified as a possible step in a path
    /// </summary>
    Open,
    /// <summary>
    /// The node has already been included in a path and will not be considered again
    /// </summary>
    Closed
}
