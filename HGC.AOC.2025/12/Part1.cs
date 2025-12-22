using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2025._12;

public class Part1 : ISolution
{
    readonly int[] sizes = { 6, 7, 7, 7, 6, 7 };
    
    public object? Answer()
    {
        var trees = this.ReadInputLines()
            .SkipWhile(line => !line.Contains('x')).Select(Tree.Parse);

        var treesByOccupancy = trees.Select(tree =>
        {
            var totalArea = tree.Width * tree.Height;
            var totalSize = tree.Gifts.Select((c, i) => c * sizes[i]).Sum();
            var occupancy = (float)totalSize / totalArea;
            return new { tree, totalSize, totalArea, occupancy };
        });

        return treesByOccupancy.Count(tree => tree.occupancy < 0.8);
    }

    struct Tree(int width, int height, int[] gifts)
    {
        public int Width { get; } = width;
        public int Height { get; } = height;
        public int[] Gifts { get; } = gifts;

        public static Tree Parse(string line)
        {
            var parts = line.Split(' ',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var width = Int32.Parse(parts[0].Split('x')[0]);
            var height = Int32.Parse(parts[0].Split('x')[1].TrimEnd(':'));

            var gifts = parts.Skip(1).Select(Int32.Parse).ToArray();

            return new Tree(width, height, gifts);
        }
    }
    
}