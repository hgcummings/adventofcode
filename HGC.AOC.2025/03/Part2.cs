using System.Text;
using HGC.AOC.Common;

namespace HGC.AOC._2025._03;

public class Part2 : ISolution
{
    public object? Answer()
    {
        return this.ReadInputLines().Sum(HighestJoltage);
    }

    public long HighestJoltage(string bank)
    {
        var currentIndex = 0;
        var totalBuilder = new StringBuilder();
        for (var i = 12; i > 0; --i)
        {
            var highestOption = bank[currentIndex..(bank.Length - i + 1)].Max();
            currentIndex = bank.IndexOf(highestOption, currentIndex) + 1;
            totalBuilder.Append(highestOption);
        }
        
        Console.Write(bank);
        var total = Int64.Parse(totalBuilder.ToString());
        Console.WriteLine($" -> {total}");
        return total;
    }
    
    
}