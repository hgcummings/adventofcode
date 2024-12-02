using HGC.AOC.Common;

namespace HGC.AOC._2024._02;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        return input.Count(line =>
        {
            int? direction = null;
            int? prev = null;
            foreach (var entry in line.
                         Split(' ', StringSplitOptions.RemoveEmptyEntries).
                         Select(Int32.Parse))
            {
                if (prev.HasValue)
                {
                    if (direction.HasValue)
                    {
                        if (Math.Sign(entry - prev.Value) != direction)
                        {
                            Console.WriteLine($"{line} is unsafe: {prev} to {entry} changes direction");
                            return false;
                        }
                    }
                    else
                    {
                        direction = Math.Sign(entry - prev.Value);
                        if (direction == 0)
                        {
                            return false;
                        }
                    }

                    var distance = Math.Abs(entry - prev.Value);
                    if (distance is < 1 or > 3)
                    {
                        Console.WriteLine($"{line} is unsafe: {prev} to {entry} differ by {distance}");
                        return false;
                    }
                }

                prev = entry;
            }

            Console.WriteLine($"{line} is safe");
            return true;
        });
    }
}