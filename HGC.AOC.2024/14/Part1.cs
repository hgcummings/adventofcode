using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2024._14;

public class Part1 : ISolution
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

        var time = 100;

        foreach (var robot in robots)
        {
            robot.PX = (((robot.PX + time * robot.VX) % Width) + Width) % Width;
            robot.PY = (((robot.PY + time * robot.VY) % Height) + Height) % Height;
        }
        
        Console.WriteLine($"After {time} second(s):");
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

        var counts = new[] { 0, 0, 0, 0 };
        foreach (var bot in robots)
        {
            if (bot.PX < Width / 2)
            {
                if (bot.PY < Height / 2)
                {
                    counts[0]++;
                }
                else if (bot.PY > Height / 2)
                {
                    counts[1]++;
                }
            }
            else if (bot.PX > Width / 2)
            {
                if (bot.PY < Height / 2)
                {
                    counts[2]++;
                }
                else if (bot.PY > Height / 2)
                {
                    counts[3]++;
                }
            }
        }

        return counts[0] * counts[1] * counts[2] * counts[3];
    }
    
    private class RobotData
    {
        public int PX { get; set; }
        public int PY { get; set; }
        public int VX { get; set; }
        public int VY { get; set; }
    }
}