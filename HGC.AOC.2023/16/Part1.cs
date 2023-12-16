using System.Drawing;
using System.Runtime.Intrinsics.X86;
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
        var history = new List<int>();

        var beamsToProcess = true;
        while (beamsToProcess)
        {
            var newBeams = new List<Beam>();
            beamsToProcess = false;
            foreach (var beam in beams)
            {
                if (!beam.InBounds(map) || processedBeamStates.Contains(beam))
                {
                    continue;
                }

                beamsToProcess = true;
                processedBeamStates.Add(beam);
                
                var cell = map[beam.Y][beam.X];
                energisedLocations.Add(new Point(beam.X, beam.Y));
                switch (cell, beam.D)
                {
                    case ('.', _) or ('-', Dir.Right) or ('|', Dir.Down) or ('-', Dir.Left) or ('|', Dir.Up):
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

            beams = newBeams;
            history.Add(energisedLocations.Count);
            Console.WriteLine(energisedLocations.Count);
        }

        // OutputEnergised(map, energisedLocations);
        return history[^1];
    }

    private static void OutputEnergised(string[] map, HashSet<Point> energisedLocations)
    {
        Console.WriteLine();
        for (var y = 0; y < map.Length; ++y)
        {
            for (var x = 0; x < map[y].Length; ++x)
            {
                Console.Write(energisedLocations.Contains(new Point(x, y)) ? '#' : '.');
            }
            Console.WriteLine();
        }
        Console.WriteLine();
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
            switch (D)
            {
                case Dir.Right:
                    return new Beam(X + 1, Y, D);
                case Dir.Down:
                    return new Beam(X, Y + 1, D);
                case Dir.Left:
                    return new Beam(X - 1, Y, D);
                case Dir.Up:
                    return new Beam(X, Y - 1, D);
            }

            throw new Exception();
        }

        public Beam Reflect(char cell)
        {
            switch (D, cell)
            {
                case (Dir.Down, '\\') or (Dir.Up, '/'):
                    return new Beam(X, Y, Dir.Right);
                case (Dir.Right, '\\') or (Dir.Left, '/'):
                    return new Beam(X, Y, Dir.Down);
                case (Dir.Down, '/') or (Dir.Up, '\\'):
                    return new Beam(X, Y, Dir.Left);
                case (Dir.Right, '/') or (Dir.Left, '\\'):
                    return new Beam(X, Y, Dir.Up);
            }
            
            throw new Exception();
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