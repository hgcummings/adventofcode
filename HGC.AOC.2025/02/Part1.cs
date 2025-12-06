using HGC.AOC.Common;

namespace HGC.AOC._2025._02;

public class Part1 : ISolution
{
    public object? Answer()
    {
        long max = 0;
        
        var ranges = this.ReadInput().Split(",")
            .Select(str =>
            {
                var range = str.Split("-").Select(Int64.Parse).ToList();
                max = Math.Max(max, range[1] + 1);
                return range;
            }).ToList();

        long total = 0;

        for (long i = 1; i * i * 10 < max; i *= 10)
        {
            var unit = (i * 10) + 1;
            foreach (var range in ranges)
            {
                if (i * i * 10 > range[1] || i * i * 100 < range[0])
                {
                    continue;
                }

                var a = Math.Max((range[0] + unit - 1) / unit, i);
                var b = Math.Min((range[1] / unit), i * 10 - 1);
                
                var sum = a == b ? a : ((b * b) + b) / 2 - ((a * a) - a) / 2;
                total += sum * unit;
            }
        }

        return total;
    }
}