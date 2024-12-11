using HGC.AOC.Common;

namespace HGC.AOC._2024._11;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var stones = this
            .ReadInput("input.txt")
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .GroupBy(s => s)
            .ToDictionary(g => g.Key, g => (long) g.Count());

        for (var i = 0; i < 75; ++i)
        {
            var newStones = new Dictionary<string, long>();
            
            foreach (var entry in stones)
            {
                var newKeys = Step(entry.Key);
                foreach (var key in newKeys)
                {
                    if (newStones.ContainsKey(key))
                    {
                        newStones[key] += entry.Value;
                    }
                    else
                    {
                        newStones[key] = entry.Value;
                    }
                }
            }

            stones = newStones;
            
            Console.WriteLine(i);
        }
        
        return stones.Values.Sum();
    }
    
    private IEnumerable<string> Step(string stone)
    {
        if (stone == "0")
        {
            yield return "1";
        }
        else if (stone.Length % 2 == 0)
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