using System.Text;
using System.Text.RegularExpressions;
using HGC.AOC.Common;
using static HGC.AOC.Common.ArrayHelpers;

namespace HGC.AOC._2016._09;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var inputStream = this.GetInputStream("input.txt");
        var markerWriter = new StringBuilder();
        var markerRegex = new Regex("(?'count'[0-9]+)x(?'repeat'[0-9]+)");
        var outputWriter = new StringBuilder();

        while (!inputStream.EndOfStream)
        {
            var next = (char) inputStream.Read();

            if (next == '\n')
            {
                Console.WriteLine(outputWriter.ToString());
                outputWriter.Clear();
                continue;
            }
            
            if (next == '(')
            {
                markerWriter.Clear();
                
                while (next != ')')
                {
                    markerWriter.Append(next);
                    next = (char)inputStream.Read();
                }

                var marker = markerWriter.ToString();
                var match = markerRegex.Match(marker, 1);

                var count = Int32.Parse(match.Groups["count"].Value);
                var repeat = Int32.Parse(match.Groups["repeat"].Value);

                var buffer = new char[count];
                inputStream.Read(buffer);

                for (var i = 0; i < repeat; ++i)
                {
                    outputWriter.Append(buffer.AsSpan());
                }
            }
            else
            {
                outputWriter.Append(next);
            }
            
        }

        return outputWriter.ToString().Length.ToString();
    }
}