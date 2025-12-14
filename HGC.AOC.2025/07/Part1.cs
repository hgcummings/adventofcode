using HGC.AOC.Common;

namespace HGC.AOC._2025._07;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var beams = new List<int>();
        var splitters = new List<List<int>>();

        foreach (var line in this.ReadInputLines())
        {
            if (beams.Count == 0)
            {
                beams.Add(line.IndexOf('S'));
            }
            else if (line.Contains('^'))
            {
                splitters.Add(line
                    .Select((c, i) => new { c, i })
                    .Where(x => x.c == '^')
                    .Select(x => x.i)
                    .ToList());
            }
        }

        var splitCount = 0;
        foreach (var row in splitters)
        {
            beams = beams.SelectMany(beam =>
            {
                if (row.Contains(beam))
                {
                    ++splitCount;
                    return new[] { beam - 1, beam + 1 };
                }

                return new[] { beam };
            }).Distinct().ToList();
        }

        return splitCount;
    }
}