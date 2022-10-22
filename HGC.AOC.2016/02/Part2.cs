using HGC.AOC.Common;

namespace HGC.AOC._2016._02;

public class Part2 : ISolution
{
    public string? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var pad = new List<string>
        {
            "  1  ",
            " 234 ",
            "56789",
            " ABC ",
            "  D  "
        };
        
        var sequence = new List<(int, int)> { (2, 0) };
        
        foreach (var line in input.Where(l => l.Length > 0))
        {
            var current = sequence.Last();
            foreach (char dir in line.Trim())
            {
                var nextPos = dir switch
                {
                    'U' => (current.Item1 - 1, current.Item2),
                    'D' => (current.Item1 + 1, current.Item2),
                    'L' => (current.Item1, current.Item2 - 1),
                    'R' => (current.Item1, current.Item2 + 1)
                };

                if (nextPos.Item1 < 5 && nextPos.Item2 < 5 &&
                    nextPos.Item1 >= 0 && nextPos.Item2 >= 0 &&
                    pad[nextPos.Item1][nextPos.Item2] != ' ')
                {
                    current = nextPos;
                }
            }
            
            sequence.Add(current);
        }

        return String.Join("", sequence.Skip(1).Select(pos => pad[pos.Item1][pos.Item2]));
    }
}