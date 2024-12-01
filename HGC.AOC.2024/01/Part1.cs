using HGC.AOC.Common;

namespace HGC.AOC._2024._01;

public class Part1 : ISolution
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
        
        left.Sort();
        right.Sort();

        return left.Zip(right, (leftLoc, rightLoc) => Math.Abs(leftLoc - rightLoc)).Sum();
    }
}