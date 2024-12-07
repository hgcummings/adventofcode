using HGC.AOC.Common;

namespace HGC.AOC._2024._07;

public class Part2 : ISolution
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

        return lines.Where(IsValid).Sum(line => line.output);
    }

    bool IsValid((long output, List<long> inputs) line)
    {
        if (line.inputs.Count == 1)
        {
            return line.output == line.inputs[0];
        }
        
        var last = line.inputs[^1];
        
        if (line.output % last == 0 && IsValid((line.output / last, line.inputs[..^1])))
        {
            return true;
        }

        if (line.output > last && IsValid((line.output - last, line.inputs[..^1])))
        {
            return true;
        }

        var lastString = last.ToString();
        var outString = line.output.ToString();
        if (outString.EndsWith(lastString) && outString.Length > lastString.Length &&
            IsValid((Int64.Parse(outString[..^lastString.Length]), line.inputs[..^1])))
        {
            return true;
        }

        return false;
    }
}