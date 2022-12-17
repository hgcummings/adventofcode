using System.Drawing;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2022._12;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var height = new Dictionary<Point, int>();

        List<Point> starts = new List<Point>();
        Point dest = default;

        var x = 0;
        var y = 0;
        foreach (var line in input)
        {
            x = 0;
            foreach (var symbol in line)
            {
                var point = new Point(x, y);
                switch (symbol)
                {
                    case 'S':
                    case 'a':
                        starts.Add(point);
                        height[point] = 'a';
                        break;
                    case 'E':
                        dest = point;
                        height[point] = 'z';
                        break;
                    default:
                        height[point] = symbol;
                        break;
                }
                ++x;
            }
            ++y;
        }

        var maxX = x;
        var maxY = y;

        var minDist = Int32.MaxValue;
        for (var i = 0; i < starts.Count; ++i)
        {
            var start = starts[i];
            var toVisit = new List<Point>();
            var dist = new Dictionary<Point, int>();
            for (x = 0; x < maxX; ++x)
            {
                for (y = 0; y < maxY; ++y)
                {
                    var point = new Point(x, y);
                    if (point == start)
                    {
                        dist[point] = 0;
                    }
                    else
                    {
                        dist[point] = Int32.MaxValue;
                        toVisit.Add(point);
                    }
                }
            }
            
            var current = start;
            while (toVisit.Contains(dest))
            {
                for (x = current.X - 1; x <= current.X + 1; ++x)
                {
                    for (y = current.Y - 1; y <= current.Y + 1; ++y)
                    {
                        if (x == current.X || y == current.Y)
                        {
                            var neighbour = new Point(x, y);
                            if (toVisit.Contains(neighbour) && height[neighbour] <= height[current] + 1)
                            {
                                var distFromCurrent = dist[current] + 1;
                                dist[neighbour] = Math.Min(distFromCurrent, dist[neighbour]);
                            }
                        }
                    }
                }

                toVisit.Remove(current);
                if (toVisit.Any())
                {
                    current = toVisit.MinBy(p => dist[p]);
                    if (dist[current] == Int32.MaxValue)
                    {
                        // Not actually reachable
                        break;
                    }
                }
            }

            minDist = Math.Min(minDist, dist[dest]);

            if (i % 10 == 0)
            {
                Console.WriteLine($"{i} / {starts.Count} (min: {minDist}");
            }
        }

        return minDist;
    }

}