using HGC.AOC.Common;

namespace HGC.AOC._2024._07;

public class Part1 : ISolution
{
    public object? Answer()
    {
        List<(long output, List<long> inputs)> lines =
            this.ReadInputLines("input.txt").Select(line =>
            {
                var parts = line.Split(":");
                return (Int64.Parse(parts[0]),
                    parts[1].Trim().Split(' ').Select(Int64.Parse).ToList());
            }).ToList();

        return lines.Sum(line =>
        {
            for (var i = 0; i < 1 << (line.inputs.Count - 1); ++i)
            {
                var operators = Enumerable.Range(0, line.inputs.Count - 1)
                    .Select(j => (1 << j & i) == 0);

                var total = operators.Zip(line.inputs.Skip(1))
                    .Aggregate(line.inputs[0], (curr, next) =>
                        next.First ? (curr * next.Second) : (curr + next.Second));

                if (total == line.output)
                {
                    return line.output;
                }
            }
            
            return 0;
        });
    }
}