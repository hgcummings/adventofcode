using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2023._07;

public class Part1 : ISolution
{
    public object? Answer()
    {
        return this.ReadInputLines("input.txt")
            .Select(line => line.SplitBySpaces())
            .Select(arr => new Play(arr[0], Int32.Parse(arr[1])))
            .OrderBy(p => p.Hand, new HandComparer())
            .Select(p => p.Bid)
            .Select((x, i) => x * (i + 1))
            .Sum();
    }

    public const string CardVals = "23456789TJQKA";

    public static HandType GetHandType(String hand)
    {
        var groups = hand.GroupBy(c => c).OrderByDescending(g => g.Count()).ToList();

        if (groups.Count == 1)
        {
            return HandType.FiveOfAKind;
        }

        if (groups[0].Count() == 4)
        {
            return HandType.FourOfAKind;
        }

        if (groups.Count == 2)
        {
            return HandType.FullHouse;
        }

        if (groups[0].Count() == 3)
        {
            return HandType.ThreeOfAKind;
        }

        if (groups.Count == 3)
        {
            return HandType.TwoPair;
        }

        if (groups.Count == 4)
        {
            return HandType.OnePair;
        }

        return HandType.HighCard;
    }
    
    public class HandComparer : IComparer<String>
    {
        public int Compare(string? x, string? y)
        {
            if ((x == null) && (y == null)) return 0;
            if (x == null) return 1;
            if (y == null) return -1;

            var xHandType = GetHandType(x);
            var yHandType = GetHandType(y);
            
            if (xHandType != yHandType)
            {
                return xHandType - yHandType;
            }

            foreach (var cards in x.Zip(y))
            {
                var (xCard, yCard) = cards;
                if (xCard != yCard)
                {
                    return CardVals.IndexOf(xCard) - CardVals.IndexOf(yCard);
                }
            }

            return 0;
        }
    }

    public struct Play
    {
        public string Hand { get; }
        public int Bid { get; }

        public Play(string hand, int bid)
        {
            Hand = hand;
            Bid = bid;
        }
    }
    
    public enum HandType
    {
        HighCard = 0,
        OnePair = 1,
        TwoPair = 2,
        ThreeOfAKind = 3,
        FullHouse = 4,
        FourOfAKind = 5,
        FiveOfAKind = 6
    }
}