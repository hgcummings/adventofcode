using System.Collections.Concurrent;
using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2024._22;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var monkeys = this.ReadInputLines("input.txt").Select(Int64.Parse);

        var values = new Tally<(int, int, int, int)>();

        Parallel.ForEach(monkeys, monkey =>
        {
            foreach (var entry in SequenceValues(monkey, 2000))
            {
                values.Increase(entry.Key, entry.Value);
            }
        });

        Console.WriteLine(values.Highest);
        return values.HighestValue;
    }

    public Dictionary<(int,int,int,int), int> SequenceValues(long num, int steps)
    {
        var seq = new List<int>(5);
        var values = new Dictionary<(int, int, int, int), int>();

        var prev = 0;
        for (var i = 0; i < steps; ++i)
        {
            num = (num ^ (num * 64)) % 16777216;
            num = (num ^ (num / 32)) % 16777216;
            num = (num ^ (num * 2048)) % 16777216;

            var next = (int) num % 10;
            
            if (i > 1)
            {
                seq.Add(next - prev);
                if (i > 5)
                {
                    seq.RemoveAt(0);
                    values.TryAdd((seq[0], seq[1], seq[2], seq[3]), next);
                }
            }

            prev = next;
        }

        return values;
    }
}