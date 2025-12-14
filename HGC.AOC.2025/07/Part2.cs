using HGC.AOC.Common;

namespace HGC.AOC._2025._07;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var beams = new Tally<int>();
        var splitters = new List<List<int>>();

        foreach (var line in this.ReadInputLines())
        {
            if (line.Contains('S'))
            {
                beams.Increment(line.IndexOf('S'));
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

        foreach (var row in splitters)
        {
            var newBeams = new Tally<int>();
            foreach (var entry in beams)
            {
                if (row.Contains(entry.Key))
                {
                    newBeams.Increase(entry.Key - 1, entry.Value);
                    newBeams.Increase(entry.Key + 1, entry.Value);
                }
                else
                {
                    newBeams.Increase(entry.Key, entry.Value);
                }
            }

            beams = newBeams;
        }

        return beams.Total;
    }
}