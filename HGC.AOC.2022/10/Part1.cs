using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2022._10;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        Action? executingInstruction = null;
        int executionCompletion = 0;
        var register = 1;
        var result = 0;

        var enumerator = input.GetEnumerator();

        for (var cycle = 1; cycle <= 220; ++cycle)
        {
            if ((cycle - 20) % 40 == 0)
            {
                Console.WriteLine($"{cycle} {register}");
                result += cycle * register;
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
        
        return result;
    }

}