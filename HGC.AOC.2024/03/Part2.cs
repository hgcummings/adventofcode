using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2024._03;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInput("input.txt");

        var exp = new Regex("(?'Instruction'mul|do(n't)?)\\(((?'A'[0-9]{1,3}),(?'B'[0-9]{1,3}))?\\)");

        return exp.Matches(input)
            .Select(match => match.Parse<InstructionData>())
            .Aggregate((true, 0L), (acc, inc) => inc.Instruction switch
            {
                "mul" => acc.Item1 ? (acc.Item1, acc.Item2 + inc.A.Value * inc.B.Value) : acc,
                "do" => (true, acc.Item2),
                "don't" => (false, acc.Item2)
            }, acc => acc.Item2);
    }
    
    private class InstructionData
    {
        public string Instruction { get; set; }
        public int? A { get; set; }
        public int? B { get; set; }
    }
}
    
