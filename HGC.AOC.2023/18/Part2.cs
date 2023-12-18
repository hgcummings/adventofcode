﻿using System.ComponentModel;
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
        var maxY = lines.Max(line => Math.Max(line.A.Y, line.B.Y));

        long count = 0;

        for (var y = minY; y <= maxY; ++y)
        {
            var row = y;
            var vLines = lines
                .Where(line => line.IsVertical && line.IntersectsRow(row))
                .OrderBy(line => line.A.X);

            Line? prev = null;
            var inside = false;
            var inPair = false;
            foreach (var line in vLines)
            {
                count += 1;
                if (line.A.Y != y && line.B.Y != y)
                {
                    if (inside && prev != null)
                    {
                        count += line.A.X - prev.Value.A.X - 1;
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
                            count += line.A.X - prev.Value.A.X - 1;
                        }
                        else if (line.A.Y == prev.Value.A.Y || line.B.Y == prev.Value.B.Y)
                        {
                            count += line.A.X - prev.Value.A.X - 1;
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
                            count += line.A.X - prev.Value.A.X - 1;
                        }
                        
                        inPair = true;
                    }
                }
                    
                prev = line;
            }

            if (y % 100000 == 0)
            {
                Console.WriteLine($"{minY}/{y}/{maxY}");
            }
        }

        return count;
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