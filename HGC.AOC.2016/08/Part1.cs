using System.Text.RegularExpressions;
using HGC.AOC.Common;
using static HGC.AOC.Common.ArrayHelpers;

namespace HGC.AOC._2016._08;

public class Part1 : ISolution
{
    private const int Width = 50;
    private const int Height = 6;

    public string? Answer()
    {
        var input = this.ReadInputLines("input.txt");
        
        var display = InitArray(Height, _ => InitArray(Width, _ => false));

        var rules = new Dictionary<Regex, Action<InstructionData>>
        {
            { new Regex("rotate row y=(?'A'[0-9]+) by (?'B'[0-9]+)"), RotateRow },
            { new Regex("rotate column x=(?'A'[0-9]+) by (?'B'[0-9]+)"), RotateCol },
            { new Regex("rect (?'A'[0-9]+)x(?'B'[0-9]+)"), DrawRect }
        };

        void RotateRow(InstructionData data)
        {
            display[data.A] = InitArray(Width, i => display[data.A][(i - data.B + Width) % Width]);
        }
        
        void RotateCol(InstructionData data)
        {
            var newCol = InitArray(Height, i => display[(i - data.B + Height) % Height][data.A]);
            
            for (var x = 0; x < Height; ++x)
            {
                display[x][data.A] = newCol[x];
            }
        }
        
        void DrawRect(InstructionData data)
        {
            for (var y = 0; y < data.B; ++y)
            {
                for (var x = 0; x < data.A; ++x)
                {
                    display[y][x] = true;
                }
            }
        }
        
        foreach (var line in input)
        {
            var foundRule = false;
            foreach (var rule in rules)
            {
                var match = rule.Key.Match(line);
                if (match.Success)
                {
                    foundRule = true;
                    rule.Value(match.Parse<InstructionData>());
                    break;
                }
            }

            if (!foundRule)
            {
                throw new Exception($"Invalid instruction {line}");
            }
        }

        var count = 0;
        foreach (var row in display)
        {
            Console.WriteLine(String.Join("", row.Select(p => p ? '#' : '.')));
            count += row.Count(p => p);
        }

        return count.ToString();
    }
    
    private class InstructionData
    {
        public int A { get; set; }
        public int B { get; set; }
    }
}