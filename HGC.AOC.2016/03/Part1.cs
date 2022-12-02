using HGC.AOC.Common;

namespace HGC.AOC._2016._03;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");
        var possibleCount = 0;

        foreach (var line in input)
        {
            var sides = line.Trim()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(Int32.Parse).OrderByDescending(s => s).ToList();

            if (sides[0] < sides.Skip(1).Sum())
            {
                possibleCount++;
            }
        }

        return possibleCount.ToString();
    }
}