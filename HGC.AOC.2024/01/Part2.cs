using HGC.AOC.Common;

namespace HGC.AOC._2024._01;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var left = new List<int>();
        var right = new List<int>();

        foreach (string line in input)
        {
            var locations = line
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(Int32.Parse)
                .ToList();
            
            left.Add(locations[0]);
            right.Add(locations[1]);
        }

        return left.Sum(leftLoc => (long)leftLoc * right.Count(rightLoc => rightLoc == leftLoc));
    }
}