using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2024._23;

public class Part1 : ISolution
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

        var triples = new List<string>();

        foreach (var entry in links)
        {
            foreach (var first in entry.Value)
            {
                foreach (var second in entry.Value)
                {
                    if (first != second && links[first].Contains(second))
                    {
                        triples.Add(String.Join(',',
                            new[] { entry.Key, first, second }.Order()));
                    } 
                }
            }
        }

        triples = triples.Distinct().ToList();

        return triples.Count(t => t[0] == 't' || t[3] == 't' || t[6] == 't');
    }
}