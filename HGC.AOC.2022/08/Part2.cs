using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2022._08;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var heights = new List<List<int>>();
        var scores = new List<List<int>>();
        
        foreach (var line in input)
        {
            heights.Add(line.Select(c => Int32.Parse("" + c)).ToList());
            scores.Add(line.Select(_ => 1).ToList());
        }

        for (var y = 0; y < heights.Count; ++y)
        {
            for (var x = 0; x < heights[y].Count; ++x)
            {
                var height = heights[y][x];
                
                var dirScr = 0;
                for (var i = 1; x + i < heights[y].Count; ++i)
                {
                    dirScr += 1;
                    if (heights[y][x + i] >= height)
                    {
                        break;
                    }
                }
                scores[y][x] *= dirScr;
                
                dirScr = 0;
                for (var i = 1; x - i >= 0; ++i)
                {
                    dirScr += 1;
                    if (heights[y][x - i] >= height)
                    {
                        break;
                    }
                }
                scores[y][x] *= dirScr;
                
                dirScr = 0;
                for (var j = 1; y + j < heights.Count; ++j)
                {
                    dirScr += 1;
                    if (heights[y + j][x] >= height)
                    {
                        break;
                    }
                }
                scores[y][x] *= dirScr;
                
                dirScr = 0;
                for (var j = 1; y - j >= 0; ++j)
                {
                    dirScr += 1;
                    if (heights[y - j][x] >= height)
                    {
                        break;
                    }
                }
                scores[y][x] *= dirScr;
            }
        }
        
        // foreach (var row in scores)
        // {
        //     Console.WriteLine(String.Join(',',row));
        // }

        return scores.SelectMany(row => row).Max();
    }

}