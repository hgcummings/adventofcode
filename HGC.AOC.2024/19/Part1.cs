using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2024._19;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt").ToList();

        var towels = input[0].Split(", ").OrderByDescending(t => t.Length).ToList();

        Console.WriteLine(String.Join(',', towels));
        
        var patterns = input[2..];

        var cache = new Dictionary<string, bool>();
        cache[String.Empty] = true;
        foreach (var towel in towels)
        {
            cache[towel] = true;
        }

        bool IsPossible(string pattern)
        {
            if (cache.TryGetValue(pattern, out var possible))
            {
                return possible;
            }

            var result = IsPossibleRaw(pattern);
            cache[pattern] = result;
            return result;
        }

        bool IsPossibleRaw(String pattern) {
            foreach (var towel in towels)
            {
                if (pattern.StartsWith(towel) && IsPossible(pattern[towel.Length..]))
                {
                    return true;
                }
            }

            return false;
        }

        return patterns.Count(p =>
        {
            Console.WriteLine(p);
            return IsPossible(p);
        });
    }
}