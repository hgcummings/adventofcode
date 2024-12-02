using HGC.AOC.Common;

namespace HGC.AOC._2024._02;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        return input.Count(line => IsSafe(line.
            Split(' ', StringSplitOptions.RemoveEmptyEntries).
            Select(Int32.Parse).ToList())
        );
    }

    private bool IsSafe(List<int> report)
    {
        return IsSafe(report, null) || 
               Enumerable.Range(0, report.Count).Any(skip => IsSafe(report, skip));
    }
    
    private bool IsSafe(List<int> report, int? skip) {
        int? direction = null;
        int? prev = null;
        for (var i = 0; i < report.Count; ++i)
        {
            if (i == skip)
            {
                continue;
            }

            var entry = report[i];
            
            if (prev.HasValue)
            {
                if (direction.HasValue)
                {
                    if (Math.Sign(entry - prev.Value) != direction)
                    {
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
                    return false;
                }
            }

            prev = entry;
        }
        return true;
    }
}