using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2024._06;

public class Part2 : ISolution
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

        bool InBounds(Point pos)
        {
            return pos.X >= 0 && pos.Y >= 0 && pos.X < input[0].Length && pos.Y < input.Count;
        }

        bool EntersLoop((Point pos, int dir) current)
        {
            var visited = new HashSet<(Point pos, int dir)>();
            while (InBounds(current.pos))
            {
                if (!visited.Add(current))
                {
                    return true;
                }
                var nextPos = current.dir switch
                {
                    0 => current.pos with { Y = current.pos.Y - 1 },
                    1 => current.pos with { X = current.pos.X + 1 },
                    2 => current.pos with { Y = current.pos.Y + 1 },
                    3 => current.pos with { X = current.pos.X - 1 },
                };
                if (obstacles.Contains(nextPos))
                {
                    current.dir = (current.dir + 1) % 4;
                }
                else
                {
                    current.pos = nextPos;
                }
            }

            return false;
        }

        var count = 0;
        for (var y = 0; y < input.Count; ++y)
        {
            for (var x = 0; x < input[0].Length; ++x)
            {
                var point = new Point(x, y);
                if (point == guard.pos) continue;
                if (!obstacles.Add(point)) continue;
                if (EntersLoop(guard))
                {
                    ++count;
                }

                obstacles.Remove(point);
            }
        }

        return count;
    }
}