using HGC.AOC.Common;

namespace HGC.AOC._2016._01;

public class Part2: ISolution
{
    public object? Answer()
    {
        var input = this.ReadInput("input.txt");

        var x = 0;
        var y = 0;
        var dx = 0;
        var dy = 1;

        var history = new List<(int, int)>();

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

            for (var step = 0; step < length; ++step)
            {
                x += dx;
                y += dy;
                if (history.Contains((x, y)))
                {
                    return (Math.Abs(x) + Math.Abs(y)).ToString();
                }
                history.Add((x, y));
            }
        }

        return null;
    }
}