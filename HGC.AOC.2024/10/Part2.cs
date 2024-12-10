using System.Drawing;
using System.Xml;
using HGC.AOC.Common;

namespace HGC.AOC._2024._10;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var map = this.ReadInputLines("input.txt")
            .Select(line => line.Select(c => c - 48).ToList())
            .ToList();

        int Trails(int x, int y)
        {
            var h = map[y][x];
            if (h == 9)
            {
                return 1;
            }
            var nh = h + 1;
            return
                (y > 0 ? (map[y - 1][x] == nh ? Trails(x, y - 1) : 0) : 0) +
                (y < map.Count - 1 ? (map[y + 1][x] == nh ? Trails(x, y + 1) : 0) : 0) +
                (x > 0 ? (map[y][x - 1] == nh ? Trails(x - 1, y) : 0) : 0) +
                (x < map[0].Count - 1 ? (map[y][x + 1] == nh ? Trails(x + 1, y) : 0) : 0);
        }

        var sum = 0;
        for (var y = 0; y < map.Count; ++y)
        {
            var row = map[y];
            for (var x = 0; x < row.Count; ++x)
            {
                if (row[x] == 0)
                {
                    sum += Trails(x, y);
                }
            }
        }

        return sum;
    }
}