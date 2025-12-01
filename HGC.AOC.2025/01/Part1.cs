using HGC.AOC.Common;

namespace HGC.AOC._2025._01;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var value = 50;
        var count = 0;

        foreach (var line in this.ReadInputLines("input.txt"))
        {
            value += 100;
            var move = Int32.Parse(line.Substring(1));
            if (line[0] == 'L')
            {
                move *= -1;
            }

            value = (value + move) % 100;
            if (value == 0)
            {
                ++count;
            }
        }

        return count;
    }
}