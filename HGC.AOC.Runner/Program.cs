using System.Diagnostics;
using HGC.AOC.Common;

var solution = HGC.AOC._2022.Entry.Current();

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