using System.Collections;
using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2023._21;

public class Part2 : ISolution
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
        
        IEnumerable<Point> GetNeighbours(Point point)
        {
            yield return point with { X = point.X - 1 };
            yield return point with { Y = point.Y - 1 };
            yield return point with { X = point.X + 1 };
            yield return point with { Y = point.Y + 1 };
        }

        var oddCount = 0L;
        var evenCount = 1L;
        
        var frontier = new HashSet<Point> { foundStart!.Value };
        var counts = new List<int>();
        counts.Add(1);
        
        for (var i = 0; i < 65+131+131; ++i)
        {
            var newFrontier = new HashSet<Point>();

            foreach (var point in frontier)
            {
                foreach (var n in GetNeighbours(point))
                {
                    if (!map[((n.Y % map.Length) + map.Length) % map.Length][(n.X % map[0].Length
                            + map[0].Length) % map[0].Length])
                    {
                        newFrontier.Add(n);
                    }
                }
            }

            frontier = newFrontier;            
            counts.Add(frontier.Count);
        }

        var diff1 = counts[65 + 131] - counts[65];
        var diff2 = counts[65 + 131 + 131] - counts[65 + 131];

        var a = (diff2 - diff1) / 2;
        var b = diff1 - a;

        var x = (26501365L - 65L) / 131L;

        return a * x * x + b * x + counts[65];
    }
}