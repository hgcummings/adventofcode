using System.Collections.Immutable;
using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2023._23;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var map = this.ReadInputLines("input.txt").Select(
            line =>
                line.Select(c =>
                    c switch
                    {
                        '.' => Direction.All,
                        '#' => Direction.None,
                        '>' => Direction.Right,
                        'v' => Direction.Down,
                        '<' => Direction.Left,
                        '^' => Direction.Up,
                        _ => throw new Exception("Unrecognised terrain type")
                    }).ToArray()).ToArray();

        var start = new Point(Array.IndexOf(map[0], Direction.All), 0);
        var end = new Point(Array.IndexOf(map[^1], Direction.All), map.Length - 1);

        var longestPath = 0;
        
        var searchStack = new Stack<(Point from, ImmutableHashSet<Point> history)>();
        searchStack.Push((start with { Y = start.Y + 1 }, ImmutableHashSet.Create(start)));

        while (searchStack.Count != 0)
        {
            var (from, history) = searchStack.Pop();
            if (from.X == end.X && from.Y == end.Y)
            {
                longestPath = Math.Max(longestPath, history.Count);
            }
            else
            {
                var newHistory = history.Add(from);
                foreach (var next in GetNeighbours(from)
                             .Where(n => (map[n.p.Y][n.p.X] & n.d) != 0)
                             .Where(n => !history.Contains(n.p)))
                {
                    searchStack.Push((next.p, newHistory));
                }
            }
        }

        return longestPath;
    }
    
    IEnumerable<(Point p, Direction d)> GetNeighbours(Point point)
    {
        yield return (point with { X = point.X + 1 }, Direction.Right);
        yield return (point with { Y = point.Y + 1 }, Direction.Down);
        yield return (point with { X = point.X - 1 }, Direction.Left);
        yield return (point with { Y = point.Y - 1 }, Direction.Up);
    }
    
    Direction Reverse(Direction dir)
    {
        return dir switch
        {
            Direction.Right => Direction.Left,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Up => Direction.Down,
            _ => throw new InvalidOperationException("Can only travel in cardinal directions")
        };
    }

    [Flags]
    public enum Direction
    {
        None = 0,
        Right = 1,
        Down = 2,
        Left = 4,
        Up = 8,
        All = 15
    }
}