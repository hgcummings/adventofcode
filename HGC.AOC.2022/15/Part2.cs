using System.Drawing;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2022._15;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var min = 0;
        var max = 4000000;
        var input = this.ReadInputLines("input.txt");
        
        var sensorRegex = new Regex("Sensor at x=(?'SensorX'[0-9\\-]+), y=(?'SensorY'[0-9\\-]+): closest beacon is at x=(?'BeaconX'[0-9\\-]+), y=(?'BeaconY'[0-9\\-]+)");

        var sensors = input
            .Select(line => sensorRegex.Match(line).Parse<SensorData>())
            .OrderByDescending(s => s.BeaconDistance)
            .ToList();

        for (var y = min; y <= max; ++y)
        {
            if (y % 100000 == 0)
            {
                Console.WriteLine(y);
            }

            var ranges = new List<Tuple<int, int>>();
            foreach (var sensor in sensors)
            {
                var rowDist = Math.Abs(y - sensor.SensorY);
                if (rowDist <= sensor.BeaconDistance)
                {
                    var newRange = new Tuple<int, int>(
                        sensor.SensorX - (sensor.BeaconDistance - rowDist),
                        sensor.SensorX + (sensor.BeaconDistance - rowDist));

                    var newRanges = new List<Tuple<int, int>>();
                    foreach (var range in ranges)
                    {
                        if (range.Item2 >= newRange.Item1 && range.Item1 <= newRange.Item2)
                        {
                            newRange = new Tuple<int, int>(
                                Math.Min(range.Item1, newRange.Item1),
                                Math.Max(range.Item2, newRange.Item2));
                        }
                        else
                        {
                            newRanges.Add(range);
                        }
                    }

                    newRanges.Add(newRange);
                    ranges = newRanges.Distinct().ToList();
                }
            }

            if (ranges.Count > 1)
            {
                Console.Write(y + ": ");
                foreach (var range in ranges.OrderBy(range => range.Item1))
                {
                    Console.Write(range);
                }
                Console.WriteLine();
                return ((ranges.OrderBy(range => range.Item1).First().Item2 + 1) * (long) 4000000) + y;
            }
        }

        return null;
    }

    private class SensorData
    {
        
        public int SensorX { get; set; }
        public int SensorY { get; set; }
        public int BeaconX { get; set; }
        public int BeaconY { get; set; }

        public int BeaconDistance => Math.Abs(BeaconX - SensorX) + Math.Abs(BeaconY - SensorY);
    }
}