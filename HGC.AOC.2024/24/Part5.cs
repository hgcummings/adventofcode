using System.Collections.Immutable;
using Combinatorics.Collections;
using HGC.AOC.Common;

namespace HGC.AOC._2024._24;

public class Part5 : ISolution
{
    public object? Answer()
    {
        var initialised = false;

        var swaps = new List<(string, string)>
        {
            ( "z27", "kcd" ),
            ( "z23", "pfn" ),
            ( "shj", "z07" ),
            ( "wkb", "tpk" )
        };
        
        var lines = this.ReadInputLines("input.txt")
            .SkipWhile(line => line != String.Empty)
            .Skip(1)
            .ToList();

        var outNodes = lines.Select(line => line.Split(" -> ")[1]).Distinct().ToList();
        var highestBit = outNodes.Where(n => n[0] == 'z').Order().Last();
        var nodesToLabel = outNodes.Where(n => n[0] != 'z').ToList();

        var allPairs = new Combinations<string>(outNodes, 2)
            .Select(pair => (pair[0], pair[1]))
            .ToList();

        var baseResult = ResolveLabels(lines, new List<(string, string)>(), highestBit);

        var bestSwaps = BestSwaps(
            lines,
            highestBit,
            nodesToLabel.Count,
            ImmutableList.Create<(string, string)>(),
            baseResult,
            allPairs);

        return String.Join(";", bestSwaps.First().Item1.Select(p => $"{p.Item1},{p.Item2}").Order());
    }

    public IEnumerable<(IReadOnlyList<(string, string)>, int)> BestSwaps(
        List<string> lines,
        string highestBit,
        int nodesToLabel,
        ImmutableList<(string, string)> baseSwaps,
        int baseResult,
        IReadOnlyList<(string, string)> allPairs)
    {
        if (baseSwaps.Count > 3)
        {
            Console.WriteLine("(Terminating)");
            Console.WriteLine();
            yield break;
        }
        
        Console.WriteLine(String.Join(";", baseSwaps.Select(p => $"{p.Item1},{p.Item2}").Order()));
        var bestPairs = allPairs
            .Where(p => !baseSwaps.Contains(p))
            .AsParallel()
            .Select(pair => (pair, ResolveLabels(lines, baseSwaps.Add(pair), highestBit)))
            .Where(result => result.Item2 > baseResult)
            .OrderByDescending(result => result.Item2)
            .ToList();

        var completeResult = bestPairs.FirstOrDefault(pair => pair.Item2 == nodesToLabel);
        if (completeResult.Item2 == nodesToLabel)
        {
            yield return (baseSwaps.Add(completeResult.pair), completeResult.Item2);
        }
        else
        {
            foreach (var pair in bestPairs)
            {
                foreach (var swaps in BestSwaps(lines, highestBit, nodesToLabel,
                             baseSwaps.Add(pair.pair), pair.Item2, allPairs))
                {
                    yield return swaps;
                }
            }
        }
    }

    public int ResolveLabels(
        IReadOnlyList<String> lines,
        IReadOnlyList<(string l, string r)> swaps,
        string highestBit)
    {
        string Unswap(string node)
        {
            foreach (var swap in swaps)
            {
                if (swap.l == node) return swap.r;
                if (swap.r == node) return swap.l;
            }

            return node;
        }
        
        var labels = new Dictionary<string, string>();

        string ResolveLabel(string node)
        {
            return labels.ContainsKey(node) ? labels[node] : node;
        }
        
        var aliasCount = labels.Count;

        while (true)
        {
            foreach (var line in lines)
            {
                var parts = line.Split(" ");
                var dest = Unswap(parts[4]);
            
                var inputs = new[] { ResolveLabel(parts[0]), ResolveLabel(parts[2]) }.Order().ToList();

                var inL = inputs[0];
                var inR = inputs[1];
                var op = parts[1];

                if (inL[0] == 'x' && inR[0] == 'y')
                {
                    if (op == "XOR" && inL[1..] != "00" && inR[1..] != "00")
                    {
                        labels[dest] = "a" + inL[1..];
                    }

                    if (op == "AND")
                    {
                        labels[dest] = "b" + inL[1..];
                    }
                }
                else if (inL[0] == 'a' && inR[0] == 'c')
                {
                    if (op == "AND")
                    {
                        labels[dest] = "d" + inL[1..];
                    }
                }
                else if (inL[0] == 'b' && inR[0] == 'd')
                {
                    if (op == "OR" && dest != highestBit)
                    {
                        labels[dest] = "c" + inL[1..];
                    }
                }
                else if (inL == "a01" && inR == "b00" && op == "AND")
                {
                    labels[dest] = "d01";
                }
            }

            if (labels.Count == aliasCount)
            {
                break;
            }

            aliasCount = labels.Count;
        }

        return labels.Count;
    }
}