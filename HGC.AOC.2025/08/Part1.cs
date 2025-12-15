using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2025._08;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var boxes = this.ReadInputLines("input.txt")
            .Select(line =>
            {
                var components = line.Split(',').Select(Int32.Parse).ToList();
                return new Point3D(components[0], components[1], components[2]);
            }).ToList();

        var pairs = new PriorityQueue<Tuple<Point3D, Point3D>, long>();

        for (var i = 0; i < boxes.Count; ++i)
        {
            for (var j = i + 1; j < boxes.Count; ++j)
            {
                var sqDist =
                    (long) (boxes[j].X - boxes[i].X) * (boxes[j].X - boxes[i].X) +
                    (long) (boxes[j].Y - boxes[i].Y) * (boxes[j].Y - boxes[i].Y) +
                    (long) (boxes[j].Z - boxes[i].Z) * (boxes[j].Z - boxes[i].Z);
                
                pairs.Enqueue(new Tuple<Point3D, Point3D>(boxes[i], boxes[j]), sqDist);
            }
        }

        var circuits = new List<HashSet<Point3D>>();
        for (var i = 0; i < 1000; ++i)
        {
            var (a, b) = pairs.Dequeue();

            var aCircuit = circuits.SingleOrDefault(c => c.Contains(a));
            var bCircuit = circuits.SingleOrDefault(c => c.Contains(b));

            if (aCircuit != null && bCircuit != null)
            {
                if (aCircuit == bCircuit)
                {
                    continue;
                }
                
                circuits.Remove(bCircuit);
                foreach (var point in bCircuit)
                {
                    aCircuit.Add(point);
                }
            }
            else if (aCircuit != null)
            {
                aCircuit.Add(b);
            }
            else if (bCircuit != null)
            {
                bCircuit.Add(a);
            }
            else
            {
                circuits.Add([a, b]);
            }
        }

        return circuits.Select(c => c.Count).OrderDescending().Take(3).Product();
    }
}