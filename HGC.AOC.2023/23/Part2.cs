using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2023._23;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var map = this.ReadInputLines("input.txt").Select(
            line =>
                line.Select(c =>
                    c switch
                    {
                        '.' => true,
                        '#' => false,
                        '>' => true,
                        'v' => true,
                        '<' => true,
                        '^' => true,
                        _ => throw new Exception("Unrecognised terrain type")
                    }).ToArray()).ToArray();

        var start = new Point(Array.IndexOf(map[0], true), 0);
        var end = new Point(Array.IndexOf(map[^1], true), map.Length - 1);

        var longestPath = 0;
        
        var searchQueue = new ConcurrentStack<(Point from, ImmutableHashSet<Point> history)>();
        searchQueue.Push((start with { Y = start.Y + 1 }, ImmutableHashSet.Create(start)));

        void Action(int _)
        {
            var started = false;
            while (searchQueue.TryPop(out var current) || !started)
            {
                started = true;
                var (from, history) = current;

                if (from.X == end.X && from.Y == end.Y)
                {
                    var pathLength = Math.Max(longestPath, history.Count);
                    while (longestPath < pathLength)
                    {
                        Interlocked.Increment(ref longestPath);
                        Console.WriteLine(longestPath);
                        Console.WriteLine(searchQueue.Count);
                    }
                }
                else
                {
                    var newHistory = history.Add(from);
                    foreach (var next in GetNeighbours(from)
                                 .Where(n => map[n.Y][n.X])
                                 .Where(n => !history.Contains(n)))
                    {
                        searchQueue.Push((next, newHistory));
                    }
                }
            }
        }

        Enumerable.Range(1, 16).AsParallel().ForAll(Action);
        
        return longestPath;
    }
    
    IEnumerable<Point> GetNeighbours(Point point)
    {
        yield return point with { X = point.X + 1 };
        yield return point with { Y = point.Y + 1 };
        yield return point with { X = point.X - 1 };
        yield return point with { Y = point.Y - 1 };
    }
}