using HGC.AOC.Common;

namespace HGC.AOC._2025._05;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var ranges = new List<LongRange>();

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

            for (var i = 0; i < ranges.Count; ++i)
            {
                for (var j = i + 1; j < ranges.Count; ++j)
                {
                    if (ranges[i].Intersects(ranges[j]))
                    {
                        ranges[i].Intersect(ranges[j], out var remainder);
                        ranges.RemoveAt(i);
                        ranges.InsertRange(i, remainder);
                    }
                }
            }

            for (var i = 0; i < ranges.Count; ++i)
            {
                for (var j = i + 1; j < ranges.Count; ++j)
                {
                    if (ranges[i].Intersects(ranges[j]))
                    {
                        ranges[i].Intersect(ranges[j], out var remainder);
                        ranges.RemoveAt(i);
                        ranges.InsertRange(i, remainder);
                    }
                }
            }
        }

        return ranges.Sum(r => r.ToExc - r.From);
    }
}