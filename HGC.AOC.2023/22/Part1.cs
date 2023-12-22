using HGC.AOC.Common;

namespace HGC.AOC._2023._22;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var bricks = this.ReadInputLines("input.txt").Select(
            line =>
            {
                var ends = line.Split("~").Select(e =>
                {
                    var coords = e.Split(",").Select(Int32.Parse).ToArray();
                    return new Point3D(coords[0], coords[1], coords[2]);
                }).OrderBy(p => p.X + p.Y + p.Z).ToArray();
                return new Brick(ends[0], ends[1]);
            }).ToArray();

        bool Advance()
        {
            var anyFell = false;
            foreach (var brick in bricks)
            {
                if (brick.OnGround)
                {
                    continue;
                }
                
                if (brick.SupportedBy().Any(p => (bricks.Any(b => b.Occupies(p)))))
                {
                    continue;
                }

                brick.Fall();
                anyFell = true;
            }

            return anyFell;
        }

        Console.WriteLine(String.Join(", ", bricks.Select(b => b.From.Z)));
        Console.WriteLine(bricks.Max(b => b.From.Z));
        for (var i = 0; Advance(); i++)
        {
            if (i % 100 == 0)
            {
                Console.WriteLine(bricks.Max(b => b.From.Z));
            }
        }

        var labels = new[] { "A", "B", "C", "D", "E", "F", "G" };

        var supportingBricks = bricks.SelectMany((b, i) =>
        {
            var supporters =
                bricks.Where(ob => b.SupportedBy().Intersect(ob.HighestPoints()).Any()).ToArray();

            if (supporters.Length == 1)
            {
                return supporters;
            }

            return Enumerable.Empty<Brick>();
        }).Distinct().ToArray();
        
        Console.WriteLine($"{supportingBricks.Length} supporting out of {bricks.Length}");

        return bricks.Length - supportingBricks.Count();
    }
    
    public class Brick(Point3D from, Point3D to)
    {
        public Point3D From { get; private set; } = from;
        public Point3D To { get; private set; } = to;

        public bool Occupies(Point3D p)
        {
            return p.X >= From.X && p.X <= To.X &&
                   p.Y >= From.Y && p.Y <= To.Y &&
                   p.Z >= From.Z && p.Z <= To.Z;
        }

        public bool OnGround => From.Z == 1;

        public IEnumerable<Point3D> SupportedBy()
        {
            for (var x = From.X; x <= To.X; ++x)
            {
                for (var y = From.Y; y <= To.Y; ++y)
                {
                    yield return new Point3D(x, y, From.Z - 1);
                }
            }
        }

        public IEnumerable<Point3D> HighestPoints()
        {
            for (var x = From.X; x <= To.X; ++x)
            {
                for (var y = From.Y; y <= To.Y; ++y)
                {
                    yield return new Point3D(x, y, To.Z);
                }
            }
        }

        public void Fall()
        {
            From = new Point3D(From.X, From.Y, From.Z - 1);
            To = new Point3D(To.X, To.Y, To.Z - 1);
        }
    }

    public readonly struct Point3D(int x, int y, int z)
    {
        public bool Equals(Point3D other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override bool Equals(object? obj)
        {
            return obj is Point3D other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        public int X { get; } = x;
        public int Y { get; } = y;
        public int Z { get; } = z;
    }
}