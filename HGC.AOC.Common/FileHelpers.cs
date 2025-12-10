namespace HGC.AOC.Common;

public static class FileHelpers
{
    public static StreamReader GetInputStream(this ISolution obj, string resourceName)
    {
        var type = obj.GetType();
        var stream = type.Assembly.GetManifestResourceStream($"{type.Namespace!}.{resourceName}")!;
        return new StreamReader(stream);
    }
    
    public static string ReadInput(this ISolution obj, string resourceName = "input.txt")
    {
        return GetInputStream(obj, resourceName).ReadToEnd();
    }

    public static IEnumerable<string> ReadInputLines(this ISolution obj, string resourceName = "input.txt")
    {
        var streamReader = GetInputStream(obj, resourceName);
        while (!streamReader.EndOfStream)
        {
            yield return streamReader.ReadLine()!;
        }
    }
}