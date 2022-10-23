using HGC.AOC.Common;

namespace HGC.AOC._2016._03;

public class Part2 : ISolution
{
    public string? Answer()
    {
        var input = this.ReadInputLines("input.txt");
        var possibleCount = 0;
        var buffer = new List<List<int>>();

        foreach (var line in input)
        {
            buffer.Add(line.Trim()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(Int32.Parse).ToList());

            if (buffer.Count == 3)
            {
                for (var i = 0; i < 3; ++i)
                {
                    var sides = new List<int>
                    {
                        buffer[0][i], buffer[1][i], buffer[2][i]
                    };
                    
                    sides.Sort();
                    
                    if (sides[2] < sides[0] + sides[1])
                    {
                        possibleCount++;
                    }
                }
                
                buffer.Clear();
            }
        }

        return possibleCount.ToString();
    }
}