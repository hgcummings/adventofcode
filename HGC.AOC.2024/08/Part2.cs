using System.Data;
using System.Drawing;
using System.Xml;
using HGC.AOC.Common;

namespace HGC.AOC._2024._08;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt").ToList();

        var antennae = new Dictionary<char, List<Point>>();

        for (var y = 0; y < input.Count; ++y)
        {
            var row = input[y];
            for (var x = 0; x < row.Length; ++x)
            {
                if (row[x] != '.')
                {
                    if (!antennae.ContainsKey(row[x]))
                    {
                        antennae.Add(row[x], new List<Point>());
                    }
                    antennae[row[x]].Add(new Point(x, y));
                }
            }
        }

        var antinodes = new HashSet<Point>();

        bool InBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < input[0].Length && y < input.Count;
        }
        
        foreach (var freq in antennae.Values)
        {
            foreach (var i in freq)
            {
                foreach (var j in freq)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    
                    var dx = j.X - i.X;
                    var dy = j.Y - i.Y;
                    var x = i.X;
                    var y = i.Y;
                    
                    var gcd = (int) Arithmetic.GreatestCommonDivisor(dx, dy);
                    dx /= gcd;
                    dy /= gcd;

                    while (InBounds(x, y))
                    {
                        antinodes.Add(new Point(x, y));
                        x += dx;
                        y += dy;
                    }
                }
            }
        }
        
        return antinodes.Count;
    }
}