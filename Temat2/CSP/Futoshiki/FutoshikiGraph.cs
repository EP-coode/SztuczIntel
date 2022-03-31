using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Temat2_CSP.Futoshiki;

public enum Relation
{
    GREATER_THAN = '>',
    LESS_THAN = '<',
}

public class Node
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Value { get; set; }

    public Node()
    {

    }

    public Node(Node n)
    {
        X = n.X;
        Y = n.Y;
        Value = n.Value;
    }

    public override int GetHashCode()
    {
        int hash = 23;
        hash = hash * 31 + X;
        hash = hash * 31 + Y;
        return hash;
    }
}


public class FutoshikiGraph
{
    private Dictionary<Node, HashSet<(Node, Relation)>> adjencyList;
    private int[] domain;

    public FutoshikiGraph(int[] domain)
    {
        adjencyList = new Dictionary<Node, HashSet<(Node, Relation)>>();
        this.domain = domain;
    }

    public FutoshikiGraph(FutoshikiGraph graph)
    {
        Dictionary<Node, Node> nodesMap = new();
        adjencyList = new Dictionary<Node, HashSet<(Node, Relation)>>();
        domain = graph.domain;

        // coppy nodes
        foreach (var v in graph.adjencyList)
        {
            Node newNode = new Node(v.Key);
            nodesMap.Add(v.Key, newNode);
            AddNode(newNode);
        }

        // coppy edges
        foreach (var v in graph.adjencyList)
        {
            Node src = nodesMap.GetValueOrDefault(v.Key);
            HashSet<(Node, Relation)> relations;
            adjencyList.TryGetValue(src, out relations);

            foreach (var edge in v.Value)
            {
                Node dst = nodesMap.GetValueOrDefault(edge.Item1);
                relations.Add((dst, edge.Item2));

                if (dst.X == 0 && dst.Y == 6)
                {
                    Console.WriteLine("AAA");
                }
            }
        }

    }

    public void AddNode(Node n)
    {
        if (adjencyList.ContainsKey(n))
        {
            throw new InvalidOperationException();
        }
        adjencyList.Add(n, new HashSet<(Node, Relation)>());
    }

    public void AddEdge(Node n1, Node n2, Relation r)
    {
        if (!adjencyList.ContainsKey(n1) || !adjencyList.ContainsKey(n2))
        {
            throw new InvalidOperationException();
        }

        HashSet<(Node, Relation)> n1Relations;
        HashSet<(Node, Relation)> n2Relations;
        adjencyList.TryGetValue(n1, out n1Relations);
        adjencyList.TryGetValue(n2, out n2Relations);

        if (r == Relation.LESS_THAN)
        {
            n1Relations.Add((n2, Relation.LESS_THAN));
            n2Relations.Add((n1, Relation.GREATER_THAN));
        }
        else
        {
            n1Relations.Add((n2, Relation.GREATER_THAN));
            n2Relations.Add((n1, Relation.LESS_THAN));
        }
    }

    private bool AreConstraintsValid(Node n)
    {
        HashSet<(Node, Relation)> relations;
        adjencyList.TryGetValue(n, out relations);


        int maxValue = domain[domain.Length - 1];
        int minValue = domain[0];

        foreach (var edge in relations)
        {

            if (n.Value == -1 || edge.Item1.Value == -1)
                continue;

            if (n.Value == minValue && edge.Item2 == Relation.GREATER_THAN)
                return false;

            if (n.Value == maxValue && edge.Item2 == Relation.LESS_THAN)
                return false;

            switch (edge.Item2)
            {
                case Relation.LESS_THAN:
                    if (n.Value >= edge.Item1.Value)
                    {
                        return false;
                    }
                    break;
                case Relation.GREATER_THAN:
                    if (n.Value <= edge.Item1.Value)
                    {
                        return false;
                    }
                    break;
            }
        }
        return true;
    }

    private bool IsValid(Node n)
    {
        return HasNoRepeatsINRowsAndCollumns(n) && AreConstraintsValid(n);
    }

    private bool HasNoRepeatsINRowsAndCollumns(Node n)
    {
        return !adjencyList
            .Where(
            e => (e.Key.X == n.X || e.Key.Y == n.Y)
            && e.Key.Value == n.Value
            && e.Key.Value != -1
            && n.Value != -1
            && e.Key != n
            ).Any();
    }

    private Node? FindNextEmpty()
    {
        foreach (var node in adjencyList.Keys)
        {
            if (node.Value == -1)
            {
                return node;
            }
        }

        return null;
    }

    public ICollection<char[][]> Solve()
    {
        ICollection<char[][]> sollutions = new List<char[][]>();

        foreach (var value in domain)
        {
            FutoshikiGraph child = new(this);
            Node emptyNode = child.FindNextEmpty();

            if (emptyNode == null)
            {
                sollutions.Add(ConvetToArray());
                return sollutions;
            }

            emptyNode.Value = value;

            if (child.IsValid(emptyNode))
            {
                sollutions = sollutions.Concat(child.Solve()).ToList();
            }
        }

        return sollutions;
    }

    public char[][] ConvetToArray()
    {
        int size = domain.Length + domain.Length - 1;
        char[][] result = new char[size][];

        for (int i = 0; i < size; i++)
        {
            result[i] = new char[size];
            Array.Fill(result[i], '-');
        }

        foreach (var node in adjencyList)
        {
            result[node.Key.Y][node.Key.X] = node.Key.Value.ToString()[0];

            foreach (var edge in node.Value)
            {
                int x = (edge.Item1.X + node.Key.X) / 2;
                int y = (edge.Item1.Y + node.Key.Y) / 2;
                if (node.Key.X < edge.Item1.X || node.Key.Y > edge.Item1.Y)
                {
                    if (edge.Item2 == Relation.LESS_THAN)
                        result[y][x] = '<';
                    else
                        result[y][x] = '>';
                }
            }

        }

        return result;
    }

    public Node? GetNodeByPosition(int x, int y)
    {
        return adjencyList.AsEnumerable()
            .First(n => n.Key.X == x && n.Key.Y == y)
            .Key;
    }
}

