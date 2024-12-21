using System.Collections.Immutable;
using System.Drawing;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2016._15;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var discRegex = new Regex(
            "Disc #[0-9]+ has (?'Count'[0-9]+) positions; at time=0, it is at position (?'Index'[0-9]+).");
        
        var discs = this.ReadInputLines("input.txt")
            .Select((line, i) =>
            {
                var disc = discRegex.Match(line).Parse<Disc>();
                disc.Index = (disc.Index + i + 1) % disc.Count;
                return disc;
            })
            .ToList();

        var largestDisc = discs.MaxBy(d => d.Count)!;
        var fromT = largestDisc.Index == 0 ? 0 : largestDisc.Count - largestDisc.Index;
        for (var t = fromT;; t += largestDisc.Count)
        {
            if (t % 100000 == 0)
            {
                Console.WriteLine(t);
            }
            if (!discs.Any(disc => ((disc.Index + t) % disc.Count) != 0))
            {
                return t;
            }
        }
    }

    public class Disc
    {
        public long Count { get; set; }
        public long Index { get; set; }

        public override string ToString()
        {
            return $"{Index}/{Count}";
        }
    }
}