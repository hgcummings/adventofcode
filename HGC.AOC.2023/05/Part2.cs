using System.Diagnostics;
using HGC.AOC.Common;

namespace HGC.AOC._2023._05;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var maps = new List<List<Tuple<LongRange, LongRange>>>();

        List<Tuple<LongRange, LongRange>> currentMap = null;

        var currentRanges = new List<LongRange>();
        
        foreach (var line in input)
        {
            if (currentRanges.Count == 0)
            {
                var seedValues = line.Substring("seeds: ".Length).Trim().Split(" ").Select(Int64.Parse);
                long? start = null;

                foreach (var seedValue in seedValues)
                {
                    if (start == null)
                    {
                        start = seedValue;
                    }
                    else
                    {
                        currentRanges.Add(LongRange.FromLength(start.Value, seedValue));
                        start = null;
                    }
                }
                
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
                LongRange.FromLength(values[1], values[2]),
                LongRange.FromLength(values[0], values[2])));
        }
        
        maps.Add(currentMap);

        foreach (var map in maps)
        {
            var remainingRanges = currentRanges;
            var newRanges = new List<LongRange>();
            while (remainingRanges.Count > 0)
            {
                var nextRangesToCheck = new List<LongRange>();
                foreach (var range in remainingRanges)
                {
                    var foundMatch = false;
                    foreach (var entry in map)
                    {
                        if (range.Intersects(entry.Item1))
                        {
                            foundMatch = true;
                            var intersection = range.Intersect(entry.Item1, out List<LongRange> remainder);
                            newRanges.Add(intersection.Shift(entry.Item2.From - entry.Item1.From));
                            nextRangesToCheck.AddRange(remainder);
                        }
                    }

                    if (foundMatch == false)
                    {
                        newRanges.Add(range);
                    }
                }

                remainingRanges = nextRangesToCheck;
            }

            currentRanges = newRanges;
        }

        return currentRanges.Select(r => r.From).Min();
    }

    public struct LongRange
    {
        public static LongRange FromLength(long from, long length)
        {
            return new LongRange(from, from + length);
        }
        
        private LongRange(long from, long toExc)
        {
            From = from;
            ToExc = toExc;
        }

        public long From { get; }
        public long ToExc { get; }

        public bool Intersects(LongRange other)
        {
            return this.From < other.ToExc && this.ToExc > other.From;
        }

        public LongRange Shift(long shift)
        {
            return new LongRange(From + shift, ToExc + shift);
        }

        public LongRange Intersect(LongRange other, out List<LongRange> remainder)
        {
            if (!this.Intersects(other))
            {
                throw new Exception("Ranges don't intersect");
            }

            var intFrom = Math.Max(From, other.From);
            var intersection = new LongRange(intFrom, Math.Min(ToExc, other.ToExc));

            remainder = new List<LongRange>();
            
            if (From < intFrom)
            {
                remainder.Add(new LongRange(From, intFrom));
            }

            if (ToExc > intersection.ToExc)
            {
                remainder.Add(new LongRange(intersection.ToExc, ToExc));
            }

            return intersection;
        }

        public override string ToString()
        {
            return String.Format("[{0},{1})", From, ToExc);
        }
    }
}