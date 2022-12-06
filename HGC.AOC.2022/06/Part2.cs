using HGC.AOC.Common;

namespace HGC.AOC._2022._06;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.GetInputStream("input.txt");

        var index = 0;
        var buffer = "";

        while (!input.EndOfStream)
        {
            var next = (char) input.Read();
            if (buffer.Length == 14)
            {
                buffer = buffer.Substring(1);
            }

            buffer += next;
            ++index;
            if (buffer.Distinct().Count() == 14)
            {
                break;
            }

        }
        
        Console.WriteLine(buffer);

        return index;
    }
}