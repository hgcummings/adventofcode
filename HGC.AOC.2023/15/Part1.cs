using HGC.AOC.Common;

namespace HGC.AOC._2023._15;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var steps = this.ReadInputLines("input.txt").First().Split(",");

        return steps.Select(Hash).Sum();
    }

    public int Hash(string input)
    {
        byte curr = 0;
        foreach (int ch in input)
        {
            curr += (byte) ch;
            curr *= 17;
        }

        return curr;
    }
}