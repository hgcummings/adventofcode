using System.Drawing;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2022._22;

public class Part1 : ISolution
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

        // var history = new Dictionary<(int, int), int>();
        // history[(x, y)] = f;

        foreach (var command in commands)
        {
            (x, y, f) = command.Apply(map, x, y, f);
            // history[(x, y)] = f;
        }

        // Console.WriteLine();
        // for (var row = 0; row < map.Count; ++row)
        // {
        //     for (var col = 0; col < map[row].Length; ++col)
        //     {
        //         if (history.ContainsKey((col, row)))
        //         {
        //             Console.Write(history[(col, row)] switch
        //             {
        //                 0 => '>',
        //                 1 => 'v',
        //                 2 => '<',
        //                 3 => '^'
        //             });
        //         }
        //         else
        //         {
        //             Console.Write(map[row][col]);
        //         }
        //     }
        //     Console.WriteLine();
        // }

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
        
        public (int, int, int) Apply(List<string> map, int x, int y, int f)
        {
            var mapWidth = map.Select(row => row.Length).Max();
            var mapHeight = map.Count;
            
            var direction = f switch
            {
                0 => new Point(1, 0),
                1 => new Point(0, 1),
                2 => new Point(-1, 0),
                3 => new Point(0, -1)
            };

            var lastOpenX = x;
            var lastOpenY = y;
            for (var i = 0; i < _distance; ++i)
            {
                x = (x + direction.X + mapWidth) % mapWidth;
                y = (y + direction.Y + mapHeight) % mapHeight;

                while (x >= map[y].Length || map[y][x] == Gap)
                {
                    x = (x + direction.X + mapWidth) % mapWidth;
                    y = (y + direction.Y + mapHeight) % mapHeight;
                }

                if (map[y][x] == Open)
                {
                    lastOpenX = x;
                    lastOpenY = y;
                }

                if (map[y][x] == Wall)
                {
                    return (lastOpenX, lastOpenY, f);
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