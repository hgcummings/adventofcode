using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2022._09;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var visited = new HashSet<Point>();

        var start = new Point(0, 0);
        var knots = new List<Point>();
        for (var i = 0; i < 10; ++i)
        {
            knots.Add(start);
        }

        visited.Add(start);

        foreach (var line in input)
        {
            var direction = line[0];
            var magnitude = Int32.Parse(line.Split(" ")[1]);

            var dx = direction switch
            {
                'R' => 1,
                'L' => -1,
                _ => 0
            };

            var dy = direction switch
            {
                'U' => 1,
                'D' => -1,
                _ => 0
            };

            for (var step = 0; step < magnitude; ++step)
            {
                knots[0] = new Point(knots[0].X + dx, knots[0].Y + dy);

                for (var i = 1; i < 10; ++i)
                {
                    var distX = knots[i-1].X - knots[i].X;
                    var distY = knots[i-1].Y - knots[i].Y;

                    if (Math.Abs(distX) > 1 || Math.Abs(distY) > 1)
                    {
                        knots[i] = new Point(
                            knots[i].X + (distX == 0 ? 0 : distX / Math.Abs(distX)),
                            knots[i].Y + (distY == 0 ? 0 : distY / Math.Abs(distY))
                        );
                    }
                }
                
                visited.Add(knots[9]);
            }
        }
        
        return visited.Count;
    }

}