using System.Collections;
using System.Diagnostics;
using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2023._21;

public class Part2Hard : ISolution
{
    public object? Answer()
    {
        Point? foundStart = null;
        var map = this.ReadInputLines("input.txt").Select(
            (line, y) =>
            {
                if (line.IndexOf('S') != -1)
                {
                    foundStart = new Point(line.IndexOf('S'), y);
                }

                return line.Select(c => c == '#').ToArray();
            }).ToArray();

        const int steps = 5000;

        var distanceFromCentre = new Dictionary<Point, int> { [foundStart!.Value] = 0 };
        ExploreMap(distanceFromCentre, map);
        
        var totalOdd = 0L;
        var totalEven = 0L;

        var corner = distanceFromCentre[new Point(0, 0)] % 2;
        var inner = new int[2];
        inner[corner] = distanceFromCentre.Count(e => e.Value % 2 == corner);
        inner[1 - corner] = distanceFromCentre.Count(e => e.Value % 2 == 1 - corner);

        void ExploreX(int initial, int stride)
        {
            var index = initial - 2 * stride + Math.Sign(stride);
            var offset = initial - stride;
            for (var x = initial;; x += stride)
            {
                var lastEdge = distanceFromCentre
                    .Where(e => e.Key.X == x - stride && e.Key.Y >= 0 && e.Key.Y < map.Length)
                    .ToArray();
                if (lastEdge.Length != 0 && lastEdge.All(e => e.Value > steps))
                {
                    break;
                }

                if (Math.Abs(x - initial) <= 2 * Math.Abs(stride) ||
                    lastEdge.Any(e => e.Value > steps - map.Length - map[0].Length))
                {
                    var frontier = lastEdge
                        .ToDictionary(e => e.Key with { X = index }, e => e.Value + 1);
                    ExploreMap(frontier, map);
                    foreach (var entry in frontier)
                    {
                        distanceFromCentre[entry.Key with { X = entry.Key.X + x - offset }] =
                            entry.Value;
                    }
                }
                else
                {
                    for (var y = 0; y < map.Length; ++y)
                    {
                        var distance =
                            2 * distanceFromCentre[new Point(x - stride, y)]
                            - distanceFromCentre[new Point(x - 2 * stride, y)];

                        totalOdd -= distance % 2;
                        totalEven -= (1 - distance % 2);

                        distanceFromCentre[new Point(x, y)] = distance;
                    }

                    var relCorner = distanceFromCentre[new Point(x - stride, 0)] % 2;
                    totalOdd += inner[relCorner];
                    totalEven += inner[1 - relCorner];
                }
            }
        }
        
        void ExploreY(int initial, int stride)
        {
            var index = initial - 2 * stride + Math.Sign(stride);
            var offset = initial - stride;
            for (var y = initial;; y += stride)
            {
                var lastEdge = distanceFromCentre
                    .Where(e => e.Key.Y == y - stride &&
                                e.Key.X >= 0 && e.Key.X < map[0].Length)
                    .ToArray();
                if (lastEdge.Length != 0 && lastEdge.All(e => e.Value > steps))
                {
                    break;
                }

                if (Math.Abs(y - initial) <= 2 * Math.Abs(stride) ||
                    lastEdge.Any(e => e.Value > steps - map.Length - map[0].Length))
                {
                    var frontier = lastEdge
                        .ToDictionary(e => e.Key with { Y = index }, e => e.Value + 1);
                    ExploreMap(frontier, map);
                    foreach (var entry in frontier)
                    {
                        distanceFromCentre[entry.Key with { Y = entry.Key.Y + y - offset }] =
                            entry.Value;
                    }
                }
                else
                {
                    for (var x = 0; x < map[0].Length; ++x)
                    {
                        var distance =
                            2 * distanceFromCentre[new Point(x, y - stride)]
                            - distanceFromCentre[new Point(x, y - 2 * stride)];

                        totalOdd -= distance % 2;
                        totalEven -= (1 - distance % 2);

                        distanceFromCentre[new Point(x, y)] = distance;
                    }

                    var relCorner = distanceFromCentre[new Point(0, y - stride)] % 2;
                    totalOdd += inner[relCorner];
                    totalEven += inner[1 - relCorner];
                }
            }
        }

        ExploreX(2 * map[0].Length - 1, map[0].Length);
        ExploreX(-map[0].Length, -map[0].Length);
        
        ExploreY(2 * map.Length - 1, map.Length);
        ExploreY(-map.Length, -map.Length);

        var minX = distanceFromCentre.Keys.Min(k => k.X);
        var maxX = distanceFromCentre.Keys.Max(k => k.X);
        FillY(map[0].Length, maxX, 0);
        FillY(map[0].Length, maxX, map.Length - 1);
        
        void FillY(int fromX, int toX, int y)
        {
            var step = Math.Sign(toX - fromX);
            int prev = distanceFromCentre[new Point(fromX - step, y)];
            for (var x = fromX; x != toX + step; x += step)
            {
                var calcD = prev + 1;
                var point = new Point(x, y);
                if (distanceFromCentre.TryGetValue(new Point(x, y), out var d))
                {
                    if (d != calcD)
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    distanceFromCentre[point] = calcD;
                    
                    totalOdd -= calcD % 2;
                    totalEven -= (1 - calcD % 2);
                }

                prev = calcD;
            }
        }
        
        totalOdd += distanceFromCentre.Count(e => e.Value <= steps && e.Value % 2 == 1);
        totalEven += distanceFromCentre.Count(e => e.Value <= steps && e.Value % 2 == 0);
        Console.WriteLine($"{totalOdd}, {totalEven}");

        return steps % 2 == 1 ? totalOdd : totalEven;
    }

    private static void ExploreMap(Dictionary<Point, int> distance, bool[][] 
            map)
    {
        var frontier = distance.Keys.ToHashSet();
        
        while(frontier.Count > 0)
        {
            var newFrontier = new HashSet<Point>();

            foreach (var point in frontier)
            {
                foreach (var n in GetNeighbours(point))
                {
                    var dp = distance[point];

                    if (distance.ContainsKey(n) && distance[n] <= dp)
                    {
                        continue;
                    }
                
                    if (!map[n.Y][n.X])
                    {
                        distance[n] = dp + 1;
                        newFrontier.Add(n);
                    }
                }
            }

            frontier = newFrontier;
        }

        IEnumerable<Point> GetNeighbours(Point point)
        {
            if (point.X > 0)
            {
                yield return point with { X = point.X - 1 };
            }

            if (point.Y > 0)
            {
                yield return point with { Y = point.Y - 1 };
            }

            if (point.X < map[0].Length - 1)
            {
                yield return point with { X = point.X + 1 };
            }

            if (point.Y < map.Length - 1)
            {
                yield return point with { Y = point.Y + 1 };
            }
        }
    }
}