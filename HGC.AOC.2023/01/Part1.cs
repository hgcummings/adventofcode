using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2023._01;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var sum = input.Sum(line =>
        {
            var digRegex = new Regex("[0-9]");
            var digits = digRegex.Matches(line);
            return (10 * Int32.Parse(digits.First().Value)) + Int32.Parse(digits.Last().Value);
        });

        return sum.ToString();
    }
}