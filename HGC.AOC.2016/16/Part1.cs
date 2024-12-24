using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Drawing;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2016._16;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = "10001001100000001";

        var fill = ExpandToFill(input, 35651584);
        
        return CheckSum(fill);
    }

    string ExpandToFill(string input, int length)
    {
        if (input.Length >= length)
        {
            return input[..length];
        }

        return ExpandToFill(
            input + '0' + String.Join("", 
                input.Reverse().Select(c => c == '1' ? '0' : '1')),
            length);
    }

    string CheckSum(string input)
    {
        if (input.Length % 2 == 1)
        {
            return input;
        }

        var builder = new StringBuilder();
        for (var i = 0; i < input.Length / 2; ++i)
        {
            builder.Append(input[2 * i] == input[2 * i + 1] ? '1' : '0');
        }

        return CheckSum(builder.ToString());
    }
}