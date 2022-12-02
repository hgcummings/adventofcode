using System.Text.RegularExpressions;
using HGC.AOC.Common;
namespace HGC.AOC._2016._04;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        foreach (var room in GetValidRooms(input))
        {
            Console.WriteLine($"{room.Item1}: {room.Item2}");
        }

        return String.Empty;
    }

    private IEnumerable<(int, string)> GetValidRooms(IEnumerable<string> input)
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
                yield return (sector, String.Join("-", words
                    .Select(word => String.Concat(word
                        .Select(c => (char) ((c - 97 + sector) % 26 + 97))))));
            }
        }
    }
}