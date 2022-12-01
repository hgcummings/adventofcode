using HGC.AOC.Common;

namespace HGC.AOC._2022._01;

public class Part2 : ISolution
{
    public string? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var elves = new List<int>();
        var currentElf = 0;

        foreach (string line in input)
        {
            if (line.Trim() == String.Empty)
            {
                elves.Add(currentElf);
                currentElf = 0;
            }
            else
            {
                currentElf += Int32.Parse(line.Trim());
            }
        }

        return elves.OrderByDescending(x => x).Take(3).Sum().ToString();
    }
}