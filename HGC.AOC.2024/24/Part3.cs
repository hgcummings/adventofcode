using System.Drawing;
using System.Globalization;
using HGC.AOC.Common;

namespace HGC.AOC._2024._24;

public class Part3 : ISolution
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
        };

        string Unswap(string label)
        {
            return swaps.ContainsKey(label) ? swaps[label] : label;
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
                    writer.WriteLine($"{parts[0]} -> {dest} [label={parts[1]}]");
                    writer.WriteLine($"{parts[2]} -> {dest} [label={parts[1]}]");
                    var inputs = new[] { parts[0], parts[2] }.Order().ToList();
                    gates.Add(new Gate(inputs[0], inputs[1], parts[1], dest));
                }
            }
            writer.WriteLine("}");
        }

        foreach (var gate in gates.OrderBy(g => g.X))
        {
            Console.WriteLine($"{gate.A} {gate.Op} {gate.B} => {gate.X}");
        }
        
        string ResolveInput(string input)
        {
            if (input[0] == 'x' || input[0] == 'y')
            {
                return input;
            }

            var gate = gates.Single(g => g.X == input);

            var inputs = new[] { ResolveInput(gate.A), ResolveInput(gate.B) }.Order().ToList();
            
            return $"{inputs[0]} {gate.Op} {inputs[1]}";
        }

        var prevLine = "=";
        using (var writer = new StreamWriter("output.txt"))
        {
            foreach (var gate in gates.Where(g => g.X[0] == 'z').OrderBy(g => g.X))
            {
                var line = $"{ResolveInput(gate.X)} => {gate.X}";
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

    struct Gate(string a, string b, string op, string x)
    {
        public string A { get; } = a;
        public string B { get; } = b;
        public string X { get; } = x;
        public string Op { get; } = op;
    }
}