using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2016._04;

public class Part1 : ISolution
{
    public string? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        return GetSectors(input).Sum().ToString();
    }

    private IEnumerable<int> GetSectors(IEnumerable<string> input)
    {
        foreach (var line in input)
        {
            var words = line.Split("-").ToList();
            var metadata = words.Last();
            words.Remove(metadata);

            var match = Regex.Match(metadata, "(?'sector'[0-9]+)\\[(?'checksum'[a-z]+)\\]");
            
            var sector = Int32.Parse(match.Groups["sector"].Value);
            var checksum = match.Groups["checksum"].Value;

            var sum = words
                .SelectMany(word => word)
                .GroupBy(letter => letter)
                .OrderByDescending(g => g.Count()).ThenBy(g => g.Key)
                .Select(g => g.Key)
                .Take(5);

            if (String.Concat(sum) == checksum)
            {
                yield return sector;
            }
        }
    }
}