using HGC.AOC.Common;

namespace HGC.AOC._2025._05;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var ranges = new List<LongRange>();
        var ids = new List<long>();

        using (var lines = this.ReadInputLines().GetEnumerator())
        {
            while (lines.MoveNext())
            {
                var line = lines.Current;
                if (String.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                var limits = line.Split('-').Select(Int64.Parse).ToList();
                ranges.Add(LongRange.FromToInclusive(limits[0], limits[1]));
            }

            while (lines.MoveNext())
            {
                var line = lines.Current;
                if (String.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                ids.Add(Int64.Parse(line));
            }
        }
        
        return ids.Count(id => ranges.Any(range => range.Contains(id)));
    }
}