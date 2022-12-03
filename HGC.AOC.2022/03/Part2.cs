using HGC.AOC.Common;

namespace HGC.AOC._2022._03;

public class Part2 : ISolution
{
    public object? Answer()
    {
        return this.ReadInputLines("input.txt")
            .Chunk(3)
            .Select(group => group[0].Intersect(group[1]).Intersect(group[2]).Single())
            .Sum(Priority);
    }

    private int Priority(char item) => item > 96 ? item - 96 : item - 38;
}