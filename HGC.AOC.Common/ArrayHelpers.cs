namespace HGC.AOC.Common;

public static class ArrayHelpers
{
    public static T[] InitArray<T>(int length, Func<int, T> initValue)
    {
        var array = new T[length];

        for (var i = 0; i < length; ++i)
        {
            array[i] = initValue(i);
        }

        return array;
    }
}