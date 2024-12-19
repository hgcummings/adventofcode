using System.Collections.Immutable;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2016._12;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var lines = this.ReadInputLines("input.txt")
            .Select(line => line
                .Split(' ')
                .Select(elem => Int64.TryParse(elem, out var val) ?
                    (object) val : elem.Length == 1 ? elem[0] : elem)
                .ToList())
            .ToList();
        var i = 0;

        var mem = new Dictionary<char, long>();
        mem['a'] = 0;
        mem['b'] = 0;
        mem['c'] = 0;
        mem['d'] = 0;

        while (i < lines.Count)
        {
            switch (lines[i])
            {
                case ["cpy", char from, char to]:
                    mem[to] = mem[from];
                    break;
                case ["cpy", long from, char to]:
                    mem[to] = from;
                    break;
                case ["inc", char reg]:
                    mem[reg] += 1;
                    break;
                case ["dec", char reg]:
                    mem[reg] -= 1;
                    break;
                case ["jnz", char test, char to]:
                    if (mem[test] != 0)
                    {
                        i += (int) mem[to];
                        continue;
                    }

                    break;
                case ["jnz", long test, char to]:
                    if (test != 0)
                    {
                        i += (int) mem[to];
                        continue;
                    }
                    break;
                
                case ["jnz", char test, long to]:
                    if (mem[test] != 0)
                    {
                        i += (int) to;
                        continue;
                    }

                    break;
                case ["jnz", long test, long to]:
                    if (test != 0)
                    {
                        i += (int) to;
                        continue;
                    }
                    break;
            }

            ++i;
        }
        
        return mem['a'];
    }
}