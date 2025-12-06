using HGC.AOC.Common;

namespace HGC.AOC._2025._02;

public class Part2 : ISolution
{
    public object? Answer()
    {
        long total = 0;
        
        foreach (var str in this.ReadInput().Split(","))
        {
            var range = str.Split("-").Select(Int64.Parse).ToList();
            for (long i = range[0]; i <= range[1]; ++i)
            {
                if (IsInvalid(i))
                {
                    total += i;
                }
            }
        }

        return total;
    }

    public bool IsInvalid(long id)
    {
        var idStr = id.ToString();
        for (var l = 1; l <= idStr.Length / 2; ++l)
        {
            if (idStr.Length % l == 0)
            {
                if (IsInvalid(idStr, l))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsInvalid(string id, int l)
    {
        var sequence = id.Substring(0, l);
        for (var i = 1; i < id.Length / l; ++i)
        {
            if (id.Substring(i * l, l) != sequence)
            {
                return false;
            }
        }

        return true;
    }
}