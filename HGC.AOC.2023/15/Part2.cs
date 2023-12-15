using System.Runtime.InteropServices;
using HGC.AOC.Common;

namespace HGC.AOC._2023._15;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var boxes = ArrayHelpers.InitArray(256, _ => new List<Lens>());
        var steps = this.ReadInputLines("input.txt").First().Split(",");

        foreach (var step in steps)
        {
            if (step.EndsWith('-'))
            {
                var lens = step[..^1];
                var box = Hash(lens);
                var idx = boxes[box].FindIndex(l => l.Label == lens);
                if (idx != -1)
                {
                    boxes[box].RemoveAt(idx);
                }
            }
            else
            {
                var parts = step.Split('=');
                var lens = parts[0];
                var fLen = Int32.Parse(parts[1]);
                var box = Hash(lens);
                var idx = boxes[box].FindIndex(l => l.Label == lens);
                if (idx == -1)
                {
                    boxes[box].Add(new Lens(lens, fLen));
                }
                else
                {
                    boxes[box].RemoveAt(idx);
                    boxes[box].Insert(idx, new Lens(lens, fLen));
                }
            }
        }

        return boxes
            .Select((box, i) => box
                .Select((l, j) => (i + 1) * (j + 1) * l.FocalLen)
                .Sum())
            .Sum();
    }

    struct Lens
    {
        public Lens(string label, int focalLen)
        {
            Label = label;
            FocalLen = focalLen;
        }

        public string Label { get; }
        public int FocalLen { get; }
    }

    public int Hash(string input)
    {
        byte curr = 0;
        foreach (int ch in input)
        {
            curr += (byte) ch;
            curr *= 17;
        }

        return curr;
    }
}