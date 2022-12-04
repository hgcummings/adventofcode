using HGC.AOC.Common;

namespace HGC.AOC._2022._04;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");
        var total = 0;
        foreach (var line in input)
        {
            var ranges = line.Split(",").Select(def => ParseRange(def)).ToList();
            if (ranges[0].Contains(ranges[1]) || ranges[1].Contains(ranges[0]))
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

        public bool Contains(AssignedRange other)
        {
            return this.Start <= other.Start && this.End >= other.End;
        }
    }
}