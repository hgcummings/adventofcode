using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2023._06;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt").ToList();

        var times = input[0].Substring(11)
            .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse);
        var distances = input[1].Substring(11)
            .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse);

        return times.Zip(distances).Select(race =>
        {
            var t = race.First;
            var d = race.Second;

            var min = (int) Math.Ceiling(((t - Math.Sqrt(t * t - 4 * d)) / 2) + 0.001);
            var max = (int) Math.Floor(((t + Math.Sqrt(t * t - 4 * d)) / 2) - 0.001);

            Console.WriteLine(min + ", " + max);
            
            return max - min + 1;
        }).Aggregate(1, (a, b) => a * b);
    }
}