namespace HGC.AOC.Common;

public static class StringHelpers
{
    public static string ExpandEdges(this string input, int length)
    {
        if (input.Length >= length)
        {
            return input;
        }
        else
        {
            var padding = length - input.Length;
            return input
                .PadLeft(input.Length + padding / 2, input[0])
                .PadRight(length, input[^1]);
        }
    }
}