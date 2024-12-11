using HGC.AOC.Common;

namespace HGC.AOC._2024._11;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var stones = this
            .ReadInput("input.txt")
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .AsEnumerable();

        for (var i = 0; i < 25; ++i)
        {
            stones = stones.SelectMany(Step);
        }
        
        return stones.Count();
    }

    private IEnumerable<string> Step(string stone)
    {
        if (stone == "0")
        {
            yield return "1";
        }
        else
        {
            if (stone.Length % 2 == 0)
            {
                yield return stone.Substring(0, stone.Length / 2);
                yield return Int64.Parse(stone.Substring(stone.Length / 2)).ToString();
            }
            else
            {
                yield return (Int64.Parse(stone) * 2024).ToString();
            }
        }
    }
}