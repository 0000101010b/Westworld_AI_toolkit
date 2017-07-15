using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

/*
public class Tuple<T1, T2>
{
    public T1 Item1 { get; set; }
    public T2 Item2 { get; set; }

    public static Tuple<T1, T2> CreateTuple(T1 item1, T2 item2)
    {
        return new Tuple<T1, T2>
        {
            Item1 = item1,
            Item2 = item2
        };
    }
}

public interface WeightedGraph<Node>
{
    double Cost(Node a, Node b);
    IEnumerable<Node> Neighbors(Node id);
}

public class Node {

    public int x, y;

    public Node(int x,int y)
    {
        this.x = x;
        this.y = y;
    }
}
class NodeComparer : IEqualityComparer<Node>
{
    #region IEqualityComparer<Node> Members

    public bool Equals(Node x, Node y)
    {
        return (x.x == y.x && y.y == x.y);
    }

    public int GetHashCode(Node obj)
    {
        return obj.GetHashCode();
    }
    #endregion
}

public class SquareGrid : WeightedGraph<Node>
{
    public static readonly Node[] DIRS = new[]
    {
            new Node(1, 0),
            new Node(0, -1),
            new Node(-1, 0),
            new Node(0, 1)

    };

    public int width,height;
    public List<Node> walls = new List<Node>();
    public List<Node> walkable = new List<Node>();


    public SquareGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public bool InBounds(Node id)
    {

        return -width/2 <= id.x && id.x <= width/2
            && -height/2 <= id.y && id.y <= height/2;
    }

    public bool Passable(Node id)
    {
        return !walls.Contains(id, new NodeComparer());
    }

    public double Cost(Node a, Node b)
    {
        return walkable.Contains(b, new NodeComparer()) ? 5 : 1;
    }

    public IEnumerable<Node> Neighbors(Node id)
    {
        foreach (var dir in DIRS)
        {
            Node next = new Node(id.x + dir.x, id.y + dir.y);
            if (InBounds(next) && Passable(next))
            {
                yield return next;
            }
        }
    }
}

public class PriorityQueue<T>
{
    private List<Tuple<T, double>> elements = new List<Tuple<T, double>>();

    public int Count
    {
        get { return elements.Count; }
    }

    public void Enqueue(T item, double priority)
    {
        elements.Add(Tuple<T, double>.CreateTuple(item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Item2 < elements[bestIndex].Item2)
            {
                bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].Item1;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }
}

public class AStarSearch
{
    public Dictionary<Node, Node> cameFrom= new Dictionary<Node, Node>();
    public Dictionary<Node, double> costSoFar= new Dictionary<Node, double>();

    // Note: a generic version of A* would abstract over Node and
    // also Heuristic
    static public double Heuristic(Node a, Node b)
    {
        return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
    }

    public AStarSearch(WeightedGraph<Node> graph, Node start, Node goal)
    {
        var frontier = new PriorityQueue<Node>();
        frontier.Enqueue(start, 0);

        cameFrom[start] = start;
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current.Equals(goal))
            {
                break;
            }

            foreach (var next in graph.Neighbors(current))
            {
                double newCost = costSoFar[current] + graph.Cost(current, next);

                if (!costSoFar.ContainsKey(next)||
                    newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    double priority = newCost + Heuristic(next, goal);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;

                }
            }
        }
    }
}*/