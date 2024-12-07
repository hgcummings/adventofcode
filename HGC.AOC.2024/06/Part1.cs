using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2024._06;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt").ToList();

        var items = input
            .SelectMany((line, y) => line.Select((c, x) => (c, new Point(x, y))))
            .Where(i => i.c != '.')
            .ToList();

        var obstacles = items.Where(i => i.c == '#').Select(i => i.Item2).ToHashSet();
        (Point pos, int dir) guard = (items.Single(i => i.c == '^').Item2, 0);

        var visited = new HashSet<Point>();

        bool InBounds(Point pos)
        {
            return pos.X >= 0 && pos.Y >= 0 && pos.X < input[0].Length && pos.Y < input.Count;
        }

        while (InBounds(guard.pos))
        {
            visited.Add(guard.pos);
            var nextPos = guard.dir switch
            {
                0 => guard.pos with { Y = guard.pos.Y - 1 },
                1 => guard.pos with { X = guard.pos.X + 1 },
                2 => guard.pos with { Y = guard.pos.Y + 1 },
                3 => guard.pos with { X = guard.pos.X - 1 },
            };
            if (obstacles.Contains(nextPos))
            {
                guard.dir = (guard.dir + 1) % 4;
            }
            else
            {
                guard.pos = nextPos;
            }
        }

        return visited.Count;
    }
}