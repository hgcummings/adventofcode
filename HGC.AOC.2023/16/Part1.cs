using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2023._16;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var map = this.ReadInputLines("input.txt").ToArray();
        var beams = new List<Beam> { new(0, 0, Dir.Right) };
        var energisedLocations = new HashSet<Point>();
        var processedBeamStates = new HashSet<Beam>();

        while (beams.Count > 0)
        {
            var newBeams = new List<Beam>();
            foreach (var beam in beams)
            {
                if (beam.InBounds(map) && processedBeamStates.Add(beam))
                {
                    var cell = map[beam.Y][beam.X];
                    energisedLocations.Add(new Point(beam.X, beam.Y));
                    switch (cell, beam.D)
                    {
                        case ('.', _)
                            or ('-', Dir.Right) or ('|', Dir.Down)
                            or ('-', Dir.Left) or ('|', Dir.Up):
                            newBeams.Add(beam.Advance());
                            break;
                        case ('/', _) or ('\\', _):
                            newBeams.Add(beam.Reflect(cell).Advance());
                            break;
                        case ('|', Dir.Right) or ('|', Dir.Left):
                            newBeams.Add(new Beam(beam.X, beam.Y + 1, Dir.Down));
                            newBeams.Add(new Beam(beam.X, beam.Y - 1, Dir.Up));
                            break;
                        case ('-', Dir.Down) or ('-', Dir.Up):
                            newBeams.Add(new Beam(beam.X + 1, beam.Y, Dir.Right));
                            newBeams.Add(new Beam(beam.X - 1, beam.Y, Dir.Left));
                            break;
                    }
                }
            }

            beams = newBeams;
        }

        return energisedLocations.Count;
    }

    struct Beam(int x, int y, Dir d)
    {
        public bool Equals(Beam other)
        {
            return X == other.X && Y == other.Y && D == other.D;
        }

        public override bool Equals(object? obj)
        {
            return obj is Beam other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, (int)D);
        }

        public int X { get; } = x;
        public int Y { get; } = y;
        public Dir D { get; } = d;

        public Beam Advance()
        {
            return D switch
            {
                Dir.Right => new Beam(X + 1, Y, D),
                Dir.Down => new Beam(X, Y + 1, D),
                Dir.Left => new Beam(X - 1, Y, D),
                Dir.Up => new Beam(X, Y - 1, D),
                _ => throw new Exception()
            };
        }

        public Beam Reflect(char cell)
        {
            return (D, cell) switch
            {
                (Dir.Down, '\\') or (Dir.Up, '/') => new Beam(X, Y, Dir.Right),
                (Dir.Right, '\\') or (Dir.Left, '/') => new Beam(X, Y, Dir.Down),
                (Dir.Down, '/') or (Dir.Up, '\\') => new Beam(X, Y, Dir.Left),
                (Dir.Right, '/') or (Dir.Left, '\\') => new Beam(X, Y, Dir.Up),
                _ => throw new Exception()
            };
        }

        public bool InBounds(string[] map)
        {
            return X >= 0 && X < map.Length && Y >= 0 && Y < map[0].Length;
        }
    }

    enum Dir
    {
        Right,
        Down,
        Left,
        Up
    }
}