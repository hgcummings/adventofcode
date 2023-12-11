using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2023._11;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var map = new List<String>();
        List<int> emptyCols = null;
        foreach (var line in input)
        {
            if (emptyCols == null)
            {
                emptyCols = Enumerable.Range(0, line.Length).ToList();
            }
            
            map.Add(line);
            if (line.All(c => c == '.'))
            {
                map.Add(line);
            }

            for (var y = 0; y < line.Length; ++y)
            {
                if (line[y] != '.')
                {
                    emptyCols.Remove(y);
                }
            }
        }


        var count = 0;
        foreach (var i in emptyCols)
        {
            for (var x = 0; x < map.Count; ++x)
            {
                map[x] = map[x].Insert(i + count, ".");
            }

            ++count;
        }

        var galaxies = new List<Point>();
        for (var x = 0; x < map.Count; ++x)
        {
            var row = map[x];
            for (var y = 0; y < row.Length; ++y)
            {
                if (row[y] == '#')
                {
                    galaxies.Add(new Point(x,y));
                }
            }
        }

        var totalDistance = 0;

        for (var i = 0; i < galaxies.Count; ++i)
        {
            for (var j = i + 1; j < galaxies.Count; ++j)
            {
                var a = galaxies[i];
                var b = galaxies[j];

                var distance = Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
                totalDistance += distance;
            }
        }

        return totalDistance;
    }
}