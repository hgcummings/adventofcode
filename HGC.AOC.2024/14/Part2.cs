using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2024._14;

public class Part2 : ISolution
{
    private const int Width = 101;
    private const int Height = 103;
    
    public object? Answer()
    {
        var input = this.ReadInput("input.txt");

        var robotRegex =
            new Regex("p=(?'PX'-?[0-9]+),(?'PY'-?[0-9]+) v=(?'VX'-?[0-9]+),(?'VY'-?[0-9]+)");

        var robots = 
            robotRegex.Matches(input).Select(match => match.Parse<RobotData>()).ToList();

        var time = 1;

        for (var i = 1; ; ++i)
        {
            foreach (var robot in robots)
            {
                robot.PX = (((robot.PX + time * robot.VX) % Width) + Width) % Width;
                robot.PY = (((robot.PY + time * robot.VY) % Height) + Height) % Height;
            }

            if (Enumerable.Range(0, Height).Max(y =>
                    Math.Abs(robots.Count(r => r.PX < Width / 2 && r.PY == y) -
                    robots.Count(r => r.PX > Width / 2 && r.PY == y))) < 10)
            {
                if (i % 100000 == 0)
                {
                    Console.WriteLine(i);
                }
                continue;
            }
            
            Console.WriteLine($"After {i} second(s):");
            for (var y = 0; y < Height; ++y)
            {
                for (var x = 0; x < Width; ++x)
                {
                    var count = robots.Count(r => r.PX == x && r.PY == y);
                    Console.Write(count == 0 ? "." : count.ToString());
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
    
    private class RobotData
    {
        public int PX { get; set; }
        public int PY { get; set; }
        public int VX { get; set; }
        public int VY { get; set; }
    }
}