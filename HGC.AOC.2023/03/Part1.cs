using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2023._03;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var lines = this.ReadInputLines("input.txt").ToArray();
        var numRegex = new Regex("[0-9]+");

        var partSum = 0;

        IEnumerable<string> Neighbours(int line, Match location)
        {
            var minX = Math.Max(0, location.Index - 1);
            var maxX = Math.Min(location.Index + location.Length + 1, lines[line].Length);
            var maxLength = maxX - minX;
            if (line > 0)
            {
                yield return lines[line - 1].Substring(minX, maxLength);
            }

            if (location.Index > 0)
            {
                yield return lines[line].Substring(location.Index - 1, 1);
            }

            if (location.Index + location.Length < lines[line].Length)
            {
                yield return lines[line].Substring(location.Index + location.Length, 1);
            }

            if (line < lines.Length - 1)
            {
                yield return lines[line + 1].Substring(minX, maxLength);
            }
        }

        for (int y = 0; y < lines.Length; ++y)
        {
            var line = lines[y];
            var matches = numRegex.Matches(line);

            foreach (Match match in matches)
            {
                if (Neighbours(y, match).Any(n => n.Any(c => c != '.')))
                {
                    partSum += Int32.Parse(match.Value);
                }
            }
        }

        return partSum;
    }
}