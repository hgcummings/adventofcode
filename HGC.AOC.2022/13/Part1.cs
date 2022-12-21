using HGC.AOC.Common;
using Newtonsoft.Json.Linq;

namespace HGC.AOC._2022._13;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var pairs = this.ReadInputLines("input.txt")
            .Where(line => line.Trim() != String.Empty)
            .Select(line => JArray.Parse(line.Trim()))
            .Chunk(2);

        var index = 1;
        var total = 0;
        foreach (var pair in pairs)
        {
            if (CheckOrder(pair[0], pair[1]) >= 0)
            {
                total += index;
                Console.WriteLine($"Pair {index} in right order.");
            }
            else
            {
                Console.WriteLine($"Pair {index} in wrong order.");
            }
            ++index;
        }

        return total;
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