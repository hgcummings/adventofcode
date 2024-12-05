using HGC.AOC.Common;

namespace HGC.AOC._2024._05;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");
        var rules = new List<(int a, int b)>();
        var updates = new List<List<int>>();

        var endOfRules = false;
        foreach (var line in input)
        {
            if (endOfRules)
            {
                updates.Add(line.Split(',').Select(Int32.Parse).ToList());
            }
            else if (line.Trim() == String.Empty)
            {
                endOfRules = true;
            }
            else
            {
                var parts = line.Split('|');
                rules.Add((Int32.Parse(parts[0]), Int32.Parse(parts[1])));
            }
        }

        bool IsValid(List<int> update)
        {
            for (var i = 0; i < update.Count; ++i)
            {
                for (var j = i + 1; j < update.Count; ++j)
                {
                    if (rules.Any(rule => rule.b == update[i] && rule.a == update[j]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        return updates.Where(IsValid).Sum(update => update[(update.Count - 1) / 2]);
    }
}
    
