using System.Drawing;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2022._15;

public class Part1 : ISolution
{
    public object? Answer()
    {
        // const int checkRow = 10;
        // var input = this.ReadInputLines("example.txt");

        const int checkRow = 2000000;
        var input = this.ReadInputLines("input.txt");
        
        var sensorRegex = new Regex("Sensor at x=(?'SensorX'[0-9\\-]+), y=(?'SensorY'[0-9\\-]+): closest beacon is at x=(?'BeaconX'[0-9\\-]+), y=(?'BeaconY'[0-9\\-]+)");

        var sensors = input.Select(line => sensorRegex.Match(line).Parse<SensorData>()).ToList();

        var maxRange = sensors.Select(s => s.BeaconDistance).Max();
        var maxX = Math.Max(sensors.Select(s => s.SensorX).Max(), sensors.Select(s => s.BeaconX).Max());
        var maxY = Math.Max(sensors.Select(s => s.SensorY).Max(), sensors.Select(s => s.BeaconY).Max());

        int count = 0;
        for (var x = -maxRange; x <= maxX + maxRange; ++x)
        {
            if (x % 100000 == 0)
            {
                Console.WriteLine(x);
            }
            // for (var y = -maxRange; y <= maxY + maxRange; ++y)
            // {
                var eliminated = CheckEliminated(x, checkRow);
                if (eliminated)// && y == checkRow)
                {
                    count++;
                }
            // }
        }

        bool CheckEliminated(int x, int y)
        {
            foreach (var sensor in sensors)
            {
                if (sensor.BeaconX == x && sensor.BeaconY == y)
                {
                    return false;
                }

                if (Math.Abs(x - sensor.SensorX) + Math.Abs(y - sensor.SensorY) <= sensor.BeaconDistance)
                {
                    return true;
                }
            }

            return false;
        }

        return count;
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