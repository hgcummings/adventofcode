using System.Collections.Immutable;
using System.Globalization;
using HGC.AOC.Common;

namespace HGC.AOC._2024._17;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var prog = new List<byte>();

        foreach (var line in input)
        {
            if (line.StartsWith("Program"))
            {
                prog = line.Substring(line.IndexOf(' ') + 1)
                    .Split(',').Select(Byte.Parse).ToList();
            }
        }

        var allowableStrings = new List<List<string>>();

        for (var j = 0; j < prog.Count; ++j)
        {
            Console.WriteLine($"Target: {prog[j]}");
            var allowableStringsForDigit = new List<string>();
            var target = prog[j];

            for (byte lsb = 0; lsb < 8; ++lsb)
            {
                var offset = lsb ^ 6;

                for (var msb = 0; msb < 8; ++msb)
                {
                    switch (lsb)
                    {
                        case 4 when msb % 2 is not 1:
                        case 6 when msb is not 6:
                        case 7 when msb is not (3 or 7):
                            continue;
                    }

                    var b = offset ^ msb;
                    b ^= 4;
                    if (target != b % 8)
                    {
                        continue;
                    }
                    Console.Write($"{lsb:B3}, {offset}, {msb:B3}:");
                        
                    var msbStr = msb.ToString("B3");
                    for (var k = 3; k < offset; ++k)
                    {
                        msbStr += '.';
                    }

                    var lsbStr = lsb.ToString("B3").Substring(Math.Max(0, 3 - offset));
                    var combStr = msbStr + lsbStr;

                    var leftPad = "";
                    for (var l = 0; l < (prog.Count + 3 - j) * 3 - combStr.Length; ++l)
                    {
                        leftPad += ".";
                    }
                        
                    for (var r = 0; r < j; ++r)
                    {
                        combStr += "...";
                    }
                    Console.WriteLine(leftPad + combStr);
                        
                    allowableStringsForDigit.Add(leftPad+combStr);
                }
            }
            allowableStrings.Add(allowableStringsForDigit);
            Console.WriteLine();
        }

        bool AreCompatible(string a, string b)
        {
            if (a.Length != b.Length)
            {
                throw new InvalidOperationException("Strings must be the same length");
            }

            for (var i = 0; i < a.Length; ++i)
            {
                if (a[i] == '0' && b[i] == '1' || a[i] == '1' && b[i] == '0')
                {
                    return false;
                }
            }

            return true;
        }
        
        IEnumerable<ImmutableList<string>> FindCompatibleSets(
            IEnumerable<ImmutableList<string>> head, int offset)
        {
            if (offset == allowableStrings.Count)
            {
                return head;
            }

            var candidates = allowableStrings[offset];
            return FindCompatibleSets(head
                .SelectMany(set => 
                    candidates.Where(c => set.All(s => AreCompatible(s, c))).Select(set.Add)),
                offset + 1);
        }
        
        var compatibleSets = FindCompatibleSets([ImmutableList<string>.Empty], 0).ToList();

        return compatibleSets.Min(compatibleSet =>
        {
            var combinedString = compatibleSet[0].ToList();
            foreach (var element in compatibleSet)
            {
                Console.WriteLine(element);
                for (var i = 0; i < element.Length; ++i)
                {
                    if (element[i] != '.')
                    {
                        combinedString[i] = element[i];
                    }
                }
            }

            var flattenedString = String.Join("", combinedString);
            Console.WriteLine(flattenedString);
            Console.WriteLine();
            return Int64.Parse(
                flattenedString.Replace('.', '0'),
                NumberStyles.BinaryNumber);
        });
    }
}