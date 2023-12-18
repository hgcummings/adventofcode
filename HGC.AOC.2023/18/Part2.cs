using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2023._18;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var instructionRegex = new Regex(
            "[RDLU] [0-9]+ \\(#(?'DistString'[0-9a-f]{5})(?'Dir'[0-3])\\)");

        var instructions = this.ReadInputLines("input.txt")
            .Select(line => instructionRegex.Match(line).Parse<Instruction>());

        var start = new Point(0, 0);
        var position = start;
        var lines = new List<Line>();
        
        foreach (var step in instructions)
        {
            var nextPos = step.Dir switch
            {
                0 => position with { X = position.X + step.Dist },
                1 => position with { Y = position.Y + step.Dist },
                2 => position with { X = position.X - step.Dist },
                3 => position with { Y = position.Y - step.Dist },
                _ => throw new ArgumentOutOfRangeException()
            };
            lines.Add(new Line(position, nextPos));
            position = nextPos;
        }
        
        var minY = lines.Min(line => Math.Min(line.A.Y, line.B.Y));

        var countsPerRow = new Dictionary<int, long>();
        foreach (var y in lines
                     .SelectMany<Line, int>(l => [l.A.Y, l.A.Y + 1, l.B.Y, l.B.Y + 1])
                     .Distinct())
        {
            var row = y;
            long countForRow = 0;
            var vLines = lines
                .Where(line => line.IsVertical && line.IntersectsRow(row))
                .OrderBy(line => line.A.X);

            Line? prev = null;
            var inside = false;
            var inPair = false;
            foreach (var line in vLines)
            {
                countForRow += 1;
                if (line.A.Y != y && line.B.Y != y)
                {
                    if (inside && prev != null)
                    {
                        countForRow += line.A.X - prev.Value.A.X - 1;
                    }
                    inside = !inside;
                }
                else
                {
                    if (inPair && prev != null)
                    {
                        if (line.A.Y == prev.Value.B.Y || line.B.Y == prev.Value.A.Y)
                        {
                            inside = !inside;
                            countForRow += line.A.X - prev.Value.A.X - 1;
                        }
                        else if (line.A.Y == prev.Value.A.Y || line.B.Y == prev.Value.B.Y)
                        {
                            countForRow += line.A.X - prev.Value.A.X - 1;
                        }
                        else
                        {
                            throw new Exception("Invalid pair");
                        }

                        inPair = false;
                    }
                    else
                    {
                        if (inside && prev != null)
                        {
                            countForRow += line.A.X - prev.Value.A.X - 1;
                        }
                        
                        inPair = true;
                    }
                }
                    
                prev = line;
            }

            countsPerRow[y] = countForRow;
        }

        var prevY = minY - 1;
        var prevCount = 0L;
        var total = 0L;
        foreach (var (y, count) in countsPerRow.OrderBy(entry => entry.Key))
        {
            total += (y - prevY - 1) * prevCount;
            total += count;

            prevY = y;
            prevCount = count;
        }

        return total;
    }

    public class Instruction
    {
        public string DistString { get; set; }
        public int Dist => int.Parse(DistString, NumberStyles.HexNumber);
        public int Dir { get; set; }

    }

    public struct Line
    {
        public Line(Point a, Point b)
        {
            if (a.Y < b.Y)
            {
                A = a;
                B = b;                
            }
            else
            {
                A = b;
                B = a;
            }

            if (!IsHorizontal && !IsVertical)
            {
                throw new Exception("Invalid line");
            }
        }

        public Point A { get; }
        public Point B { get; }

        public bool IntersectsRow(int y)
        {
            return y >= Math.Min(A.Y, B.Y) && y <= Math.Max(A.Y, B.Y);
        }

        public bool IsVertical => A.X == B.X;
        public bool IsHorizontal => A.Y == B.Y;
    }

}