using HGC.AOC.Common;

namespace HGC.AOC._2016._06;

public class Part2 : ISolution
{
    public string? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var counts = new List<Tally<char>>();
        
        foreach (var rawLine in input)
        {
            var line = rawLine.Trim();

            if (counts.Count == 0)
            {
                for (var i = 0; i < line.Length; ++i)
                {
                    counts.Add(new Tally<char>());
                }
            }
            
            for (var i = 0; i < line.Length; ++i)
            {
                counts[i].Increment(line[i]);
            }
        }

        return String.Join("", counts.Select(d => d.Lowest));
    }
}