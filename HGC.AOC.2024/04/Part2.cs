using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2024._04;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var grid = this.ReadInputLines("input.txt").ToList();
        var width = grid[0].Length;
        var height = grid.Count;

        var count = 0;
        for (var y = 0; y < height; ++y)
        {
            for (var x = 0; x < width; ++x)
            {
                if (grid[y][x] == 'A')
                {
                    if (x > 0 && y > 0 && x < width - 1 && y < height - 1)
                    {
                        if (((grid[y - 1][x - 1] == 'M' && grid[y + 1][x + 1] == 'S') ||
                             (grid[y - 1][x - 1] == 'S' && grid[y + 1][x + 1] == 'M')) &&
                            ((grid[y - 1][x + 1] == 'M' && grid[y + 1][x - 1] == 'S') ||
                             (grid[y - 1][x + 1] == 'S' && grid[y + 1][x - 1] == 'M')))
                        {
                            ++count;
                        }
                    }
                }
            }
        }
        
        return count;
    }
}
    
