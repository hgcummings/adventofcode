using HGC.AOC.Common;

namespace HGC.AOC._2022._06;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.GetInputStream("input.txt");

        var index = 0;
        var buffer = "";

        while (!input.EndOfStream)
        {
            var next = (char) input.Read();
            if (buffer.Length == 4)
            {
                buffer = buffer.Substring(1);
            }

            buffer += next;
            ++index;
            if (buffer.Distinct().Count() == 4)
            {
                break;
            }

        }
        
        Console.WriteLine(buffer);

        return index;
    }
}