using System.Collections;
using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2023._21;

public class Part1 : ISolution
{
    public object? Answer()
    {
        Point? foundStart = null;
        var map = this.ReadInputLines("input.txt").Select(
            (line, y) =>
            {
                if (line.IndexOf('S') != -1)
                {
                    foundStart = new Point(line.IndexOf('S'), y);
                }

                return line.Select(c => c == '#').ToArray();
            }).ToArray();

        var frontier = new HashSet<Point> { foundStart!.Value };

        IEnumerable<Point> GetNeighbours(Point point)
        {
            if (point.X > 0)
            {
                yield return point with { X = point.X - 1 };
            }

            if (point.Y > 0)
            {
                yield return point with { Y = point.Y - 1 };
            }

            if (point.X < map[0].Length - 1)
            {
                yield return point with { X = point.X + 1 };
            }

            if (point.Y < map.Length - 1)
            {
                yield return point with { Y = point.Y + 1 };
            }
        }
        
        for (var i = 0; i < 64; ++i)
        {
            var newFrontier = new HashSet<Point>();

            foreach (var point in frontier)
            {
                foreach (var n in GetNeighbours(point))
                {
                    if (!map[n.Y][n.X])
                    {
                        newFrontier.Add(n);
                    }
                }
            }

            frontier = newFrontier;
            Console.WriteLine($"{i+1}: {frontier.Count}");
        }
        
        return frontier.Count;
    }
}