using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2025._09;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var vertexTiles = this.ReadInputLines()
            .Select(line =>
            {
                var components = line.Split(',').Select(Int32.Parse).ToList();
                return new Point(components[0], components[1]);
            }).ToList();

        var horizontalEdges = new List<HorizontalEdge>();
        var verticalEdges = new List<VerticalEdge>();
        for (var i = 0; i < vertexTiles.Count; ++i)
        {
            var from = vertexTiles[i];
            var to = i == vertexTiles.Count - 1 ? vertexTiles[0] : vertexTiles[i + 1];

            if (from.X == to.X)
            {
                verticalEdges.Add(
                    new VerticalEdge(from.X, Math.Min(from.Y, to.Y), Math.Max(from.Y, to.Y)));
            }
            else if (from.Y == to.Y)
            {
                horizontalEdges.Add(
                    new HorizontalEdge(from.Y, Math.Min(from.X, to.X), Math.Max(from.X, to.X)));
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        bool IsValidRectangle(Point a, Point b)
        {
            var minX = Math.Min(a.X, b.X);
            var minY = Math.Min(a.Y, b.Y);
            var maxX = Math.Max(a.X, b.X);
            var maxY = Math.Max(a.Y, b.Y);
            
            if (horizontalEdges.Any(e => e.Y > minY && e.Y < maxY && e.FromX < maxX && e.ToX > minX))
            {
                return false;
            }

            if (verticalEdges.Any(e => e.X > minX && e.X < maxX && e.FromY < maxY && e.ToY > minY))
            {
                return false;
            }

            return true;
        }
        
        var max = 0L;

        for (var i = 0; i < vertexTiles.Count; ++i)
        {
            for (var j = i + 1; j < vertexTiles.Count; ++j)
            {
                var area = (Math.Abs(vertexTiles[j].X - vertexTiles[i].X) + 1L) *
                           (Math.Abs(vertexTiles[j].Y - vertexTiles[i].Y) + 1L);
                
                if (area <= max)
                {
                    continue;
                }

                if (IsValidRectangle(vertexTiles[i], vertexTiles[j]))
                {
                    max = area;
                }
            }
        }

        return max;
    }

    struct HorizontalEdge
    {
        public int Y;
        public int FromX;
        public int ToX;

        public HorizontalEdge(int y, int fromX, int toX)
        {
            Y = y;
            FromX = fromX;
            ToX = toX;
        }
    }

    struct VerticalEdge
    {
        public int X;
        public int FromY;
        public int ToY;

        public VerticalEdge(int x, int fromY, int toY)
        {
            X = x;
            FromY = fromY;
            ToY = toY;
        }
    }
}