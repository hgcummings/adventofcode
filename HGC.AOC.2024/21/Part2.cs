using System.Collections.Concurrent;
using HGC.AOC.Common;

namespace HGC.AOC._2024._21;

public class Part2 : ISolution
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
        return this.ReadInputLines("input.txt").Sum(code =>
            ExpandedCost(25, ShortestSequence(Numeric, code)) * Int64.Parse(code[..^1]));
    }

    readonly struct CacheKey(int rounds, string seq)
    {
        public int Rounds { get; } = rounds;
        public String Seq { get; } = seq;
    }

    private ConcurrentDictionary<CacheKey, long> costCache = new();
    
    long ExpandedCost(int rounds, string input)
    {
        var cacheKey = new CacheKey(rounds, input);

        return costCache.GetOrAdd(cacheKey, key => rounds == 1
            ? ShortestSequence(Directional, key.Seq).Length
            : key.Seq.Split('A')
                .Sum(sub => ExpandedCost(key.Rounds - 1, ShortestSequence(Directional, sub + 'A'))) - 1);
    }

    string ShortestSequence(List<string> keypad, string input)
    {
        (int fromX, int fromY) = Location(keypad, 'A');

        string sequence = String.Empty;
        foreach (var key in input)
        {
            (int toX, int toY) = Location(keypad, key);
            {
                sequence += PathToKey(keypad, fromX, fromY, toX, toY);
            }
            (fromX, fromY) = (toX, toY);
        }

        return sequence;
    }

    string PathToKey(List<string> keypad, int fromX, int fromY, int toX, int toY)
    {
        if (keypad[fromY][toX] != ' ')
        {
            if (toX < fromX || keypad[toY][fromX] == ' ')
            {
                return XPath(fromX, toX) + YPath(fromY, toY) + 'A';
            }
        }
        if (keypad[toY][fromX] != ' ')
        {
            return YPath(fromY, toY) + XPath(fromX, toX) + 'A';
        }

        throw new InvalidOperationException();
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