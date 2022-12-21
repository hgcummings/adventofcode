using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2022._14;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var map = new HashSet<Point>();

        var maxX = 0;
        var maxY = 0;

        foreach (var line in input)
        {
            var vertices = line.Split(" -> ").Select(v => v.Split(",").Select(Int32.Parse).ToArray()).ToArray();

            for (var i = 1; i < vertices.Length; ++i)
            {
                var from = new Point(vertices[i - 1][0], vertices[i - 1][1]);
                var to = new Point(vertices[i][0], vertices[i][1]);
                var x = from.X;
                while (true)
                {
                    maxX = Math.Max(maxX, x);
                    var y = from.Y;
                    while (true)
                    {
                        map.Add(new Point(x, y));
                        
                        maxY = Math.Max(maxY, y);
                        
                        if (y == to.Y)
                        {
                            break;
                        }
                       
                        y += Math.Sign(to.Y - from.Y);
                    }

                    if (x == to.X)
                    {
                        break;
                    }

                    x += Math.Sign(to.X - from.X);
                }
            }
        }

        var total = 0;
        
        while (true)
        {
            var grain = new Point(500, 0);

            while (grain.Y < maxY)
            {
                if (!map.Contains(grain with { Y = grain.Y + 1 }))
                {
                    grain = grain with { Y = grain.Y + 1 };
                }
                else if (!map.Contains(new Point(grain.X - 1, grain.Y + 1)))
                {
                    grain = new Point(grain.X - 1, grain.Y + 1);
                }
                else if (!map.Contains(new Point(grain.X + 1, grain.Y + 1)))
                {
                    grain = new Point(grain.X + 1, grain.Y + 1);
                }
                else
                {
                    map.Add(grain);
                    total += 1;
                    break;
                }
            }

            if (grain.Y == maxY)
            {
                // No further obstructions, so will keep falling indefinitely
                break;
            }
        }
        
        return total;
    }
}