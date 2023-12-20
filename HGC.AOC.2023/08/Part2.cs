using HGC.AOC.Common;

namespace HGC.AOC._2023._08;

public class Part2 : ISolution
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

        var currentNodes = map.Keys.Where(IsStartNode).ToArray();
        var history = currentNodes.Select(node => new List<string> { node }).ToList();
        var offsets = new int[currentNodes.Length];

        var step = 0;
        while (offsets.Any(offset => offset == 0))
        {
            var direction = directions[step % directions.Length];

            for (var i = 0; i < currentNodes.Length; ++i)
            {
                if (offsets[i] != 0)
                {
                    continue;
                }
                
                currentNodes[i] = direction == 'L' ?
                    map[currentNodes[i]].Item1 : map[currentNodes[i]].Item2;

                var pastIndex = history[i].Count;
                while (pastIndex >= directions.Length)
                {
                    pastIndex -= directions.Length;
                    if (history[i][pastIndex] == currentNodes[i])
                    {
                        offsets[i] = pastIndex;
                    }
                }

                if (offsets[i] == 0)
                {
                    // Didn't find cycle yet
                    history[i].Add(currentNodes[i]);
                }
            }
            
            ++step;
        }
        
        Console.WriteLine(String.Join(", ", offsets));
        Console.WriteLine(String.Join(", ", history.Select(h => h.Count)));
        // Console.WriteLine(String.Join(", ", history[0]));
        // Console.WriteLine(String.Join(", ", history[1]));
        Console.WriteLine(String.Join(", ",
            history.Select((h, i) => h.Skip(offsets[i]).Count(IsEndNode))));
        Console.WriteLine(String.Join(", ",
            history.Select(h => h.FindIndex(IsEndNode))));

        return history
            .Select(h => (long) h.FindIndex(IsEndNode))
            .Aggregate(Arithmetic.LeastCommonMultiple);
    }

    private bool IsStartNode(string node)
    {
        return node.EndsWith("A");
    }

    private bool IsEndNode(string node)
    {
        return node.EndsWith("Z");
    }
}