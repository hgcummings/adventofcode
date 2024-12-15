using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2024._15;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt").ToList();

        var walls = new HashSet<Point>();
        var boxes = new HashSet<Point>();
        var robot = new Point();
        var moves = string.Empty;
        var inMap = true;

        var width = input[0].Length;
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
                        walls.Add(new Point(x, y));
                    }
                    else if (c == 'O')
                    {
                        boxes.Add(new Point(x, y));
                    }
                    else if (c == '@')
                    {
                        robot = new Point(x, y);
                    }
                }   
            }
            else
            {
                moves += input[y];
            }
        }

        Console.WriteLine("Initial state:");
        DrawMap(width, height, boxes, walls, robot);

        foreach (var m in moves)
        {
            var dir = Direction(m);

            var target = new Point(robot.X + dir.X, robot.Y + dir.Y);
            var boxesToPush = new List<Point>();
            
            while (boxes.Contains(target))
            {
                boxesToPush.Add(target);
                target = new Point(target.X + dir.X, target.Y + dir.Y);
            }

            if (!walls.Contains(target))
            {
                if (boxesToPush.Count > 0)
                {
                    boxes.Remove(boxesToPush[0]);
                    boxes.Add(target);   
                }

                robot = new Point(robot.X + dir.X, robot.Y + dir.Y);
            }
            
            Console.WriteLine();
            Console.WriteLine($"Move {m}:");
            DrawMap(width, height, boxes, walls, robot);
        }

        return boxes.Sum(b => b.X + 100 * b.Y);
    }

    private void DrawMap(int width, int height, ISet<Point> boxes, ISet<Point> walls, Point robot)
    {
        return;
        for (var y = 0; y < height; ++y)
        {
            for (var x = 0; x < width; ++x)
            {
                var objects = 0;
                var p = new Point(x, y);
                if (robot == p)
                {
                    objects++;
                    Console.Write('@');
                }

                if (walls.Contains(p))
                {
                    objects++;
                    Console.Write('#');
                }
                
                if (boxes.Contains(p))
                {
                    objects++;
                    Console.Write('O');
                }

                if (objects == 0)
                {
                    Console.Write('.');
                }
                else if (objects > 1)
                {
                    throw new InvalidOperationException("Overlapping objects");
                }
            }
            Console.WriteLine();
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