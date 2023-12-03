using System.Drawing;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2023._03;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var lines = this.ReadInputLines("input.txt").ToArray();
        var numRegex = new Regex("[0-9]+");

        var gearLocations = new Dictionary<Point, List<int>>();

        IEnumerable<Point> GearNeighbours(int line, Match location)
        {
            var minX = Math.Max(0, location.Index - 1);
            var maxX = Math.Min(location.Index + location.Length + 1, lines[line].Length);
            if (line > 0)
            {
                for (var x = minX; x < maxX; ++x)
                {
                    if (lines[line - 1][x] == '*')
                    {
                        yield return new Point(x, line - 1);
                    }
                }
            }

            if (location.Index > 0 && lines[line][location.Index - 1] == '*')
            {
                yield return new Point(location.Index - 1, line);
            }

            if ((location.Index + location.Length < lines[line].Length) && lines[line][location.Index + location.Length] == '*')
            {
                yield return new Point(location.Index + location.Length, line);
            }

            if (line < lines.Length - 1)
            {
                for (var x = minX; x < maxX; ++x)
                {
                    if (lines[line + 1][x] == '*')
                    {
                        yield return new Point(x, line + 1);
                    }
                }
            }
        }

        for (int y = 0; y < lines.Length; ++y)
        {
            var line = lines[y];
            var matches = numRegex.Matches(line);

            foreach (Match match in matches)
            {
                foreach (var gear in GearNeighbours(y, match))
                {
                    if (gearLocations.ContainsKey(gear))
                    {
                        gearLocations[gear].Add(Int32.Parse(match.Value));
                    }
                    else
                    {
                        gearLocations[gear] = new List<int> { Int32.Parse(match.Value) };
                    }
                }
            }
        }

        return gearLocations
            .Where(g => g.Value.Count == 2)
            .Select(g => g.Value[0] * g.Value[1])
            .Sum();
    }
}