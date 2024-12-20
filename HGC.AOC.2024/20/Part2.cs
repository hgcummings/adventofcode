using System.Diagnostics;
using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2024._20;

public class Part2 : ISolution
{
    private const int Threshold = 100;
    private const int MaxTunnel = 20;
    
    private List<string> map;
    private int startX;
    private int startY;
    private int endX;
    private int endY;
    
    public object? Answer()
    {
        map = this.ReadInputLines("input.txt").ToList();

        startY = map.FindIndex(row => row.Contains('S'));
        startX = map[startY].IndexOf('S');
        
        endY = map.FindIndex(row => row.Contains('E'));
        endX = map[endY].IndexOf('E');

        var minHonestDistance = MinHonestDistance(startX, startY);

        var start = new State(startX, startY);
        var distances = new Dictionary<State, int> { [start] = 0 };

        var queue = new PriorityQueue<State, int>();
        queue.Enqueue(start, 0);

        var i = 0;
        while (queue.Count > 0)
        {
            if (++i % 10000 == 0)
            {
                Console.WriteLine(queue.Count);
            }

            var node = queue.Dequeue();

            if (node.C)
            {
                var end = node with { X = endX, Y = endY };
                var distanceToEndViaNode = distances[node] + MinHonestDistance(node.X, node.Y);
                if (distanceToEndViaNode <= minHonestDistance - Threshold &&
                    distanceToEndViaNode < distances.GetValueOrDefault(end, Int32.MaxValue))
                {
                    distances[end] = distanceToEndViaNode;
                }
                continue;
            }
            
            foreach (var (neighbour, cost) in node.Neighbours(map))
            {
                var distanceViaNode = distances[node] + cost;
                if (distanceViaNode <= minHonestDistance - Threshold &&
                    distanceViaNode < distances.GetValueOrDefault(neighbour, Int32.MaxValue))
                {
                    queue.Enqueue(neighbour, distanceViaNode);
                    distances[neighbour] = distanceViaNode;
                }
            }
        }
        
        var shortcuts = new Dictionary<Cheat, int>();
        foreach (var e in distances.Where(n => n.Key.X == endX && n.Key.Y == endY))
        {
            var cheat = new Cheat(e.Key.CStart, e.Key.CEnd);
            var saving = minHonestDistance - e.Value;
            shortcuts.Add(cheat, saving);
        }

        foreach (var group in shortcuts.GroupBy(e => e.Value).OrderBy(g => g.Key))
        {
            Console.WriteLine($"There are {group.Count()} cheats that save {group.Key} picoseconds");
        }
        
        return shortcuts.Keys.Count;
    }

    private Dictionary<Point, int> minHonestDistances = new();

    int MinHonestDistance(int fromX, int fromY)
    {
        var key = new Point(fromX, fromY);

        if (minHonestDistances.ContainsKey(key))
        {
            return minHonestDistances[key];
        }

        var result = MinHonestDistanceRaw(fromX, fromY);
        minHonestDistances[key] = result;
        return result;
    }
    
    int MinHonestDistanceRaw(int fromX, int fromY)
    {
        if (fromX == endX && fromY == endY)
        {
            return 0;
        }
        
        var start = new Point(fromX, fromY);
        
        var distances = new Dictionary<Point, int>();
        distances[start] = 0;

        var queue = new PriorityQueue<Point, int>();
        queue.Enqueue(start, 0);
        
        IEnumerable<Point> Neighbours(Point loc)
        {
            if (loc.X > 0) yield return loc with { X = loc.X - 1 };
            if (loc.X < map[0].Length - 1) yield return loc with { X = loc.X + 1 };
            if (loc.Y > 0) yield return loc with { Y = loc.Y - 1 };
            if (loc.Y < map.Count - 1) yield return loc with { Y = loc.Y + 1 };
        }
        
        while (queue.Count > 0)
        {
            var loc = queue.Dequeue();
            
            if (loc.X == endX && loc.Y == endY)
            {
                return distances[loc];
            }

            foreach (var neighbour in Neighbours(loc).Where(n => !IsWall(map, n.X, n.Y)))
            {
                var distanceViaLoc = distances[loc] + 1;
                if (distanceViaLoc < distances.GetValueOrDefault(neighbour, Int32.MaxValue))
                {
                    distances[neighbour] = distanceViaLoc;
                    queue.Enqueue(neighbour, distanceViaLoc);
                }
            }
        }

        return 1000000;
    }

    static bool IsWall(List<string> map, int x, int y)
    {
        return map[y][x] == '#';
    }

    struct Cheat(Point start, Point end)
    {
        public bool Equals(Cheat other)
        {
            return Start.Equals(other.Start) && End.Equals(other.End);
        }

        public override bool Equals(object? obj)
        {
            return obj is Cheat other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, End);
        }

        public Point Start { get; } = start;
        public Point End { get; } = end;
    }

    struct State(int x, int y)
    {
        public bool Equals(State other)
        {
            return X == other.X && Y == other.Y && C == other.C && CStart.Equals(other.CStart) &&
                   CEnd.Equals(other.CEnd);
        }

        public override bool Equals(object? obj)
        {
            return obj is State other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, C, CStart, CEnd);
        }

        public int X { get; init; } = x;
        public int Y { get; init; } = y;
        public bool C { get; init; } = false;
        public Point CStart { get; private init; }
        public Point CEnd { get; private init; }

        public IEnumerable<(State state, int cost)> Neighbours(List<string> map)
        {
            return Reachable(map).Where(p => !IsWall(map, p.state.X, p.state.Y));
        }

        private IEnumerable<(State state, int cost)> Reachable(List<string> map)
        {
            if (X > 0) yield return (this with { X = X - 1 }, 1);
            if (X < map[0].Length - 2) yield return (this with { X = X + 1 }, 1);
            if (Y > 0) yield return (this with { Y = Y - 1 }, 1);
            if (Y < map.Count - 2) yield return (this with { Y = Y + 1 }, 1);

            if (C) yield break;
            
            for (var dx = -MaxTunnel; dx <= MaxTunnel; ++dx)
            {
                for (var dy = -(MaxTunnel - Math.Abs(dx)); dy <= (MaxTunnel - Math.Abs(dx)); ++dy)
                {
                    if (dx == 0 && dy == 0)
                    {
                        continue;
                    }

                    if (X + dx < 0 || X + dx >= map[Y].Length - 1 ||
                        Y + dy < 0 || Y + dy >= map.Count - 1)
                    {
                        continue;
                    }

                    yield return (
                        this with
                        {
                            X = X + dx,
                            Y = Y + dy,
                            C = true,
                            CStart = new Point(X, Y),
                            CEnd = new Point(X + dx, Y + dy)
                        },
                        Math.Abs(dx) + Math.Abs(dy));
                }
            }
        }
    }
}