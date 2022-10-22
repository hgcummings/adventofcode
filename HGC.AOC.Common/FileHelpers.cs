namespace HGC.AOC.Common;

public static class FileHelpers
{
    public static string ReadResource(this object obj, string resourceName)
    {
        var type = obj.GetType();
        var stream = type.Assembly.GetManifestResourceStream(type.Namespace! + ".input.txt")!;
        return new StreamReader(stream).ReadToEnd();
    }
}