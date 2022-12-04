using HGC.AOC.Common;

namespace HGC.AOC._2022._04;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");
        var total = 0;
        foreach (var line in input)
        {
            var ranges = line.Split(",").Select(def => ParseRange(def)).ToList();
            if (ranges[0].Overlaps(ranges[1]))
            {
                total++;
            }
        }
        return total;
    }

    private AssignedRange ParseRange(string def)
    {
        var indices = def.Split("-").Select(Int32.Parse).ToList();
        return new AssignedRange(indices[0], indices[1]);
    }

    struct AssignedRange
    {
        public int Start { get; }
        public int End { get; }

        public AssignedRange(int start, int end)
        {
            Start = start;
            End = end;
        }

        public bool Overlaps(AssignedRange other)
        {
            return Enumerable.Range(Start, End - Start + 1)
                .Intersect(Enumerable.Range(other.Start, other.End - other.Start + 1))
                .Any();
        }
    }
}