using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2022._08;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var heights = new List<List<int>>();
        var visible = new List<List<bool>>();
        
        foreach (var line in input)
        {
            heights.Add(line.Select(c => Int32.Parse("" + c)).ToList());
            visible.Add(line.Select(_ => false).ToList());
        }

        for (var y = 0; y < heights.Count; ++y)
        {
            var max = -1;
            for (var x = 0; x < heights[y].Count; ++x)
            {
                if (heights[y][x] > max)
                {
                    max = heights[y][x];
                    visible[y][x] = true;
                }
            }
            
            max = -1;
            for (var x = heights[y].Count - 1; x >= 0; --x)
            {
                if (heights[y][x] > max)
                {
                    max = heights[y][x];
                    visible[y][x] = true;
                }
            }
        }
        
        for (var x = 0; x < heights[0].Count; ++x)
        {
            var max = -1;
            for (var y = 0; y < heights.Count; ++y)
            {
                if (heights[y][x] > max)
                {
                    max = heights[y][x];
                    visible[y][x] = true;
                }
            }
            
            max = -1;
            for (var y = heights.Count - 1; y >= 0; --y)
            {
                if (heights[y][x] > max)
                {
                    max = heights[y][x];
                    visible[y][x] = true;
                }
            }
        }
        
        // foreach (var row in visible)
        // {
        //     Console.WriteLine(String.Concat(row.Select(c => c ? 'V' : ' ')));
        // }

        return visible.SelectMany(row => row.Where(t => t)).Count();
    }

}