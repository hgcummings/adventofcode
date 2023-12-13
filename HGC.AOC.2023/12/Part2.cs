using System.Diagnostics;
using HGC.AOC.Common;

namespace HGC.AOC._2023._12;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var completion = 0;
        return this.ReadInputLines("input.txt")
            .Select(line => new Record(line))
            .Select(record => new Record(
                String.Join("?", Enumerable.Repeat(record.Springs, 5)),
                String.Join(",", Enumerable.Repeat(String.Join(",", record.Groups), 5))))
             .AsParallel()
             .Sum(record =>
             {
                 var stopwatch = new Stopwatch();
                 stopwatch.Start();
                 var count = PossibleArrangements(record.Springs.AsSpan(), record.Groups);
                 stopwatch.Stop();
                 Console.Write(Interlocked.Increment(ref completion));
                 Console.Write($" ({count} in {stopwatch.ElapsedMilliseconds/1000}s) ");
                 Console.Write(record.Springs.Substring(0, record.Springs.Length / 5) + " ");
                 Console.WriteLine(String.Join(",", record.Groups.Take(record.Groups.Length / 5)));
                 return count;
             });
    }

     struct Record
     {
         public string Springs { get; }
         public int[] Groups { get; }
         
         public Record(string springs, string groups)
         {
             Springs = springs;
             Groups = groups.Split(",").Select(Int32.Parse).ToArray();
         }
         
         public Record(string line) {
             var components = line.SplitBySpaces();
             var springs = components[0];
             var groups = components[1].Split(",").Select(Int32.Parse).ToArray();
         
             Springs = springs;
             Groups = groups;
         }

         public override string ToString()
         {
             return $"{Springs} {String.Join(",", Groups)}";
         }
     }

     long PossibleArrangements(ReadOnlySpan<char> sp, ReadOnlySpan<int> groups)
     {
         if (groups.Length == 0)
         {
             return sp.Contains('#') ? 0 : 1;
         }

         var size = groups[0];
         long total = 0;
         for (var i = 0; i <= sp.Length - size; ++i)
         {
             if (sp[i] == '.')
             {
                 continue;
             }

             var canFit = true;
             for (var j = 0; j < size; ++j)
             {
                 if (sp[i + j] == '.')
                 {
                     canFit = false;
                     break;
                 }
             }

             if (canFit)
             {
                 if (i + size < sp.Length)
                 {
                     if (sp[i + size] != '#')
                     {
                         total += PossibleArrangements(
                             sp[(i + size + 1)..],
                             groups[1..]);
                     }
                 }
                 else
                 {
                     total += groups.Length == 1 ? 1 : 0;
                 }
             }

             if (sp[i] == '#')
             {
                 break;
             }
         }

         return total;
     }
}