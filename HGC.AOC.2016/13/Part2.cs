﻿using System.Collections.Immutable;
using System.Drawing;
using System.Numerics;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2016._13;

public class Part2 : ISolution
{
    public object? Answer()
    {
        const uint input = 1362;
        var target = new Point(31, 39);

        bool IsWall(uint x, uint y)
        {
            return BitOperations.PopCount(x * x + 3 * x + 2 * x * y + y + y * y + input) % 2 == 1; 
        }

        var start = new Point(1, 1);
        
        var openSet = new PriorityQueue<Point, int>();
        var cameFrom = new Dictionary<Point, Point>();

        openSet.Enqueue(start, Int32.MaxValue);
        
        var gScore = new Dictionary<Point, int>();
        gScore[start] = 0;

        var reachable = new HashSet<Point>();
        reachable.Add(start);

        int H(Point point)
        {
            return Math.Abs(point.X - target.X) + Math.Abs(point.Y - target.Y);
        }

        var i = 0;
        while (openSet.Count != 0)
        {
            var current = openSet.Dequeue();

            foreach (var neighbour in Neighbours(current)
                         .Where(n => !IsWall((uint) n.X, (uint) n.Y)))
            {
                var tentativeGScore = gScore[current] + 1;
                if (tentativeGScore < gScore.GetValueOrDefault(neighbour, Int32.MaxValue) && 
                    tentativeGScore <= 50)
                {
                    reachable.Add(neighbour);
                    cameFrom[neighbour] = current;
                    gScore[neighbour] = tentativeGScore;
                    openSet.Enqueue(neighbour, tentativeGScore + H(neighbour));
                }
            }
        }

        return reachable.Count;
    }

    IEnumerable<Point> Neighbours(Point loc)
    {
        if (loc.X > 0) yield return loc with { X = loc.X - 1 };
        yield return loc with { X = loc.X + 1 };
        if (loc.Y > 0) yield return loc with { Y = loc.Y - 1 };
        yield return loc with { Y = loc.Y + 1 };
    }
}