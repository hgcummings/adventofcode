using HGC.AOC.Common;

namespace HGC.AOC._2025._06;

public class Part2 : ISolution
{
    public object? Answer()
    {
        List<string> rawOperands = new List<string>();
        string operators = null!;

        foreach (var line in this.ReadInputLines())
        {
            if (line[0] == '*' || line[0] == '+')
            {
                operators = line;
                break;
            }
            
            rawOperands.Add(line);
        }

        var total = 0L;

        for (var i = 0; i < operators.Length; ++i)
        {
            if (operators[i] != ' ')
            {
                Console.Write(operators[i]);
                var end = operators.IndexOfAny(['*', '+'], i + 1) - 1;
                if (end == -2)
                {
                    end = operators.Length;
                }
                var operands = new List<long>();
            
                for (var j = i; j < end; ++j)
                {
                    var operand = 0L;
                    var unit = 1L;
                    for (var k = rawOperands.Count - 1; k >=0; --k)
                    {
                        var digit = rawOperands[k].Substring(j, 1);
                        if (digit != " ")
                        {
                            operand += unit * Int64.Parse(digit);
                            unit *= 10;
                        }
                    }
                    operands.Add(operand);
                }
                
                Console.Write(String.Join(' ', operands));
                
                var result = operators[i] switch
                {
                    '*' => operands.Aggregate(1L, (acc, cur) => acc * cur),
                    '+' => operands.Sum(),
                    _ => throw new InvalidOperationException()
                };
                
                Console.WriteLine($" = {result}");
                total += result;
            }
        }

        return total;
    }
}