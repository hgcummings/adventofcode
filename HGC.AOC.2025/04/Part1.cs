using System.Drawing;
using System.Text;
using HGC.AOC.Common;

namespace HGC.AOC._2025._04;

public class Part1 : ISolution
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
        
        var count = 0;

        for (var y = 0; y < height; ++y)
        {
            for (var x = 0; x < width; ++x)
            {
                if (map[y][x] == '@')
                {
                    var adjacentRolls =
                        Neighbours(new Point(x, y)).Count(n => map[n.Y][n.X] == '@');
                    if (adjacentRolls < 4)
                    {
                        count++;
                    }
                }
            }
        }

        return count;
    }
}