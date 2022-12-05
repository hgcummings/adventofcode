using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2022._05;

public class Part1 : ISolution
{
    private const int StackCount = 9;
    
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");
        // var input = this.ReadInputLines("example.txt");
        var stacks = new List<List<char>>();
        for (var i = 0; i < StackCount; ++i)
        {
            stacks.Add(new List<char>());
        }

        var moveExpr = new Regex("move (?'Count'[0-9]+) from (?'From'[0-9]+) to (?'To'[0-9]+)");
        bool running = false;
        foreach (var line in input)
        {
            if (line.Trim() == String.Empty)
            {
                continue;
            }
            if (line.StartsWith(" 1   2   3"))
            {
                foreach (var stack in stacks)
                {
                    Console.WriteLine(String.Concat(stack));
                }
                running = true;
                continue;
            }
            if (running) {
                var match = moveExpr.Match(line);
                var move = match.Parse<MoveData>();
                for (var i = 0; i < move.Count; ++i)
                {
                    var from = stacks[move.From - 1];
                    var crate = from[^1];
                    from.RemoveAt(from.Count - 1);
                    stacks[move.To - 1].Add(crate);
                }
            }
            else
            {
                for (var i = 0; i < StackCount; ++i)
                {
                    var pos = 1 + (4 * i);
                    if (line[pos] != ' ')
                    {
                        stacks[i].Insert(0, line[pos]);
                    }
                }
            }
        }

        return String.Concat(stacks.Select(stack => stack.Last()));
    }
    
    private class MoveData
    {
        public int Count { get; set; }
        public int From { get; set; }
        public int To { get; set; }
    }
}