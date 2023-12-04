using HGC.AOC.Common;

namespace HGC.AOC._2023._04;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var cards = this.ReadInputLines("input.txt");

        var total = cards.Select(card =>
        {
            var winningNumbers = card.Split("|")[0].Split(":")[1].Trim()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse);
            var haveNumbers = card.Split("|")[1].Trim()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse);

            var winCount = winningNumbers.Intersect(haveNumbers).Count();

            var score = winCount == 0 ? 0 : Math.Pow(2, winCount - 1);
            return score;
        }).Sum();
        
        return total;
    }
}