using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Xml;
using HGC.AOC.Common;

namespace HGC.AOC._2024._12;

public class Part2 : ISolution
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
        {
            var vl = new HashSet<Point>();
            var hl = new HashSet<Point>();

            foreach (var p in region)
            {
                if (!region.Contains(p with { X = p.X - 1 })) vl.Add(p);
                if (!region.Contains(p with { X = p.X + 1 })) vl.Add(p with { X = p. X + 1});
                if (!region.Contains(p with { Y = p.Y - 1 })) hl.Add(p);
                if (!region.Contains(p with { Y = p.Y + 1 })) hl.Add(p with {Y = p.Y + 1});
            }

            var vCost = 0;
            var side = 0;
            var x = -2;
            var y = -2;
            
            foreach (var seg in vl.OrderBy(v => v.X).ThenBy(v => v.Y))
            {
                if (seg.X != x || seg.Y != y + 1)
                {
                    side = 0;
                    x = seg.X;
                }

                y = seg.Y;
                
                var newSide = region.Contains(seg) ? 1 : -1;
                if (newSide != side)
                {
                    vCost += 1;
                }

                side = newSide;
            }
            
            var hCost = 0;
            side = 0;
            x = -2;
            y = -2;
            foreach (var seg in hl.OrderBy(h => h.Y).ThenBy(h => h.X))
            {
                if (seg.Y != y || seg.X != x + 1)
                {
                    side = 0;
                    y = seg.Y;
                }

                x = seg.X;
                
                var newSide = region.Contains(seg) ? 1 : -1;
                if (newSide != side)
                {
                    hCost += 1;
                }

                side = newSide;
            }

            var cost = (vCost + hCost) * region.Count;
            Console.WriteLine($"{map[region.First().Y][region.First().X]}: ({vCost} + {hCost}) * {region.Count} = {cost}");
            return cost;
        });
    }
}