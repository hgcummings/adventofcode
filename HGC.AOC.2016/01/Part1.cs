using HGC.AOC.Common;

namespace HGC.AOC._2016._01;

public class Part1 : ISolution
{
    public string? Answer()
    {
        var input = this.ReadResource("input.txt");

        var x = 0;
        var y = 0;
        var dx = 0;
        var dy = 1;

        foreach (string instruction in input.Split(", "))
        {
            var turn = instruction.Trim()[0];
            var length = Int32.Parse(instruction.Trim().Substring(1));
            
            switch(turn)
            {
                case 'R':
                    (dx, dy) = (dy, -dx);
                    break;
                case 'L':
                    (dx, dy) = (-dy, dx);
                    break;
                default:
                    throw new Exception($"Invalid instruction {instruction}.");
            }

            x += dx * length;
            y += dy * length;
        }

        return (Math.Abs(x) + Math.Abs(y)).ToString();
    }
}