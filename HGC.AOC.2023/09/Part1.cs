using HGC.AOC.Common;

namespace HGC.AOC._2023._09;

public class Part1 : ISolution
{
    public object? Answer()
    {
        return this.ReadInputLines("input.txt")
            .Select(line => line.SplitBySpaces().Select(Int32.Parse))
            .Select(seq =>
            {
                var currentSeq = seq.ToList();
                var lastValues = new List<int> { currentSeq.Last() };
                while (currentSeq.Any(x => x != 0))
                {
                    var newSeq = new List<int>();
                    for (var i = 0; i < currentSeq.Count - 1; ++i)
                    {
                        newSeq.Add(currentSeq[i+1]-currentSeq[i]);
                    }
                    lastValues.Add(newSeq.Last());
                    currentSeq = newSeq;
                }

                return lastValues.Sum();
            }).Sum();
    }
}