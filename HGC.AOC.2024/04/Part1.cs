using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2024._04;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var grid = this.ReadInputLines("input.txt").ToList();
        var width = grid[0].Length;
        var height = grid.Count;

        var dirs = new List<(int, int)>
        {
            (-1, -1), (-1, 0), (-1, 1), (0, -1),
            (0, 1), (1, -1), (1, 0), (1, 1)
        };

        var count = 0;

        bool FindWord(string word, int x, int y, (int dx, int dy) dir)
        {
            for (var i = 0; i < word.Length; ++i)
            {
                var xi = x + i * dir.dx;
                var yi = y + i * dir.dy;

                if (xi < 0 || xi > width - 1 || yi < 0 || yi > height - 1 || grid[yi][xi] != word[i])
                {
                    return false;
                }
            }

            return true;
        }
        
        for (var y = 0; y < height; ++y)
        {
            for (var x = 0; x < width; ++x)
            {
                count += dirs.Count(d => FindWord("XMAS", x, y, d));
            }
        }
        
        return count;
    }
}
    
