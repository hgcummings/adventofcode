using System.Drawing;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2022._18;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var cubes = input.Select(line => line.Trim().Split(",").Select(Int32.Parse).ToList()).ToList();

        var openSides = 0;
        foreach (var cube in cubes)
        {
            var neighbours = cubes.Count(c =>
            {
                var distances = Enumerable.Range(0, 3)
                    .GroupBy(i => Math.Abs(c[i] - cube[i]))
                    .ToDictionary(g => g.Key);
                return distances.ContainsKey(0) && distances.ContainsKey(1) &&
                       distances[0].Count() == 2 && distances[1].Count() == 1;
            });
            openSides += 6 - neighbours;
        }

        return openSides;
    }
}