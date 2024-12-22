using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2024._22;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt").Select(Int64.Parse);

        return input.Sum(s => Advance(s, 2000));
    }

    public long Advance(long num, int steps)
    {
        for (var i = 0; i < steps; ++i)
        {
            num = (num ^ (num * 64)) % 16777216;
            num = (num ^ (num / 32)) % 16777216;
            num = (num ^ (num * 2048)) % 16777216;
        }

        return num;
    }
}