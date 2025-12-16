using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2025._09;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var tiles = this.ReadInputLines("example.txt")
            .Select(line =>
            {
                var components = line.Split(',').Select(Int32.Parse).ToList();
                return new Point(components[0], components[1]);
            }).ToList();

        var max = 0L;

        for (var i = 0; i < tiles.Count; ++i)
        {
            for (var j = i + 1; j < tiles.Count; ++j)
            {
                var area = (Math.Abs(tiles[j].X - tiles[i].X) + 1L) *
                           (Math.Abs(tiles[j].Y - tiles[i].Y) + 1L);
                max = Math.Max(max, area);
            }
        }

        return max;
    }
}