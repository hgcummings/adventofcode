using System.Text;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2016._09;

public class Part2 : ISolution
{
    private readonly Regex _markerRegex = new Regex("(?'count'[0-9]+)x(?'repeat'[0-9]+)");
    
    public string? Answer()
    {
        var inputStream = this.GetInputStream("input.txt");

        return Decompress(inputStream, int.MaxValue).ToString();
    }

    private long Decompress(StreamReader inputStream, int limit)
    {
        long length = 0;
        var markerWriter = new StringBuilder();
        
        for (var i = 0; i < limit && !inputStream.EndOfStream;)
        {
            var next = (char)inputStream.Read();
            ++i;

            if (next == '(')
            {
                markerWriter.Clear();

                while (next != ')')
                {
                    markerWriter.Append(next);
                    next = (char)inputStream.Read();
                    ++i;
                }

                var marker = markerWriter.ToString();
                var match = _markerRegex.Match(marker, 1);

                var count = Int32.Parse(match.Groups["count"].Value);
                var repeat = Int32.Parse(match.Groups["repeat"].Value);
                
                i += count;
                length += Decompress(inputStream, count) * repeat;
            }
            else
            {
                length++;
            }
        }

        return length;
    }
}