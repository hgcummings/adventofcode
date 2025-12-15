using HGC.AOC.Common;

namespace HGC.AOC._2023._22;

public class Part2 : ISolution
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
            }).OrderBy(b => b.From.Z).ToArray();

        bool Advance()
        {
            var anyFell = false;
            for (var i = 0; i < bricks.Length; ++i)
            {
                Brick a = bricks[i];
                if (a.OnGround)
                {
                    continue;
                }

                var newZ = a.From.Z;
                while (newZ > 1 && !bricks.Take(i).Any(b =>
                           b.From.Z <= newZ - 1 && b.To.Z >= newZ - 1 &&
                           b.From.X <= a.To.X && a.From.X <= b.To.X &&
                           b.From.Y <= a.To.Y && a.From.Y <= b.To.Y))
                {
                    newZ--;
                }
                
                if (newZ != a.From.Z)
                {
                    a.FallTo(newZ); 
                    anyFell = true;
                }
            }

            return anyFell;
        }

        while (Advance())
        {
        }

        Dictionary<Brick, Brick[]> directSupport =
            bricks.ToDictionary(ub => ub, ub => bricks
                .Where(lb => ub.SupportedBy().Intersect(lb.HighestPoints()).Any()).ToArray());
        
        IEnumerable<Brick> DirectlySupportedBy(ICollection<Brick> obs)
        {
            return bricks.Where(b => !obs.Contains(b))
                .Where(b => directSupport[b].Length > 0 && directSupport[b].All(obs.Contains));
        }
        
        IEnumerable<Brick> IndirectlySupportedBy(ICollection<Brick> obs)
        {
            var allSupported = DirectlySupportedBy(obs).ToHashSet();
            if (allSupported.Count != 0)
            {
                var expanding = true;
                while (expanding)
                {
                    expanding = false;
                    foreach (var additional in IndirectlySupportedBy(allSupported))
                    {
                        expanding |= allSupported.Add(additional);
                    }
                }
            }

            return allSupported;
        }

        return bricks.AsParallel().Select(b => IndirectlySupportedBy(new[] { b }).Count()).Sum();
    }
    
    public class Brick(Point3D from, Point3D to)
    {
        public Point3D From { get; private set; } = from;
        public Point3D To { get; private set; } = to;

        public bool OnGround => From.Z == 1;

        public IEnumerable<Point3D> SupportedBy()
        {
            if (OnGround)
            {
                yield break;
            }
            
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

        public void FallTo(int z)
        {
            To = new Point3D(To.X, To.Y, To.Z - From.Z + z);
            From = new Point3D(From.X, From.Y, z);
        }
    }
}