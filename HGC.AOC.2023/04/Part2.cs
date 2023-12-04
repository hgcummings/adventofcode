using HGC.AOC.Common;

namespace HGC.AOC._2023._04;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var cards = this.ReadInputLines("input.txt").ToArray();
        var wins = ArrayHelpers.InitArray(cards.Length, _ => 1);

        for (int i = 0; i < cards.Length; ++i)
        {
            var winningNumbers = cards[i].Split("|")[0].Split(":")[1].Trim()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse);
            var haveNumbers = cards[i].Split("|")[1].Trim()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse);

            var winCount = winningNumbers.Intersect(haveNumbers).Count();

            for (int j = i + 1; j <= i + winCount && j < wins.Length; ++j)
            {
                wins[j] += wins[i];
            }
        }
        
        return wins.Sum();
    }
}