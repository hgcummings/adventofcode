using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2024._16;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var map = this.ReadInputLines("input.txt").ToList();

        var startY = map.FindIndex(row => row.Contains('S'));
        var startX = map[startY].IndexOf('S');

        var endY = map.FindIndex(row => row.Contains('E'));
        var endX = map[endY].IndexOf('E');

        var start = new Node(startX, startY, Dir.East);
        
        var distances = new Dictionary<Node, int> { [start] = 0 };
        var prev = new Dictionary<Node, List<Node>>();

        var queue = new PriorityQueue<Node, int>();
        queue.Enqueue(start, 0);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            
            if (node.X == endX && node.Y == endY)
            {
                break;
            }

            foreach (var (neighbour, cost) in node.Neighbours(map))
            {
                var distanceViaNode = distances[node] + cost;
                if (distanceViaNode <= distances.GetValueOrDefault(neighbour, Int32.MaxValue))
                {
                    if (distanceViaNode < distances.GetValueOrDefault(neighbour, Int32.MaxValue))
                    {
                        prev[neighbour] = new List<Node>();
                        queue.Enqueue(neighbour, distanceViaNode);
                    }
                    distances[neighbour] = distanceViaNode;
                    prev[neighbour].Add(node);
                }
            }
        }

        var tiles = new HashSet<Point>();
        var prevQueue = new Queue<Node>();

        foreach (var endNode in prev.Keys.Where(n => n.X == endX && n.Y == endY))
        {
            prevQueue.Enqueue(endNode);
        }

        while (prevQueue.Count > 0)
        {
            var node = prevQueue.Dequeue();
            tiles.Add(new Point(node.X, node.Y));

            if (prev.ContainsKey(node))
            {
                foreach (var prevNode in prev[node])
                {
                    prevQueue.Enqueue(prevNode);
                }
            }
        }

        return tiles.Count;
    }
    
    struct Node
    {
        public bool Equals(Node other)
        {
            return X == other.X && Y == other.Y && D == other.D;
        }

        public override bool Equals(object? obj)
        {
            return obj is Node other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, (int)D);
        }

        public int X { get; }
        public int Y { get; }
        public Dir D { get; }

        public Node(int x, int y, Dir d)
        {
            X = x;
            Y = y;
            D = d;
        }

         public IEnumerable<(Node neighbour, int cost)> Neighbours(List<string> map)
         {
             if (D == Dir.North)
             {
                 yield return (new Node(X, Y, Dir.West), 1000);
                 yield return (new Node(X, Y, Dir.East), 1000);
                 if (Y > 1 && map[Y - 1][X] != '#')
                 {
                     yield return (new Node(X, Y - 1, D), 1);
                 }
             }
             
             if (D == Dir.East)
             {
                 yield return (new Node(X, Y, Dir.North), 1000);
                 yield return (new Node(X, Y, Dir.South), 1000);
                 if (X < map[Y].Length - 1 && map[Y][X + 1] != '#')
                 {
                     yield return (new Node(X + 1, Y, D), 1);
                 }
             }
             
             if (D == Dir.South) {
                 yield return (new Node(X, Y, Dir.West), 1000);
                 yield return (new Node(X, Y, Dir.East), 1000);
                 if (Y < map.Count - 1 && map[Y + 1][X] != '#')
                 {
                     yield return (new Node(X, Y + 1, D), 1);
                 }
             }
             
             if (D == Dir.West)
             {
                 yield return (new Node(X, Y, Dir.North), 1000);
                 yield return (new Node(X, Y, Dir.South), 1000);
                 if (X > 1 && map[Y][X - 1] != '#')
                 {
                     yield return (new Node(X - 1, Y, D), 1);
                 }
             }
         }
    }
    
    enum Dir
    {
        East,
        South,
        West,
        North
    }
}