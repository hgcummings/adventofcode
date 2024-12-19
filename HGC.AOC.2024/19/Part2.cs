using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2024._19;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt").ToList();

        var towels = input[0].Split(", ").OrderByDescending(t => t.Length).ToList();
        
        var patterns = input[2..];

        var cache = new Dictionary<string, long>();
        cache[String.Empty] = 1;

        long Options(string pattern)
        {
            if (cache.TryGetValue(pattern, out var options))
            {
                return options;
            }

            var result = OptionsRaw(pattern);
            cache[pattern] = result;
            return result;
        }

        long OptionsRaw(String pattern)
        {
            return towels.Sum(t => pattern.StartsWith(t) ? Options(pattern[t.Length..]) : 0);
        }

        return patterns.Sum(Options);
    }
}