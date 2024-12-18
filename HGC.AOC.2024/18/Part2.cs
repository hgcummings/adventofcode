using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2024._18;

public class Part2 : ISolution
{
    private const int MaxX = 70;
    private const int MaxY = 70;
    
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var bytes = input.Select(line =>
        {
            var parts = line.Split(',');
            var x = Int32.Parse(parts[0]);
            var y = Int32.Parse(parts[1]);
            return new Point(x, y);
        }).ToList();
        
        for (var i = bytes.Count; ; --i)
        {
            if (MinDistance(bytes[..i]).HasValue)
            {
                return bytes[i].ToString();
            }
        }
    }

    int? MinDistance(List<Point> bytes)
    {
        var start = new Point(0, 0);
        
        var distances = new Dictionary<Point, int>();
        distances[start] = 0;

        var queue = new PriorityQueue<Point, int>();
        queue.Enqueue(start, 0);
        
        while (queue.Count > 0)
        {
            var loc = queue.Dequeue();
            
            if (loc.X == MaxX && loc.Y == MaxY)
            {
                return distances[loc];
            }

            foreach (var neighbour in Neighbours(loc).Where(n => !bytes.Contains(n)))
            {
                var distanceViaLoc = distances[loc] + 1;
                if (distanceViaLoc < distances.GetValueOrDefault(neighbour, Int32.MaxValue))
                {
                    distances[neighbour] = distanceViaLoc;
                    queue.Enqueue(neighbour, distanceViaLoc);
                }
            }
        }

        return null;
    }

    IEnumerable<Point> Neighbours(Point loc)
    {
        if (loc.X > 0) yield return loc with { X = loc.X - 1 };
        if (loc.X < MaxX) yield return loc with { X = loc.X + 1 };
        if (loc.Y > 0) yield return loc with { Y = loc.Y - 1 };
        if (loc.Y < MaxY) yield return loc with { Y = loc.Y + 1 };
    }
}