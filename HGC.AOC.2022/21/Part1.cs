using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2022._21;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var monkeys = new Dictionary<string, Func<long>>();

        foreach (var line in input)
        {
            var parts = line.Split(": ");
            var id = parts[0];
            var value = parts[1].Split(" ");
            if (value.Length == 1)
            {
                var val = Int32.Parse(value[0]);
                monkeys.Add(id, () => val);
            }
            else
            {
                monkeys.Add(id, () => value[1] switch
                    {
                        "+" => monkeys[value[0]]() + monkeys[value[2]](),
                        "-" => monkeys[value[0]]() - monkeys[value[2]](),
                        "*" => monkeys[value[0]]() * monkeys[value[2]](),
                        "/" => monkeys[value[0]]() / monkeys[value[2]]()
                    }
                );
            }
        }
        
        return monkeys["root"]();
    }
}