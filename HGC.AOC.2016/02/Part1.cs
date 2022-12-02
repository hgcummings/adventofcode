using HGC.AOC.Common;

namespace HGC.AOC._2016._02;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var sequence = new List<int> { 5 };
        var width = 3;
        var height = 3;
        foreach (var line in input.Where(l => l.Length > 0))
        {
            var current = sequence.Last();
            foreach (char dir in line.Trim())
            {
                if (dir == 'U' && current > width)
                {
                    current -= 3;
                }
                else if (dir == 'D' && current <= (width * (height - 1)))
                {
                    current += 3;
                }
                else if (dir == 'L' && current % width != 1)
                {
                    current -= 1;
                }
                else if (dir == 'R' && current % width != 0)
                {
                    current += 1;
                }
            }
            
            sequence.Add(current);
        }

        return String.Join("", sequence.Skip(1));
    }
}