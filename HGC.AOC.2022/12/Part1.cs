using System.Drawing;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2022._12;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var height = new Dictionary<Point, int>();
        var dist = new Dictionary<Point, int>();
        var toVisit = new List<Point>();

        Point start = default;
        Point dest = default;

        var y = 0;
        foreach (var line in input)
        {
            var x = 0;
            foreach (var symbol in line)
            {
                var point = new Point(x, y);
                switch (symbol)
                {
                    case 'S':
                        start = point;
                        height[point] = 'a';
                        dist[point] = 0;
                        break;
                    case 'E':
                        dest = point;
                        height[point] = 'z';
                        dist[point] = Int32.MaxValue;
                        toVisit.Add(point);
                        break;
                    default:
                        toVisit.Add(point);
                        height[point] = symbol;
                        dist[point] = Int32.MaxValue;
                        break;
                }
                ++x;
            }
            ++y;
        }

        var current = start;
        while (toVisit.Contains(dest))
        {
            for (var x = current.X - 1; x <= current.X + 1; ++x)
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
            }
        }

        return dist[dest];
    }

}