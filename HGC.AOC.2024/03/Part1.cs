using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2024._03;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInput("input.txt");

        var exp = new Regex("mul\\((?'A'[0-9]{1,3}),(?'B'[0-9]{1,3})\\)");

        return exp.Matches(input)
            .Select(match => match.Parse<InstructionData>())
            .Select(data => (long) data.A * data.B)
            .Sum();
    }
    
    private class InstructionData
    {
        public int A { get; set; }
        public int B { get; set; }
    }
}
    
