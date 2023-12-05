using System.Diagnostics;
using HGC.AOC.Common;

namespace HGC.AOC._2023._05;

public class Part1 : ISolution
{
    public struct LongRange
    {
        public LongRange(long from, long length)
        {
            From = from;
            ToExc = from + length;
        }

        public long From { get; }
        public long ToExc { get; }
    }
    
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var maps = new List<List<Tuple<LongRange, LongRange>>>();

        List<Tuple<LongRange, LongRange>> currentMap = null;

        var currentValues = new List<long>();
        
        foreach (var line in input)
        {
            if (currentValues.Count == 0)
            {
                currentValues.AddRange(
                    line.Substring("seeds: ".Length).Trim().Split(" ").Select(Int64.Parse));
                continue;
            }

            if (line.Trim() == "")
            {
                continue;
            }

            if (line.Trim().EndsWith("map:"))
            {
                if (currentMap != null)
                {
                    maps.Add(currentMap);
                }
                
                currentMap = new List<Tuple<LongRange, LongRange>>();
                continue;
            }

            var values = line.Trim().Split(" ").Select(Int64.Parse).ToArray();
            Debug.Assert(currentMap != null, nameof(currentMap) + " != null");
            currentMap.Add(new Tuple<LongRange, LongRange>(
                new LongRange(values[1], values[2]),
                new LongRange(values[0], values[2])));
        }
        
        maps.Add(currentMap);

        Console.WriteLine(String.Join(", ", currentValues));
        foreach (var map in maps)
        {
            currentValues = currentValues.Select(v =>
                map
                    .Where(entry => v >= entry.Item1.From && v < entry.Item1.ToExc)
                    .Select(entry => (v - entry.Item1.From) + entry.Item2.From)
                    .SingleOrDefault(v)
            ).ToList();
            Console.WriteLine(String.Join(", ", currentValues));
        }

        return currentValues.Min();
    }
}