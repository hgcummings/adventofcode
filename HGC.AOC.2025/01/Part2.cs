using HGC.AOC.Common;

namespace HGC.AOC._2025._01;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var value = 50;
        var count = 0;

        foreach (var line in this.ReadInputLines("input.txt"))
        {
            var magnitude = Int32.Parse(line.Substring(1));
            var direction = (line[0] == 'L') ? -1 : 1;

            for (var step = 0; step < magnitude; ++step)
            {
                value += direction;

                if (value == -1)
                {
                    value = 99;
                }
                else if (value == 100)
                {
                    value = 0;
                }
                if (value == 0)
                {
                    ++count;
                }
            }
        }

        return count;
    }
}