using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2024._13;

public class Part2 : ISolution
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

        return machines.Sum(m =>
        {
            m.PX += 10000000000000;
            m.PY += 10000000000000;

            var bNum = m.AX * m.PY - m.AY * m.PX;
            var bDnm = m.BY * m.AX - m.AY * m.BX;

            if (bNum % bDnm != 0)
            {
                // Lines intercept at non-integer point
                return 0;
            }

            var bSteps = bNum / bDnm;

            var aDist = m.PX - m.BX * bSteps;

            if (aDist % m.AX != 0)
            {
                // Should never happen
                Console.WriteLine("Error");
                return 0;
            }

            var aSteps = aDist / m.AX;

            return bSteps + 3 * aSteps;
        });
    }
    
    private class Machine
    {
        public int AX { get; set; }
        public int AY { get; set; }
        
        public int BX { get; set; }
        public int BY { get; set; }
        
        public long PX { get; set; }
        public long PY { get; set; }
    }
}