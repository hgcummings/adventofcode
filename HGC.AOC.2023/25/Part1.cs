using System.Collections.Immutable;
using System.Drawing;
using Combinatorics.Collections;
using HGC.AOC.Common;

namespace HGC.AOC._2023._25;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var connections = this.ReadInputLines("input.txt")
            .Select(line => line.SplitBySpaces())
            .ToDictionary(parts => parts[0][..3], parts => parts[1..].ToList());

        foreach (var entry in connections.ToArray())
        {
            foreach (var to in entry.Value)
            {
                if (!connections.ContainsKey(to))
                {
                    connections[to] = new List<string>();
                }
                
                connections[to].Add(entry.Key);
            }
        }
        
        var depths = new Dictionary<string, int>();
        
        var components = connections.Keys.ToArray();
        
        for (var i = 0; i < components.Length; ++i)
        {
            var from = components[i];
    
            var visited = new HashSet<string>();
            var queue = new Queue<string>();
            queue.Enqueue(from);
    
            while (queue.TryDequeue(out var curr))
            {
                var tail = curr[^3..];
                
                if (visited.Add(tail))
                {
                    if (visited.Count == components.Length)
                    {
                        depths[from] = curr.Length;
                        break;
                    }
                    foreach (var next in connections[tail])
                    {
                        queue.Enqueue(curr + next);
                    }
                }
            }
        }
        
        // foreach (var entry in depths.OrderBy(c => c.Value))
        // {
        //     Console.WriteLine($"{entry.Key}: {entry.Value}");
        // }

        var minDepth = depths.Min(e => e.Value);
        var minNodes = depths.Where(e => e.Value == minDepth).Select(e => e.Key);

        var maxNode = depths.MaxBy(e => e.Value).Key;
        
        foreach (var triplet in new Combinations<string>(minNodes, 3))
        {
            var visited = new HashSet<string>();
            var queue = new Queue<string>();
            queue.Enqueue(maxNode);
    
            while (queue.TryDequeue(out var curr))
            {
                if (visited.Add(curr))
                {
                    if (!triplet.Contains(curr))
                    {
                        foreach (var next in connections[curr])
                        {
                            queue.Enqueue(next);
                        }
                    }
                }
            }

            if (visited.Count < components.Length)
            {
                Console.WriteLine(String.Join(", ", triplet));
                Console.WriteLine($"{visited.Count}/{components.Length}");
            }
        }

        return null;
    }
}