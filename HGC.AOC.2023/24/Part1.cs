using System.Collections.Immutable;
using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2023._24;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var hailstones = this.ReadInputLines("input.txt")
            .Select(line =>
            {
                var parts = line.Split(" @ ");
                return new Hailstone(
                    new Point3D(parts[0].Split(", ").Select(Double.Parse).ToArray()),
                    new Point3D(parts[1].Split(", ").Select(Double.Parse).ToArray()),
                    line
                );
            }).ToArray();

        // var min = 7;
        // var max = 17;
        var min = 200000000000000;
        var max = 400000000000000;

        var count = 0;
        for (var i = 0; i < hailstones.Length - 1; ++i)
        {
            for (var j = i + 1; j < hailstones.Length; ++j)
            {
                var a = hailstones[i];
                var b = hailstones[j];
                
                var x = IntersectX(a, b);
                var y = a.Fxy.Value.M * x + a.Fxy.Value.C;
                if (x < min || x > max || y < min || y > max)
                {
                    continue;
                }

                if (Math.Sign(x - a.Pos.X) != Math.Sign(a.Vel.X) ||
                    Math.Sign(x - b.Pos.X) != Math.Sign(b.Vel.X))
                {
                    // Crossed in the past
                    continue;
                }

                //

                if (!Intersect(a, b, out var point))
                {
                    Console.WriteLine($"{a} intersects {b} at {x}");
                }
                
                ++count;
            }
        }

        return count;
    }
    
    
    public bool Intersect(Hailstone a, Hailstone b, out (double x, double y) point)
    {
        var x = IntersectX(a, b);
        
        double y = Double.NegativeInfinity;
        if (Double.IsPositiveInfinity(x))
        {
            if (a.Pos.Y != b.Pos.Y)
            {
                point = (Double.NaN, Double.NaN);
                return false;
            }
        
            y = a.Pos.Y;
        }
        
        if (Double.IsNegativeInfinity(y))
        {
            y = a.Fxy.HasValue
                ? a.Fxy.Value.M * x + a.Fxy.Value.C
                : b.Fxy.Value.M * x + b.Fxy.Value.C;                    
        }
        
        point = (x, y);
        return true;
    }

    public double IntersectX(Hailstone a, Hailstone b)
    {
        if (a.Vel.X == 0 && b.Vel.X == 0)
        {
            if (a.Pos.X == b.Pos.X &&
                (a.Pos.Y == b.Pos.Y || a.Vel.Y != b.Vel.Y))
            {
                return a.Pos.X;
            }

            return double.PositiveInfinity;
        }
        else if (a.Vel.X == 0)
        {
            return a.Pos.X;
        }
        else if (b.Vel.X == 0)
        {
            return b.Pos.X;
        }

        if (a.Fxy.Value.M == b.Fxy.Value.M)
        {
            if (a.Pos.X == b.Pos.X &&
                a.Pos.Y == b.Pos.Y)
            {
                return a.Pos.X;
            }

            return double.PositiveInfinity;
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
                Fxy = new Formula(vel.Y / (double) vel.X,
                    pos.Y - (pos.X * vel.Y / (double) vel.X));
                // Fxz = new Formula(vel.Z / (double) vel.X,
                //     pos.Z - (pos.X * vel.Z / (double) vel.X));
            }
        }

        public Point3D Pos { get; }
        public Point3D Vel { get; }
        
        public Formula? Fxy { get; }

        public override string ToString() => label;
    }

    public struct Formula(double m, double c)
    {
        public double M { get; } = m;
        public double C { get; } = c;
    }

     public readonly struct Point3D(double x, double y, double z)
     {
         public bool Equals(Point3D other)
         {
             return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
         }

         public override bool Equals(object? obj)
         {
             return obj is Point3D other && Equals(other);
         }

         public override int GetHashCode()
         {
             return HashCode.Combine(X, Y, Z);
         }

         public Point3D(double[] coords) : this(coords[0], coords[1], coords[2])
         {
         }

         public bool IsIntegral()
         {
             return Math.Abs(Math.Round(X) - X) < TOLERANCE &&
                    Math.Abs(Math.Round(Y) - Y) < TOLERANCE &&
                    Math.Abs(Math.Round(Z) - Z) < TOLERANCE;
         }

         private const double TOLERANCE = 0.00001;

         public static Point3D operator -(Point3D a, Point3D b)
             => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
         
         
         public static Point3D operator +(Point3D a, Point3D b)
             => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

         
         public static Point3D operator *(double a, Point3D b)
             => new(a * b.X, a * b.Y, a * b.Z);
         
         public static Point3D operator /(Point3D a, double b)
             => new(a.X / b, a.Y / b, a.Z / b);
         
         public double X { get; } = x;
         public double Y { get; } = y;
         public double Z { get; } = z;
     }
}