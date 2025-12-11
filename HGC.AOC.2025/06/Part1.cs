using HGC.AOC.Common;

namespace HGC.AOC._2025._06;

public class Part1 : ISolution
{
    public object? Answer()
    {
        List<List<long>> operands = null;
        var operators = new List<string>();

        foreach (var line in this.ReadInputLines())
        {
            var elements = line
                .Split(' ',
                    StringSplitOptions.TrimEntries |
                    StringSplitOptions.RemoveEmptyEntries);
            if (line[0] == '*' || line[0] == '+')
            {
                operators = elements.ToList();
                break;
            }

            if (operands == null)
            {
                operands = new List<List<long>>();

                for (var i = 0; i < elements.Length; ++i)
                {
                    operands.Add(new List<long>());
                }
            }
            
            for (var i = 0; i < elements.Length; ++i)
            {
                operands[i].Add(Int64.Parse(elements[i]));
            }
        }

        return operators.Select((op, i) => op switch
        {
            "*" => operands[i].Aggregate(1L, (acc, cur) => acc * cur),
            "+" => operands[i].Sum(),
            _ => throw new InvalidOperationException()
        }).Sum();
    }
}