using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2022._17;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var blasts = this.ReadInput("input.txt");

        var rocks = new bool[5][][];
        rocks[0] = new bool[1][];
        rocks[0][0] = new[] { true, true, true, true };
        
        rocks[1] = new bool[3][];
        rocks[1][0] = new[] { false, true, false };
        rocks[1][1] = new[] { true, true, true };
        rocks[1][2] = new[] { false, true, false };

        rocks[2] = new bool[3][];
        rocks[2][0] = new[] { true, true, true };
        rocks[2][1] = new[] { false, false, true };
        rocks[2][2] = new[] { false, false, true };

        rocks[3] = new bool[4][];
        rocks[3][0] = new[] { true };
        rocks[3][1] = new[] { true };
        rocks[3][2] = new[] { true };
        rocks[3][3] = new[] { true };

        rocks[4] = new bool[2][];
        rocks[4][0] = new[] { true, true };
        rocks[4][1] = new[] { true, true };

        var rows = new List<bool[]>();
        rows.Add(new[] { true, true, true, true, true, true, true });
        var blastIndex = 0;
        var rockOffset = 0;
        
        for (var rockIndex = 0; rockIndex < 1000000; ++rockIndex)
        {
            var rock = rocks[rockIndex % rocks.Length];
            var pos = new Point(2, rows.Count + 3);
            ++rockOffset;
            if (blastIndex % blasts.Length == 6)
            {
                Console.WriteLine($"{rockIndex % rocks.Length},{rockIndex},{rows.Count - 1}");
                rockOffset = 0;
            }

            if (rockOffset == 1015)
            {
                Console.WriteLine($"{rockIndex % rocks.Length},{rockIndex},{rows.Count - 1}");
            }

            bool CanMove(Point dir)
            {
                if (pos.X + dir.X < 0 || pos.X + dir.X + rock[0].Length > rows[0].Length)
                {
                    return false;
                }
                
                for (var y = 0; y < rock.Length; ++y)
                {
                    for (var x = 0; x < rock[y].Length; ++x)
                    {   
                        if (rock[y][x] && rows.Count > pos.Y + dir.Y + y &&
                            rows[pos.Y + dir.Y + y][pos.X + dir.X + x])
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            var falling = true;
            while (falling)
            {
                switch (blasts[blastIndex++ % blasts.Length])
                {
                    case '>':
                        if (CanMove(new Point(1, 0)))
                        {
                            pos.X += 1;
                        }
                        break;
                    case '<':
                        if (CanMove(new Point(-1, 0)))
                        {
                            pos.X -= 1;
                        }
                        break;
                    default:
                        throw new Exception("Unrecognised input");
                }

                if (CanMove(new Point(0, -1)))
                {
                    pos.Y -= 1;
                }
                else
                {
                    falling = false;
                    for (var y = 0; y < rock.Length; ++y)
                    {
                        if (rows.Count <= pos.Y + y)
                        {
                            rows.Add(new[] { false, false, false, false, false, false, false });
                        }
                        for (var x = 0; x < rock[y].Length; ++x)
                        {
                            rows[pos.Y + y][pos.X + x] |= rock[y][x];
                        }
                    }
                }
            }
        }


        // for (var y = rows.Count - 1; y >= 0; --y)
        // {
        //     Console.WriteLine("|" + String.Join("", rows[y].Select(c => c ? '#': '.')) + "|");
        // }
        // Console.WriteLine();
        // Console.WriteLine();
        return rows.Count - 1;
    }
}