using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2025._11;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var connections = new Dictionary<string, List<string>>();

        foreach (var line in this.ReadInputLines())
        {
            var parts = line.
                Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            
            connections.Add(parts[0].Substring(0, parts[0].IndexOf(':')), parts.Skip(1).ToList());
        }

        
        long RoutesFrom(string start, bool visitedDac, bool visitedFft)
        {
            if (start == "out")
            {
                return (visitedDac && visitedFft) ? 1 : 0;
            }
            
            return connections[start].Sum(next => RoutesFromCached(
                next,
                visitedDac | (start == "dac"),
                visitedFft | (start == "fft")));
        }

        var cache = new Dictionary<CacheKey, long>();
        
        long RoutesFromCached(string start, bool visitedDac, bool visitedFft)
        {
            var cacheKey = new CacheKey(start, visitedDac, visitedFft);
            if (cache.TryGetValue(cacheKey, out var cached))
            {
                return cached;
            }

            var value = RoutesFrom(start, visitedDac, visitedFft);
            cache.Add(cacheKey, value);
            return value;
        }
        
        return RoutesFrom("svr", false, false);
    }

    struct CacheKey(string node, bool visitedDac, bool visitedFft)
    {
        public bool Equals(CacheKey other)
        {
            return Node == other.Node && VisitedDac == other.VisitedDac && VisitedFft == other.VisitedFft;
        }

        public override bool Equals(object? obj)
        {
            return obj is CacheKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Node, VisitedDac, VisitedFft);
        }

        public string Node = node;
        public bool VisitedDac = visitedDac;
        public bool VisitedFft = visitedFft;
    }
}