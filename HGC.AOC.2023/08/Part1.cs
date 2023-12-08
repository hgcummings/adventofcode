using HGC.AOC.Common;

namespace HGC.AOC._2023._08;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        string directions = null;
        var map = new Dictionary<string, (string, string)>();
        
        foreach (var line in input)
        {
            if (directions == null)
            {
                directions = line.Trim();
            }
            else if (line.Trim() != String.Empty)
            {
                map[line.Substring(0, line.IndexOf(" "))]
                    = (line.Substring(line.IndexOf("(") + 1, 3),
                        line.Substring(line.IndexOf(", ") + 2, 3));
            }
        }

        var currentNode = "AAA";

        var step = 0;
        while (currentNode != "ZZZ")
        {
            var direction = directions[step % directions.Length];
            currentNode = direction == 'L' ? map[currentNode].Item1 : map[currentNode].Item2;
            ++step;
        }
        
        return step;
    }
}