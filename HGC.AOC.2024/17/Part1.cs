using HGC.AOC.Common;

namespace HGC.AOC._2024._17;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var reg = new Dictionary<char, long>();
        var prog = new List<byte>();

        foreach (var line in input)
        {
            if (line.StartsWith("Register"))
            {
                var parts = line.Split(' ');
                reg[parts[1][0]] = Int64.Parse(parts[2]);
            }
            else if (line.StartsWith("Program"))
            {
                prog = line.Substring(line.IndexOf(' ') + 1)
                    .Split(',').Select(Byte.Parse).ToList();
            }
        }

        var output = new List<byte>();
        var i = 0;

        long Combo(short op) => op switch
        {
            0 => 0,
            1 => 1,
            2 => 2,
            3 => 3,
            4 => reg['A'],
            5 => reg['B'],
            6 => reg['C']
        };

        while (i < prog.Count)
        {
            switch (prog[i])
            {
                case 0:
                    reg['A'] /= 1 << (int) Combo(prog[i + 1]);
                    break;
                case 1:
                    reg['B'] ^= prog[i + 1];
                    break;
                case 2:
                    reg['B'] = Combo(prog[i + 1]) % 8;
                    break;
                case 3:
                    if (reg['A'] != 0)
                    {
                        i = prog[i + 1];
                        continue;
                    }

                    break;
                case 4:
                    reg['B'] ^= reg['C'];
                    break;
                case 5:
                    output.Add((byte)(Combo(prog[i+1]) % 8));
                    break;
                case 6:
                    reg['B'] = reg['A'] / (1 << (int) Combo(prog[i + 1]));
                    break;
                case 7:
                    reg['C'] = reg['A'] / (1 << (int) Combo(prog[i + 1]));
                    break;
            }

            i += 2;
        }
        
        return String.Join(',', output);
    }
}