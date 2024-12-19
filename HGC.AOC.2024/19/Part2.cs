using System.Collections.Concurrent;
using HGC.AOC.Common;

namespace HGC.AOC._2024._19;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt").ToList();
        var towels = input[0].Split(", ").ToList();

        var cache = new ConcurrentDictionary<string, long>();
        cache.TryAdd(String.Empty, 1);

        long Options(string pattern) =>
            cache.GetOrAdd(pattern,
                p => towels.Sum(t => p.StartsWith(t) ? Options(p[t.Length..]) : 0));

        return input[2..].AsParallel().Sum(Options);
    }
}