using System.Text;
using HGC.AOC.Common;

namespace HGC.AOC._2025._03;

public class Part1 : ISolution
{
    public object? Answer()
    {
        return this.ReadInputLines().Sum(HighestJoltage);
    }

    public int HighestJoltage(string bank)
    {
        Console.Write(bank);
        var major = bank.Substring(0, bank.Length - 1).Max();
        var majorIndex = bank.IndexOf(major);
        var minor = bank.Substring(majorIndex + 1).Max();
        var total = Int32.Parse(new StringBuilder().Append(major).Append(minor).ToString());
        Console.WriteLine($" -> {total}");
        return total;
    }
    
    
}