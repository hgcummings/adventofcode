using System.Diagnostics;
using HGC.AOC.Common;

var solution = HGC.AOC._2023.Entry.Current();
if (args.Length > 0)
{
    var part = args.Length > 2 ? int.Parse(args[1]) : 2;
    solution = HGC.AOC._2023.Entry.DayPart(int.Parse(args[0]), part);
}

Console.WriteLine($"= Running {solution.GetType().FullName} =".ExpandEdges(80));
var stopwatch = new Stopwatch();
stopwatch.Start();
var answer = solution.Answer();
stopwatch.Stop();
var answerString = answer?.ToString();
Console.WriteLine($"= Completed in {stopwatch.ElapsedMilliseconds}ms =".ExpandEdges(80));
Console.WriteLine(answerString);
if (answerString != null)
{
    new TextCopy.Clipboard().SetText(answerString);
}