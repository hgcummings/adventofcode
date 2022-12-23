using System.Collections.Concurrent;
using System.Drawing;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2022._23;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt").ToList();

        var elves = new HashSet<Point>();

        for (var y = 0; y < input.Count; ++y)
        {
            for (var x = 0; x < input[y].Length; ++x)
            {
                if (input[y][x] == '#')
                {
                    elves.Add(new Point(x, y));
                }
            }
        }


        var moving = true;
        var directions = new List<Tuple<Point, Func<Point, Point, bool>>>
        {
            new(new Point(0,-1), (elf, n) => n.Y == elf.Y - 1),
            new(new Point(0,1), (elf, n) => n.Y == elf.Y + 1),
            new(new Point(-1,0), (elf, n) => n.X == elf.X - 1),
            new(new Point(1,0), (elf, n) => n.X == elf.X + 1)
        };
        
        for(var i = 1; moving; ++i)
        {
            var moves = new ConcurrentDictionary<Point, List<Point>>();
            var movingCount = 0;
            moving = false;

            foreach (var elf in elves)
            {
                var neighbours = elves
                    .Where(e => e.X >= elf.X - 1 && e.X <= elf.X + 1 &&
                                e.Y >= elf.Y - 1 && e.Y <= elf.Y + 1 &&
                                e != elf)
                    .ToList();

                if (neighbours.Count == 0)
                {
                    moves.TryAdd(elf, new List<Point> {elf});
                }
                else
                {
                    moving = true;
                    movingCount++;
                    var foundMove = false;
                    foreach (var direction in directions)
                    {
                        if (!neighbours.Any(n => direction.Item2(elf, n)))
                        {
                            var target = new Point { X = elf.X + direction.Item1.X, Y = elf.Y + direction.Item1.Y };
                            moves.GetOrAdd(target, _ => new List<Point>()).Add(elf);
                            foundMove = true;
                            break;
                        }
                    }
                    
                    if (!foundMove)
                    {
                        moves.TryAdd(elf, new List<Point> {elf});
                    }
                }
            }

            var newElves = new HashSet<Point>();
            foreach (var pair in moves)
            {
                if (pair.Value.Count == 1)
                {
                    if (!newElves.Add(pair.Key))
                    {
                        throw new Exception("Elf already present");
                    }
                }
                else
                {
                    foreach (var elf in pair.Value)
                    {
                        if (!newElves.Add(elf))
                        {
                            throw new Exception("Elf already present");
                        }
                    }
                }
            }

            elves = newElves;

            var firstDirection = directions[0];
            directions.RemoveAt(0);
            directions.Add(firstDirection);
            
            Console.WriteLine($"Round {i}: {movingCount} / {elves.Count} moved");
        
            // for (var y = elves.Select(elf => elf.Y).Min(); y <= elves.Select(elf => elf.Y).Max(); ++y)
            // {
            //     for (var x = elves.Select(elf => elf.X).Min(); x <= elves.Select(elf => elf.X).Max(); ++x)
            //     {
            //         Console.Write(elves.Contains(new Point(x, y)) ? '#' : '.');
            //     }
            //     Console.WriteLine();
            // }
            // Console.WriteLine();
        }

        var count = 0;
        for (var x = elves.Select(elf => elf.X).Min(); x <= elves.Select(elf => elf.X).Max(); ++x)
        {
            for (var y = elves.Select(elf => elf.Y).Min(); y <= elves.Select(elf => elf.Y).Max(); ++y)
            {
                if (!elves.Contains(new Point(x, y)))
                {
                    count++;
                }
            }
        }

        return count;
    }
}