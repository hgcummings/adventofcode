using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2022._09;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var visited = new HashSet<Point>();

        var start = new Point(0, 0);
        var head = start;
        var tail = start;

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

            for (var i = 0; i < magnitude; ++i)
            {
                head = new Point(head.X + dx, head.Y + dy);

                var distX = head.X - tail.X;
                var distY = head.Y - tail.Y;

                if (Math.Abs(distX) > 1 || Math.Abs(distY) > 1)
                {
                    tail = new Point(
                        tail.X + (distX == 0 ? 0 : distX / Math.Abs(distX)),
                        tail.Y + (distY == 0 ? 0 : distY / Math.Abs(distY))
                    );
                    visited.Add(tail);
                }
            }
        }
        
        return visited.Count;
    }

}