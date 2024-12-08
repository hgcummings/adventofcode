namespace HGC.AOC.Common;

public static class Arithmetic
{
    public static long GreatestCommonDivisor(long a, long b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);
        while (true)
        {
            if (a == b)
            {
                return a;
            }

            var a1 = a;
            a = Math.Abs(a - b);
            b = Math.Min(a1, b);
        }
    }

    public static long LeastCommonMultiple(long a, long b)
    {
        return Math.Abs(a * b) / GreatestCommonDivisor(a, b);
    }
}