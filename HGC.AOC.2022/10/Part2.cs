using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2022._10;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        Action? executingInstruction = null;
        int executionCompletion = 0;
        var register = 1;
        var currentRow = "";

        var enumerator = input.GetEnumerator();

        for (var cycle = 1; ; ++cycle)
        {
            currentRow += Math.Abs(register - currentRow.Length) <= 1 ? '#' : '.';
            
            if (currentRow.Length == 40)
            {
                Console.WriteLine(currentRow);
                currentRow = "";
            }
            
            if (executingInstruction != null)
            {
                if (executionCompletion == cycle)
                {
                    executingInstruction();
                    executingInstruction = null;
                }
            }
            else if (enumerator.MoveNext())
            {
                switch (enumerator.Current.Trim().Split(" ")) 
                {
                    case ["noop"]:
                    {
                        break;
                    }
                    case ["addx", { } val]:
                    {
                        var value = Int32.Parse(val);
                        executingInstruction = () => register += value;
                        executionCompletion = cycle + 1;
                        break;
                    }
                }
            }
            else
            {
                break;
            }
        }
        return null;
    }

}