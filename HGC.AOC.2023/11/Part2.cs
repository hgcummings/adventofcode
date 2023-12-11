using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2023._11;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var map = this.ReadInputLines("input.txt").ToArray();

        List<int> emptyCols = null;
        List<int> emptyRows = new List<int>();
        var rowIndex = 0;
        foreach (var line in map)
        {
            if (emptyCols == null)
            {
                emptyCols = Enumerable.Range(0, line.Length).ToList();
            }
            
            if (line.All(c => c == '.'))
            {
                emptyRows.Add(rowIndex);
            }

            for (var y = 0; y < line.Length; ++y)
            {
                if (line[y] != '.')
                {
                    emptyCols.Remove(y);
                }
            }

            rowIndex++;
        }

        var galaxies = new List<Point>();
        for (var x = 0; x < map.Length; ++x)
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

        long totalDistance = 0;

        for (var i = 0; i < galaxies.Count; ++i)
        {
            for (var j = i + 1; j < galaxies.Count; ++j)
            {
                var a = galaxies[i];
                var b = galaxies[j];

                long distance = Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y)
                    + 999999 * emptyRows.Count(c => c < Math.Max(a.X, b.X) && c > Math.Min(a.X, b.X))
                    + 999999 * emptyCols.Count(c => c < Math.Max(a.Y, b.Y) && c > Math.Min(a.Y, b.Y));
                
                totalDistance += distance;
            }
        }

        return totalDistance;
    }
}