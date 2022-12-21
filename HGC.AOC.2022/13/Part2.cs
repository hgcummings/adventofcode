using HGC.AOC.Common;
using Newtonsoft.Json.Linq;

namespace HGC.AOC._2022._13;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var divider1 = new JArray(new JArray(2));
        var divider2 = new JArray(new JArray(6));

        var packets = this.ReadInputLines("input.txt")
            .Where(line => line.Trim() != String.Empty)
            .Select(line => JArray.Parse(line.Trim()))
            .Concat(new [] { divider1, divider2 })
            .OrderByDescending(packet => packet, new PacketComparer())
            .ToArray();
        
        return (Array.IndexOf(packets, divider1) + 1) * (Array.IndexOf(packets, divider2) + 1);
    }

    private class PacketComparer : IComparer<JArray>
    {
        public int Compare(JArray? x, JArray? y)
        {
            return CheckOrder(x!, y!);
        }
        
        private static int CheckOrder(JToken left, JToken right)
        {
            if (left is JArray leftArray && right is JArray rightArray)
            {
                return CheckOrder(leftArray, rightArray);
            }

            if (left is JArray lArray)
            {
                return CheckOrder(lArray, new JArray(right));
            }

            if (right is JArray rArray)
            {
                return CheckOrder(new JArray(left), rArray);
            }

            return right.Value<int>() - left.Value<int>();
        }

        private static int CheckOrder(JArray leftArray, JArray rightArray)
        {
            for (var i = 0;; ++i)
            {
                if (i >= leftArray.Count && i >= rightArray.Count)
                {
                    return 0;
                }
            
                if (i >= leftArray.Count)
                {
                    return 1;
                }

                if (i >= rightArray.Count)
                {
                    return -1;
                }

                var elementOrder = CheckOrder(leftArray[i], rightArray[i]);
                if (elementOrder != 0)
                {
                    return elementOrder;
                }
            }
        }
    }

    
}