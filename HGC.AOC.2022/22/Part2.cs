using System.Drawing;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2022._22;

public class Part2 : ISolution
{
    private const char Open = '.';
    private const char Wall = '#';
    private const char Gap = ' ';
    
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var map = new List<string>();
        var commands = new List<ICommand>();

        var reachedInstructions = false;
        foreach (var line in input)
        {
            if (reachedInstructions)
            {
                var buffer = "";

                void ConsumeBuffer()
                {
                    if (buffer.Length > 0)
                    {
                        commands.Add(new Walk(Int32.Parse(buffer)));
                        buffer = "";
                    }
                }

                foreach (var symbol in line)
                {
                    switch (symbol)
                    {
                        case 'L':
                            ConsumeBuffer();
                            commands.Add(new Left());
                            break;
                        case 'R':
                            ConsumeBuffer();
                            commands.Add(new Right());
                            break;
                        default:
                            buffer += symbol;
                            break;
                    }
                        
                }
                ConsumeBuffer();
            }
            else if (line.Trim() == String.Empty)
            {
                reachedInstructions = true;
            }
            else
            {
                map.Add(line);
            }
        }

        var x = map[0].IndexOf(Open);
        var y = 0;
        var f = 0;

        foreach (var command in commands)
        {
            (x, y, f) = command.Apply(map, x, y, f);
        }
        
        return (1000 * (y + 1)) + (4 * (x + 1)) + f;
    }

    private interface ICommand
    {
        (int, int, int) Apply(List<string> map, int x, int y, int f);
    }

    private class Walk : ICommand
    {
        private readonly int _distance;

        public Walk(int distance)
        {
            _distance = distance;
        }

        private (int, int, int) Proceed(int x, int y, int f)
        {
            if (y == -1 && f == 3)
            {
                if (x is >= 50 and < 100) //D1
                {
                    return (0, 150 + (x - 50), 0);
                }

                if (x is >= 100 and < 150) //C1
                {
                    return (x - 100, 199, 3);
                }
            }

            if (y is >= 0 and < 50)
            {
                if (x == 49 && f == 2) //E1
                {
                    return (0, 100 + (49 - y), 0);
                }
                
                if (x == 150 && f == 0) //B1
                {
                    return (99, 100 + (49 - y), 2);
                }
            }

            if (y == 50 && x is >= 100 and < 150 && f == 1) //A1
            {
                return (99, 50 + (x - 100), 2);
            }
            
            if (y == 99 && x is >= 0 and < 50 && f == 3) //F2
            {
                return (50, x + 50, 0);
            }

            if (y is >= 50 and < 100)
            {
                if (x == 49 && f == 2) //F1
                {
                    return (y - 50, 100, 1);
                }
                
                if (x == 100 && f == 0) //A2
                {
                    return (100 + y - 50, 49, 3);
                }
            }
            
            if (y is >= 100 and < 150)
            {
                if (x == -1 && f == 2) //E2
                {
                    return (50, 149 - y, 0);
                }

                if (x == 100 && f == 0) //B2
                {
                    return (149, 149 - y, 2);
                }
            }

            if (y == 150 && x is >= 50 and < 100 && f == 1) //G2
            {
                return (49, 150 + (x - 50), 2);
            }

            if (y is >= 150 and < 200)
            {
                if (x == -1 && f == 2) //D2
                {
                    return (50 + (y - 150), 0, 1);
                }

                if (x == 50 && f == 0) //G1
                {
                    return (50 + y - 150, 149, 3);
                }
            }

            if (y == 200 && f == 1) //C2
            {
                return (x + 100, 0, 1);
            }

            throw new Exception($"Unrecognized edge x: {x}, y: {y}, f: {f}");
        }

        private Point Direction(int facing)
        {
            return facing switch
            {
                0 => new Point(1, 0),
                1 => new Point(0, 1),
                2 => new Point(-1, 0),
                3 => new Point(0, -1)
            };
        }
        
        public (int, int, int) Apply(List<string> map, int x, int y, int f)
        {
            var lastOpenX = x;
            var lastOpenY = y;
            var lastOpenF = f;
            var direction = Direction(f);
            for (var i = 0; i < _distance; ++i)
            {
                x += direction.X;
                y += direction.Y;

                if (x < 0 || y < 0 || y >= map.Count || x >= map[y].Length || map[y][x] == Gap)
                {
                    Console.WriteLine($"Off map at x: {x}, y: {y}, f: {f}");
                    (x, y, f) = Proceed(x, y, f);
                    Console.WriteLine($"Proceeded to x: {x}, y: {y}, f: {f}");
                    direction = Direction(f);
                }

                if (map[y][x] == Open)
                {
                    lastOpenX = x;
                    lastOpenY = y;
                    lastOpenF = f;
                }

                if (map[y][x] == Wall)
                {
                    return (lastOpenX, lastOpenY, lastOpenF);
                }
            }

            return (x, y, f);
        }
    }

    private class Left : ICommand
    {
        public (int, int, int) Apply(List<string> map, int x, int y, int f)
        {
            return (x, y, (f + 3) % 4);
        }
    }

    private class Right : ICommand
    {
        public (int, int, int) Apply(List<string> map, int x, int y, int f)
        {
            return (x, y, (f + 1) % 4);
        }
    }
}