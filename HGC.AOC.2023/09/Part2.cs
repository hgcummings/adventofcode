using HGC.AOC.Common;

namespace HGC.AOC._2023._09;

public class Part2 : ISolution
{
    public object? Answer()
    {
        return this.ReadInputLines("input.txt")
            .Select(line => line.SplitBySpaces().Select(Int32.Parse))
            .Select(seq =>
            {
                var currentSeq = seq.ToList();
                var firstValues = new List<int> { currentSeq.First() };
                while (currentSeq.Any(x => x != 0))
                {
                    var newSeq = new List<int>();
                    for (var i = 0; i < currentSeq.Count - 1; ++i)
                    {
                        newSeq.Add(currentSeq[i+1]-currentSeq[i]);
                    }
                    firstValues.Add(newSeq.First());
                    currentSeq = newSeq;
                }

                firstValues.Reverse();
                var result = firstValues.Aggregate((acc, curr) => curr - acc);
                return result;
            }).Sum();
    }
}