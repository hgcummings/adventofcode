using HGC.AOC.Common;

namespace HGC.AOC._2022._03;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");
        var total = 0;
        foreach (var line in input)
        {
            var pocket1 = line.Substring(0, line.Length / 2);
            var pocket2 = line.Substring(line.Length / 2);

            total += Priority(pocket1.Intersect(pocket2).Single());
        }

        return total;
    }

    private int Priority(char item) => item > 96 ? item - 96 : item - 38;
}