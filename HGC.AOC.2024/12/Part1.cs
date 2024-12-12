using System.Drawing;
using System.Xml;
using HGC.AOC.Common;

namespace HGC.AOC._2024._12;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var map = this.ReadInputLines("input.txt").ToList();
        
        var regions = new List<HashSet<Point>>();

        for (var y = 0; y < map.Count; ++y)
        {
            for (var x = 0; x < map[y].Length; ++x)
            {
                var seed = new Point(x, y);
                var type = map[y][x];
                if (regions.Any(r => r.Contains(seed)))
                {
                    continue;
                }

                var region = new HashSet<Point>();
                var queue = new Queue<Point>();
                
                queue.Enqueue(seed);

                while (queue.Count > 0)
                {
                    var n = queue.Dequeue();
                    if (map[n.Y][n.X] != type || !region.Add(n)) continue;
                    if (n.X > 0) queue.Enqueue(n with { X = n.X - 1});
                    if (n.X < map[0].Length - 1) queue.Enqueue(n with { X = n.X + 1});
                    if (n.Y > 0) queue.Enqueue(n with { Y = n.Y - 1});
                    if (n.Y < map.Count - 1) queue.Enqueue(n with { Y = n.Y + 1});
                }
                
                regions.Add(region);
            }
        }

        return regions.Sum(region =>
            region.Sum(
                p => 4
                     - (region.Contains(p with { X = p.X - 1 }) ? 1 : 0)
                     - (region.Contains(p with { X = p.X + 1 }) ? 1 : 0)
                     - (region.Contains(p with { Y = p.Y - 1 }) ? 1 : 0)
                     - (region.Contains(p with { Y = p.Y + 1 }) ? 1 : 0)
            ) * region.Count);
    }
}