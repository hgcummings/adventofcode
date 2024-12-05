using HGC.AOC.Common;

namespace HGC.AOC._2024._05;

public class Part2 : ISolution
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

        var comparer = new RuleComparer(rules);
        updates = updates.Where(update => !IsValid(update)).ToList();
        foreach (var update in updates)
        {
            update.Sort(comparer);
        }

        return updates.Sum(update => update[(update.Count - 1) / 2]);
    }

    class RuleComparer(List<(int a, int b)> rules) : IComparer<int>
    {
        private List<(int a, int b)> rules = rules;

        public int Compare(int x, int y)
        {
            foreach (var rule in rules)
            {
                if (rule.a == x && rule.b == y)
                {
                    return -1;
                }

                if (rule.a == y && rule.b == x)
                {
                    return 1;
                }
            }

            return 0;
        }
    }
}
    

