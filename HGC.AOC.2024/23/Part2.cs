using Combinatorics.Collections;
using HGC.AOC.Common;

namespace HGC.AOC._2024._23;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var links = new Dictionary<string, List<string>>();

        void AddLink(string from, string to)
        {
            if (!links.ContainsKey(from))
            {
                links[from] = new List<string>();
            }
            links[from].Add(to);
        }
        
        foreach (var line in this.ReadInputLines("input.txt"))
        {
            var parts = line.Split('-');
            AddLink(parts[0], parts[1]);
            AddLink(parts[1], parts[0]);
        }

        for (var n = 4;; ++n)
        {
            Console.WriteLine(n);
            
            var networks = FindSubsets(links, n)
                .Select(s => String.Join(',', s.Order())).Distinct().ToList();

            if (networks.Count == 1)
            {
                return networks[0];
            }

            foreach (var network in networks)
            {
                Console.WriteLine(network);
            }
            
            Console.WriteLine();
        }
    }

    IEnumerable<IEnumerable<string>> FindSubsets(Dictionary<string, List<string>> links, int n)
    {
        foreach (var entry in links)
        {
            var combinations = new Combinations<string>(entry.Value, n - 1);

            foreach (var combination in combinations)
            {
                if (combination.All(n1 => combination.All(n2 => n1 == n2 || links[n1].Contains
                        (n2))))
                {
                    yield return combination.Concat(new[] {entry.Key});
                }
            }
        }
    }
}