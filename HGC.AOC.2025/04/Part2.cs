using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2025._04;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var map = this.ReadInputLines().ToList();

        var width = map[0].Length;
        var height = map.Count;
        
        IEnumerable<Point> Neighbours(Point loc)
        {
            if (loc.X > 0)
            {
                yield return loc with { X = loc.X - 1 };
                if (loc.Y > 0) yield return loc with { X = loc.X - 1, Y = loc.Y - 1 };
                if (loc.Y < height - 1) yield return loc with { X = loc.X - 1, Y = loc.Y + 1 };
            }

            if (loc.X < width - 1)
            {
                yield return loc with { X = loc.X + 1 };
                if (loc.Y > 0) yield return loc with { X = loc.X + 1, Y = loc.Y - 1 };
                if (loc.Y < height - 1) yield return loc with { X = loc.X + 1, Y = loc.Y + 1 };
            }
            
            if (loc.Y > 0) yield return loc with { Y = loc.Y - 1 };
            if (loc.Y < height - 1) yield return loc with { Y = loc.Y + 1 };
        }

        var rollsToRemove = new List<Point>();
        var count = 0;

        do
        {
            foreach (var rollToRemove in rollsToRemove)
            {
                var row = map[rollToRemove.Y];
                map[rollToRemove.Y] = 
                    string.Concat(
                        row.AsSpan(0,rollToRemove.X),
                        ".",
                        row.AsSpan(rollToRemove.X + 1));
                ++count;
            }
            rollsToRemove = new List<Point>();
            
            for (var y = 0; y < height; ++y)
            {
                for (var x = 0; x < width; ++x)
                {
                    if (map[y][x] == '@')
                    {
                        var roll = new Point(x, y);
                        var adjacentRolls =
                            Neighbours(roll).Count(n => map[n.Y][n.X] == '@');
                        if (adjacentRolls < 4)
                        {
                            rollsToRemove.Add(roll);
                        }
                    }
                }
            }
        } while (rollsToRemove.Count > 0);
        
        return count;
    }
}