using System.Collections.Concurrent;
using System.Drawing;
using System.Text;
using HGC.AOC.Common;

namespace HGC.AOC._2022._25;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");
        long total = 0;

        foreach (var line in input)
        {
            long value = 0;
            for (var i = 0; i < line.Length; ++i)
            {
                var power = (long) Math.Pow(5, i);
                var digit = line[line.Length - i - 1];

                value += power * (digit switch
                {
                    '=' => -2,
                    '-' => -1,
                    '0' => 0,
                    '1' => 1,
                    '2' => 2
                });
            }

            total += value;
        }
        
        Console.WriteLine(total);
        var builder = new StringBuilder();

        for (var i = (long) Math.Log(total, 5) + 1; i >= 0; --i)
        {
            var col = (long)Math.Pow(5, i);
            var digit = Enumerable.Range(0, 5)
                .Select(d => (d - 2, (d - 2) * col))
                .MinBy(x => Math.Abs(x.Item2 - total));

            total -= digit.Item2;
            builder.Append(digit.Item1 switch
            {
                -2 => '=',
                -1 => '-',
                0 => '0',
                1 => '1',
                2 => '2'
            });
        }

        var result = builder.ToString().TrimStart('0');

        return result;
    }
}