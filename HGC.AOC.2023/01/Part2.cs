using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2023._01;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        int GetDigit(Match match)
        {
            switch (match.Value)
            {
                case "one":
                    return 1;
                case "two":
                    return 2;
                case "three":
                    return 3;
                case "four":
                    return 4;
                case "five":
                    return 5;
                case "six":
                    return 6;
                case "seven":
                    return 7;
                case "eight":
                    return 8;
                case "nine":
                    return 9;
                default:
                    return Int32.Parse(match.Value);
            }
        }

        var sum = input.Sum(line =>
        {
            var firstRegex = new Regex("[1-9]|one|two|three|four|five|six|seven|eight|nine");
            var lastRegex = new Regex("[1-9]|one|two|three|four|five|six|seven|eight|nine", RegexOptions.RightToLeft);
            
            var result = (10 * GetDigit(firstRegex.Match(line))) + GetDigit(lastRegex.Match(line));
            return result;
        });

        return sum.ToString();
    }
}