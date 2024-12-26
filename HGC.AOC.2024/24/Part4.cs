using System.Drawing;
using System.Globalization;
using HGC.AOC.Common;

namespace HGC.AOC._2024._24;

public class Part4 : ISolution
{
    public object? Answer()
    {
        var initialised = false;
        var gates = new List<Gate>();

        var swaps = new Dictionary<string, string>
        {
            { "z27", "kcd" },
            { "kcd", "z27" },
            { "z23", "pfn" },
            { "pfn", "z23" },
            { "shj", "z07" },
            { "z07", "shj" },
            { "tpk", "wkb" },
            { "wkb", "tpk" }
        };

        //return String.Join(",", swaps.Keys.Order());
        
        string Unswap(string label)
        {
            return swaps.ContainsKey(label) ? swaps[label] : label;
        }

        var aliases = new Dictionary<string, string>
        {
            { "ndg", "d01" }
        };

        string Unalias(string label)
        {
            return aliases.ContainsKey(label) ? aliases[label] : label;
        }
        
        var lines = this.ReadInputLines("input.txt")
            .SkipWhile(line => line != String.Empty)
            .Skip(1)
            .ToList();

        var aliasCount = aliases.Count;

        while (true)
        {
            foreach (var line in lines)
            {
                var parts = line.Split(" ");
                var dest = Unswap(parts[4]);
            
                var inputs = new[] { Unalias(parts[0]), Unalias(parts[2]) }.Order().ToList();

                var a = inputs[0];
                var b = inputs[1];
                var op = parts[1];

                if (a[0] == 'x' && b[0] == 'y')
                {
                    if (op == "XOR")
                    {
                        aliases[dest] = "a" + a[1..];
                    }

                    if (op == "AND")
                    {
                        aliases[dest] = "b" + a[1..];
                    }
                }

                if (a[0] == 'a' && b[0] == 'c')
                {
                    if (op == "AND")
                    {
                        aliases[dest] = "d" + a[1..];
                    }
                }
                    
                if (a[0] == 'b' && b[0] == 'd')
                {
                    if (op == "OR")
                    {
                        aliases[dest] = "c" + a[1..];
                    }
                }
            }

            if (aliases.Count == aliasCount)
            {
                break;
            }

            aliasCount = aliases.Count;
        }
        
        using (var writer = new StreamWriter("graph.txt"))
        {
            writer.WriteLine("digraph G {");
            foreach (var line in this.ReadInputLines("input.txt"))
            {
                if (line == String.Empty)
                {
                    initialised = true;
                    continue;
                }

                if (initialised)
                {
                    var parts = line.Split(" ");
                    var dest = Unswap(parts[4]);
                    
                    var inputs = new[] { Unalias(parts[0]), Unalias(parts[2]) }.Order().ToList();
                    
                    gates.Add(new Gate(Unalias(inputs[0]), Unalias(inputs[1]), parts[1], Unalias(dest)));
                    writer.WriteLine($"{Unalias(inputs[0])} -> {Unalias(dest)} [label={parts[1]}]");
                    writer.WriteLine($"{Unalias(inputs[1])} -> {Unalias(dest)} [label={parts[1]}]");
                }
            }
            writer.WriteLine("}");
        }

        foreach (var gate in gates.OrderBy(g => g.To))
        {
            Console.WriteLine($"{gate.A} {gate.Op} {gate.B} => {gate.To}");
        }
        
        string ResolveInput(string input)
        {
            if (input[0] == 'x' || input[0] == 'y')
            {
                return input;
            }

            var gate = gates.Single(g => g.To == input);

            var inputs = new[] { ResolveInput(gate.A), ResolveInput(gate.B) }.Order().ToList();
            
            return $"{inputs[0]} {gate.Op} {inputs[1]}";
        }

        var prevLine = "=";
        using (var writer = new StreamWriter("output.txt"))
        {
            foreach (var gate in gates.Where(g => g.To[0] == 'z').OrderBy(g => g.To))
            {
                var line = $"{ResolveInput(gate.To)} => {gate.To}";
                writer.WriteLine(line);
                if (prevLine.Length > 22 && !line.StartsWith(prevLine[..^22]))
                {
                    break;
                }
                prevLine = line;
            }   
        }
        
        return 0;
    }

    struct Gate(string a, string b, string op, string to)
    {
        public string A { get; } = a;
        public string B { get; } = b;
        public string To { get; } = to;
        public string Op { get; } = op;
    }
}