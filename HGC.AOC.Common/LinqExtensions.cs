namespace HGC.AOC.Common;

public static class LinqExtensions
{
    public static long Product<T>(this IEnumerable<T> collection, Func<T, long> value)
    {
        return collection.Aggregate(1L, (acc, cur) => acc * value(cur));
    }
    
    public static long Product(this IEnumerable<long> collection)
    {
        return collection.Aggregate(1L, (acc, cur) => acc * cur);
    }
    
    public static long Product(this IEnumerable<int> collection)
    {
        return collection.Aggregate(1L, (acc, cur) => acc * cur);
    }
}