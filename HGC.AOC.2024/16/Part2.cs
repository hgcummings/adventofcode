using System.Collections.Immutable;
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

        var distances = new Dictionary<Node, int>();
        distances[start] = 0;

        var queue = new PriorityQueue<Node, int>();
        queue.Enqueue(start, 0);

        var shortestPath = 0;

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();

            if (node.X == endX && node.Y == endY)
            {
                shortestPath = distances[node];
                break;
            }

            foreach (var (neighbour, cost) in node.Neighbours(map))
            {
                var distanceViaNode = distances[node] + cost;
                if (distanceViaNode < distances.GetValueOrDefault(neighbour, Int32.MaxValue))
                {
                    distances[neighbour] = distanceViaNode;
                    queue.Enqueue(neighbour, distanceViaNode);
                }
            }
        }

        var paths = start.PathsToEnd(
                shortestPath,
                map,
                endX,
                endY);
        var tiles = new HashSet<Point>();

        foreach (var path in paths)
        {
            foreach (var node in path)
            {
                tiles.Add(new Point(node.X, node.Y));
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

        private static Dictionary<CacheKey, List<IList<Node>>> CachedPaths = new();
        private static int Hits = 0;
        private static int Misses = 0;
        
        public IEnumerable<IList<Node>> PathsToEnd(
            int maxCost,
            List<string> map,
            int endX,
            int endY)
        {
            var cacheKey = new CacheKey(this, maxCost);
            if (CachedPaths.TryGetValue(cacheKey, out var cached))
            {
                ++Hits;
                if (Hits % 100000 == 0)
                {
                    Console.WriteLine(
                        $"Hits: {Hits}, Misses: {Misses}, " +
                        $"MinCost: {CachedPaths.Keys.Min(k => k.Cost)}, " +
                        $"MaxCost: {CachedPaths.Keys.Max(k => k.Cost)}");
                }
                return cached;
            }

            ++Misses;
            var paths = PathsToEndRaw(maxCost, map, endX, endY).ToList();
            CachedPaths.Add(cacheKey, paths);
            return paths;
        }
        
        public IEnumerable<IList<Node>> PathsToEndRaw(
            int maxCost,
            List<string> map,
            int endX,
            int endY)
        {
            // Console.WriteLine(maxCost);
            if (X == endX && Y == endY)
            {
                yield return [this];
            }
            else
            {
                foreach (var (n, cost) in Neighbours(map))
                {
                    if (cost <= maxCost)
                    {
                        foreach (var path in
                                 n.PathsToEnd(
                                     maxCost - cost,
                                     map, endX, endY))
                        {
                            yield return [this, ..path];
                        }
                    }
                }
            }
        }

        public IEnumerable<(Node neighbour, int cost)> Neighbours(List<string> map)
        {
            if (D == Dir.North)
            {
                if (Y > 1 && map[Y - 1][X] != '#')
                {
                    yield return (new Node(X, Y - 1, D), 1);
                }
            }

            if (D == Dir.East)
            {
                if (X < map[Y].Length - 1 && map[Y][X + 1] != '#')
                {
                    yield return (new Node(X + 1, Y, D), 1);
                }
            }

            if (D == Dir.South)
            {
                if (Y < map.Count - 1 && map[Y + 1][X] != '#')
                {
                    yield return (new Node(X, Y + 1, D), 1);
                }
            }

            if (D == Dir.West)
            {
                if (X > 1 && map[Y][X - 1] != '#')
                {
                    yield return (new Node(X - 1, Y, D), 1);
                }
            }

            if (D == Dir.South)
            {
                yield return (new Node(X, Y, Dir.East), 1000);
                yield return (new Node(X, Y, Dir.West), 1000);
            }

            if (D == Dir.West)
            {
                yield return (new Node(X, Y, Dir.North), 1000);
                yield return (new Node(X, Y, Dir.South), 1000);
            }
            
            if (D == Dir.North)
            {
                yield return (new Node(X, Y, Dir.West), 1000);
                yield return (new Node(X, Y, Dir.East), 1000);
            }

            if (D == Dir.East)
            {
                yield return (new Node(X, Y, Dir.North), 1000);
                yield return (new Node(X, Y, Dir.South), 1000);
            }
        }

        struct CacheKey(Node node, int cost)
        {
            public bool Equals(CacheKey other)
            {
                return Node.Equals(other.Node) && Cost == other.Cost;
            }

            public override bool Equals(object? obj)
            {
                return obj is CacheKey other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Node, Cost);
            }

            public Node Node { get; } = node;
            public int Cost { get; } = cost;
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