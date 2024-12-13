using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2024._13;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInput("input.txt");

        var machineRegex =
            new Regex(
                "Button A: X\\+(?'AX'[0-9]+), Y\\+(?'AY'[0-9]+)\n" +
                "Button B: X\\+(?'BX'[0-9]+), Y\\+(?'BY'[0-9]+)\n" +
                "Prize: X=(?'PX'[0-9]+), Y=(?'PY'[0-9]+)");

        var machines = 
            machineRegex.Matches(input).Select(match => match.Parse<Machine>());

        var cheapest = machines.Select(m =>
        {
            var minCost = int.MaxValue;

            for (var i = 0; i < 100; ++i)
            {
                var x = m.PX - i * m.BX;
                var y = m.PY - i * m.BY;

                if (x < 0 || y < 0)
                {
                    break;
                }
                
                if (x % m.AX == 0 && y % m.AY == 0 && x / m.AX == y / m.AY)
                {
                    var cost = i + 3 * (x / m.AX);
                    minCost = Math.Min(minCost, cost);
                }
            }

            return minCost == Int32.MaxValue ? 0 : minCost;
        });
        
        return cheapest.Sum();
    }
    
    private class Machine
    {
        public int AX { get; set; }
        public int AY { get; set; }
        
        public int BX { get; set; }
        public int BY { get; set; }
        
        public int PX { get; set; }
        public int PY { get; set; }
    }
}