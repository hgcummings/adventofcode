using System.Drawing;
using System.Globalization;
using HGC.AOC.Common;

namespace HGC.AOC._2024._24;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var values = new Dictionary<string, bool>();
        var gates = new Queue<Gate>();

        var initialised = false;
        foreach (var line in this.ReadInputLines("input.txt"))
        {
            if (line == String.Empty)
            {
                initialised = true;
                continue;
            }

            if (!initialised)
            {
                var parts = line.Split(": ");
                values[parts[0]] = parts[1] == "1";
            }
            else
            {
                var parts = line.Split(" ");
                gates.Enqueue(new Gate(parts[0], parts[2], parts[1], parts[4]));
            }
        }

        while (gates.Count > 0)
        {
            var gate = gates.Dequeue();
            if (values.ContainsKey(gate.A) && values.ContainsKey(gate.B))
            {
                values.Add(gate.X, gate.Op switch
                {
                    "AND" => values[gate.A] & values[gate.B],
                    "OR" => values[gate.A] | values[gate.B],
                    "XOR" => values[gate.A] ^ values[gate.B]
                });
            }
            else
            {
                gates.Enqueue(gate);
            }
        }

        return Int64.Parse(String.Join("", values
            .Where(v => v.Key[0] == 'z')
            .OrderByDescending(v => v.Key)
            .Select(v => v.Value ? "1" : "0")), NumberStyles.BinaryNumber);

        // return values
        //     .Where(v => v.Key[0] == 'z')
        //     .OrderBy(v => v.Key)
        //     .Select((v, i) => v.Value ? 1L << i : 0L)
        //     .Sum();
    }

    struct Gate(string a, string b, string op, string x)
    {
        public string A { get; } = a;
        public string B { get; } = b;
        public string X { get; } = x;
        public string Op { get; } = op;
    }
}