using HGC.AOC.Common;

namespace HGC.AOC._2023._13;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var maps = new List<bool[][]>();
        List<bool[]> currentMap = null;

        foreach (var line in this.ReadInputLines("input.txt"))
        {
            if (line.Trim() == "")
            {
                if (currentMap != null)
                {
                    maps.Add(currentMap.ToArray());
                    currentMap = null;
                }
                continue;
            }

            currentMap ??= new List<bool[]>();
            currentMap.Add(line.Select(c => c == '#').ToArray());
        }

        return maps.Select(CalculateValue).Sum();
    }

    long CalculateValue(bool[][] map)
    {
        for (var i = 1; i < map[0].Length; ++i)
        {
            var misMatches = 0;
            for (var j = 1; ((i - j >= 0) && (i - 1 + j < map[0].Length)); ++j)
            {
                for (var y = 0; y < map.Length; ++y)
                {
                    var a = i - j;
                    var b = i - 1 + j;

                    if (map[y][a] != map[y][b])
                    {
                        misMatches += 1;
                    }
                    if (misMatches > 1)
                    {
                        break;
                    }
                }
                if (misMatches > 1)
                {
                    break;
                }
            }

            if (misMatches == 1)
            {
                return i;
            }
        }
        
        for (var i = 1; i < map.Length; ++i)
        {
            var misMatches = 0;
            for (var j = 1; ((i - j >= 0) && (i - 1 + j < map.Length)); ++j)
            {
                for (var x = 0; x < map[0].Length; ++x)
                {
                    var a = i - j;
                    var b = i - 1 + j;

                    if (map[a][x] != map[b][x])
                    {
                        misMatches += 1;
                    }
                    if (misMatches > 1)
                    {
                        break;
                    }
                }
                if (misMatches > 1)
                {
                    break;
                }
            }

            if (misMatches == 1)
            {
                return i * 100;
            }
        }

        throw new Exception("No reflection found");
    }
}