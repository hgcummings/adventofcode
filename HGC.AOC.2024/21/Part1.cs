using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2024._21;

public class Part1 : ISolution
{
    List<string> Numeric =
    [
        "789",
        "456",
        "123",
        " 0A"
    ];
        
    List<string> Directional =
    [
        " ^A",
        "<v>"
    ];
    
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt").ToList();

        return input.Sum(code =>
        {
            var firstSequences = ShortestSequencesNumeric(code).ToList();
            var secondSequences = firstSequences.SelectMany(ShortestSequencesDirectional);
            var thirdSequences = secondSequences.SelectMany(ShortestSequencesDirectional).ToList();
            
            Console.WriteLine(thirdSequences.Count);

            return thirdSequences.Select(s => s.Length).Min() * Int32.Parse(code[..^1]);
        });

    }

    List<string> ShortestSequencesNumeric(string input)
    {
        return ShortestSequences(Numeric, input);
    }

    List<string> ShortestSequencesDirectional(string input)
    {
        return ShortestSequences(Directional, input);
    }
    
    List<string> ShortestSequences(List<string> keypad, string input)
    {
        if (input == String.Empty) return [String.Empty];
        
        (int fromX, int fromY) = Location(keypad, 'A');

        List<string> sequences = new List<string> { String.Empty };
        foreach (var key in input)
        {
            (int toX, int toY) = Location(keypad, key);
            sequences = PathsToKey(keypad, fromX, fromY, toX, toY).Distinct()
                .SelectMany(path => sequences.Select(seq => seq + path)).ToList();
            (fromX, fromY) = (toX, toY);
        }

        return sequences;
    }

    IEnumerable<string> PathsToKey(
        List<string> keypad, int fromX, int fromY, int toX, int toY)
    {
         if (keypad[fromY][toX] != ' ')
         {
             if (toX < fromX || keypad[toY][fromX] == ' ')
             {
                 yield return XPath(fromX, toX) + YPath(fromY, toY) + 'A';
                 yield break;
             }
         }
         if (keypad[toY][fromX] != ' ')
         {
             yield return YPath(fromY, toY) + XPath(fromX, toX) + 'A';
         }
    }

    string XPath(int from, int to)
    {
        if (to < from) return new String('<', from - to);
        if (to > from) return new String('>', to - from);
        return String.Empty;
    }

    string YPath(int from, int to)
    {
        if (to < from) return new String('^', from - to);
        if (to > from) return new String('v', to - from);
        return String.Empty;
    }

    (int x, int y) Location(List<string> keypad, char key)
    {
        var y = keypad.FindIndex(row => row.Contains(key));
        return (keypad[y].IndexOf(key), y);
    }
}