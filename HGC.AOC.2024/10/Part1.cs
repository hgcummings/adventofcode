using System.Drawing;
using System.Xml;
using HGC.AOC.Common;

namespace HGC.AOC._2024._10;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var map = this.ReadInputLines("input.txt")
            .Select(line => line.Select(c => c - 48).ToList())
            .ToList();

        IEnumerable<Point> Trails(int x, int y)
        {
            var e = Enumerable.Empty<Point>();
            
            var h = map[y][x];
            if (h == 9)
            {
                yield return new Point(x, y);
            }
            var nh = h + 1;

            if (y > 0 && map[y - 1][x] == nh)
            {
                foreach (var trail in Trails(x, y - 1))
                {
                    yield return trail;
                }
            }
            
            if (y < map.Count - 1 && map[y + 1][x] == nh)
            {
                foreach (var trail in Trails(x, y + 1))
                {
                    yield return trail;
                }
            }
            
            if (x > 0 && map[y][x - 1] == nh)
            {
                foreach (var trail in Trails(x - 1, y))
                {
                    yield return trail;
                }
            }
            
            if (x < map[0].Count - 1 && map[y][x + 1] == nh)
            {
                foreach (var trail in Trails(x + 1, y))
                {
                    yield return trail;
                }
            }
        }

        var sum = 0;
        for (var y = 0; y < map.Count; ++y)
        {
            var row = map[y];
            for (var x = 0; x < row.Count; ++x)
            {
                if (row[x] == 0)
                {
                    sum += Trails(x, y).Distinct().Count();
                }
            }
        }

        return sum;
    }
}