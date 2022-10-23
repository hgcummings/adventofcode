namespace HGC.AOC.Common;

public static class FileHelpers
{
    public static string ReadInput(this ISolution obj, string resourceName)
    {
        var type = obj.GetType();
        var stream = type.Assembly.GetManifestResourceStream($"{type.Namespace!}.{resourceName}")!;
        return new StreamReader(stream).ReadToEnd();
    }

    public static IEnumerable<string> ReadInputLines(this ISolution obj, string resourceName)
    {
        var type = obj.GetType();
        var stream = type.Assembly.GetManifestResourceStream($"{type.Namespace!}.{resourceName}")!;
        var streamReader = new StreamReader(stream);
        while (!streamReader.EndOfStream)
        {
            yield return streamReader.ReadLine()!;
        }
    }
}