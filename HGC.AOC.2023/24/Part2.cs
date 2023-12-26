using System.Runtime.InteropServices;
using HGC.AOC.Common;

namespace HGC.AOC._2023._24;

public class Part2 : ISolution
{
    private static readonly decimal TOLERANCE = new(0.00001);

    public object? Answer()
    {
        var hailstones = this.ReadInputLines("input.txt")
            .Select(line =>
            {
                var parts = line.Split(" @ ");
                return new Hailstone(
                    new Point3D(parts[0].Split(", ").Select(Int64.Parse).ToArray()),
                    new Point3D(parts[1].Split(", ").Select(Int64.Parse).ToArray()),
                    line
                );
            }).ToArray();

        var (vx, vy) = FindVxy(hailstones, out var pos);
        Console.WriteLine(
            $"Stones have common intersection {pos.x}, {pos.y} for velocity ({vx}, {vy}, ?)");

        var a0 = hailstones[0];
        var a1 = hailstones.First(s => s.Pos.Z != a0.Pos.Z && s.Vel.Z != a0.Vel.Z);

        var r = new Hailstone(new Point3D(pos.x, pos.y, 0), new Point3D(vx, vy, 0), "rock");

        var t0x = (r.Pos.X - a0.Pos.X) / (decimal) (a0.Vel.X - r.Vel.X);
        var t0y = (r.Pos.Y - a0.Pos.Y) / (decimal) (a0.Vel.Y - r.Vel.Y);
        
        var t1x = (r.Pos.X - a1.Pos.X) / (decimal) (a1.Vel.X - r.Vel.X);
        var t1y = (r.Pos.Y - a1.Pos.Y) / (decimal) (a1.Vel.Y - r.Vel.Y);

        if (t0x != t0y || t1x != t1y)
        {
            throw new Exception("Inconsistent result");
        }

        var z0 = a0.Pos.Z + t0x * a0.Vel.Z;
        var z1 = a1.Pos.Z + t1x * a1.Vel.Z;
        
        Console.WriteLine($"{r} intersects {a0} at z = {z0}, t = {t0x}");
        Console.WriteLine($"{r} intersects {a1} at z = {z1}, t = {t1x}");

        var vz = (z1 - z0) / (t1x - t0x);
        var zFrom0 = z0 - vz * t0x;
        var zFrom1 = z1 - vz * t1x;

        if (zFrom0 != zFrom1)
        {
            throw new Exception("Inconsistent result");
        }
        
        return r.Pos.X + r.Pos.Y + zFrom0;
    }

    private (long, long) FindVxy(Hailstone[] hailstones, out (long x, long y) pos)
    {
        for (var range = 0L;; ++range)
        {
            Console.WriteLine(range);
            for (var vx = -range; vx <= range; ++vx)
            {
                for (var vy = -range; vy <= range; ++vy)
                {
                    if (Math.Abs(vx) < range && Math.Abs(vy) < range)
                    {
                        continue;
                    }
                    
                    var offset = new Point3D(vx, vy, 0);

                    var relStones = hailstones.Select(s => new Hailstone(
                        s.Pos,
                        s.Vel - offset,
                        "")).ToArray();

                    if (HaveCommonIntersection(relStones, out var intersection))
                    {
                        Console.WriteLine(
                            $"Stones have common intersection {intersection.x}, {intersection.y} for velocity ({vx}, {vy}, ?)");
                        pos = ((long)decimal.Round(intersection.x),
                            (long)decimal.Round(intersection.y));
                        return (vx, vy);
                    }
                }
            }
        }
    }
    
    private bool HaveCommonIntersection(
        Hailstone[] hailstones,
        out (decimal x, decimal y) intersection)
    {
        var a = hailstones[0];
        decimal? prevX = null;
        decimal? prevY = null;
        intersection = (decimal.MinValue, decimal.MinValue);

        foreach (var b in hailstones.Skip(1))
        {
            if (Intersect(a, b, out var point))
            {
                var (x, y) = point;
                
                if (Math.Sign(x - a.Pos.X) != Math.Sign(a.Vel.X) ||
                    Math.Sign(x - b.Pos.X) != Math.Sign(b.Vel.X))
                {
                    // Crossed in the past
                    return false;
                }

                if ((prevX.HasValue && Math.Abs(x - prevX.Value) > TOLERANCE) ||
                    (prevY.HasValue && Math.Abs(y - prevY.Value) > TOLERANCE))
                {
                    return false;
                }

                prevX = x;
                prevY = y;
            }
            else
            {
                return false;
            }
        }

        intersection = (prevX!.Value, prevY!.Value);
        return true;
    }

    public bool Intersect(Hailstone a, Hailstone b, out (decimal x, decimal y) point)
    {
        var x = IntersectX(a, b);
        
        decimal? y = null;
        if (!x.HasValue)
        {
            point = (Decimal.MinValue, Decimal.MinValue);
            return false;
        }
        
        if (!y.HasValue)
        {
            y = a.Fxy.HasValue
                ? a.Fxy.Value.M * x + a.Fxy.Value.C
                : b.Fxy.Value.M * x + b.Fxy.Value.C;
        }
        
        point = (x!.Value, y!.Value);
        return true;
    }
    
    public decimal? IntersectX(Hailstone a, Hailstone b)
    {
        if (a.Vel.X == 0 && b.Vel.X == 0)
        {
            if (a.Pos.X == b.Pos.X &&
                (a.Pos.Y == b.Pos.Y || a.Vel.Y != b.Vel.Y))
            {
                return a.Pos.X;
            }
    
            return null;
        }
        else if (a.Vel.X == 0)
        {
            return a.Pos.X;
        }
        else if (b.Vel.X == 0)
        {
            return b.Pos.X;
        }
    
        if (Math.Abs(a.Fxy!.Value.M - b.Fxy!.Value.M) < TOLERANCE)
        {
            if (Math.Abs(a.Fxy.Value.C - b.Fxy.Value.C) < TOLERANCE)
            {
                return a.Pos.X;
            }
    
            return null;
        }
        
        return (b.Fxy.Value.C - a.Fxy.Value.C) / (a.Fxy.Value.M - b.Fxy.Value.M);
    }

    public class Hailstone
    {
        private string label;
        
        public Hailstone(Point3D pos, Point3D vel, string label)
        {
            this.label = label;
            Pos = pos;
            Vel = vel;
            if (vel.X != 0)
            {
                Fxy = new Formula(vel.Y / (decimal) vel.X,
                    pos.Y - (pos.X * vel.Y / (decimal) vel.X));
            }
        }

        public Point3D Pos { get; }
        public Point3D Vel { get; }
        
        public Formula? Fxy { get; }

        public override string ToString() => label;
    }

    public struct Formula(decimal m, decimal c)
    {
        public decimal M { get; } = m;
        public decimal C { get; } = c;
    }

    public readonly struct Point3D(long x, long y, long z)
    {
        public Point3D(long[] coords) : this(coords[0], coords[1], coords[2])
        {
        }

        public static Point3D operator -(Point3D a, Point3D b)
            => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        
        
        public static Point3D operator +(Point3D a, Point3D b)
            => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        
        public static Point3D operator *(long a, Point3D b)
            => new(a * b.X, a * b.Y, a * b.Z);
        
        public static Point3D operator /(Point3D a, long b)
            => new(a.X / b, a.Y / b, a.Z / b);
        
        public long X { get; } = x;
        public long Y { get; } = y;
        public long Z { get; } = z;
    }
}