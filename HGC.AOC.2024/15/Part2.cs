using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2024._15;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt").ToList();

        var walls = new HashSet<Point>();
        var boxLs = new HashSet<Point>();
        var boxRs = new HashSet<Point>();
        var robot = new Point();
        var moves = string.Empty;
        var inMap = true;

        var width = input[0].Length * 2;
        var height = input.IndexOf(String.Empty);

        for (var y = 0; y < input.Count; ++y)
        {
            if (inMap)
            {
                if (input[y].Trim() == String.Empty)
                {
                    inMap = false;
                    continue;
                }

                for (var x = 0; x < input[y].Length; ++x)
                {
                    var c = input[y][x];
                    if (c == '#')
                    {
                        walls.Add(new Point(x * 2, y));
                        walls.Add(new Point(x * 2 + 1, y));
                    }
                    else if (c == 'O')
                    {
                        boxLs.Add(new Point(x * 2, y));
                        boxRs.Add(new Point(x * 2 + 1, y));
                    }
                    else if (c == '@')
                    {
                        robot = new Point(x * 2, y);
                    }
                }   
            }
            else
            {
                moves += input[y];
            }
        }

        Console.WriteLine("Initial state:");
        DrawMap(width, height, boxLs, boxRs, walls, robot);
        
        foreach (var m in moves)
        {
            var dir = Direction(m);

            var targets = new HashSet<Point> { new(robot.X + dir.X, robot.Y + dir.Y)};
            var boxLsToPush = new List<Point>();
            var boxRsToPush = new List<Point>();

            while (boxLs.Intersect(targets).Any() || boxRs.Intersect(targets).Any())
            {
                var newTargets = new HashSet<Point>();
                foreach (var target in targets)
                {
                    if (boxLs.Contains(target))
                    {
                        boxLsToPush.Add(target);
                        boxRsToPush.Add(target with {X = target.X + 1});
                        newTargets.Add(new Point(target.X + dir.X, target.Y + dir.Y));
                        newTargets.Add(new Point(target.X + 1 + dir.X, target.Y + dir.Y));
                    }
            
                    if (boxRs.Contains(target))
                    {
                        boxRsToPush.Add(target);
                        boxLsToPush.Add(target with {X = target.X - 1});
                        newTargets.Add(new Point(target.X + dir.X, target.Y + dir.Y));
                        newTargets.Add(new Point(target.X - 1 + dir.X, target.Y + dir.Y));
                    }
                }

                targets = newTargets.Where(t => !targets.Contains(t)).ToHashSet();
            }

            var newBoxLs = boxLsToPush.Select(b => new Point(b.X + dir.X, b.Y + dir.Y)).ToHashSet();
            var newBoxRs = boxRsToPush.Select(b => new Point(b.X + dir.X, b.Y + dir.Y)).ToHashSet();
            var newRobot = new Point(robot.X + dir.X, robot.Y + dir.Y);
            
            if (!walls.Contains(newRobot) && 
                !walls.Intersect(newBoxLs).Any() && 
                !walls.Intersect(newBoxRs).Any())
            {
                foreach (var b in boxLsToPush)
                {
                    boxLs.Remove(b);
                }

                foreach (var b in boxRsToPush)
                {
                    boxRs.Remove(b);
                }
                
                foreach (var b in newBoxLs) boxLs.Add(b);
                foreach (var b in newBoxRs) boxRs.Add(b);

                robot = newRobot;
            }
            
            Console.WriteLine();
            Console.WriteLine($"Move {m}:");
            DrawMap(width, height, boxLs, boxRs, walls, robot);
        }

        return boxLs.Sum(b => b.X + 100 * b.Y);
    }

    private void DrawMap(int width, int height, ISet<Point> boxLs, ISet<Point> boxRs,
        ISet<Point> walls, Point robot)
    {
        return;
        for (var y = 0; y < height; ++y)
        {
            var error = -1;
            for (var x = 0; x < width; ++x)
            {
                var objects = 0;
                var p = new Point(x, y);
                if (robot == p)
                {
                    objects++;
                    Console.Write("@");
                }

                if (walls.Contains(p))
                {
                    objects++;
                    Console.Write("#");
                }
                
                if (boxLs.Contains(p))
                {
                    objects++;
                    Console.Write("[");
                }
                
                if (boxRs.Contains(p))
                {
                    objects++;
                    Console.Write("]");
                }

                if (objects == 0)
                {
                    Console.Write(".");
                }
                else if (objects > 1 && error == -1)
                {
                    error = x;
                }
            }
            Console.WriteLine();
            if (error != -1)
            {
                for (var i = 0; i < error; ++i)
                {
                    Console.Write(' ');
                }
                Console.WriteLine("^ Error!");
                throw new InvalidOperationException();
            }
        }
    }

    private Point Direction(char c)
    {
        return c switch
        {
            '^' => new Point(0, -1),
            '>' => new Point(1, 0),
            'v' => new Point(0, 1),
            '<' => new Point(-1, 0)
        };
    }
}