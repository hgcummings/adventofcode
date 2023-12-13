using HGC.AOC.Common;

namespace HGC.AOC._2023._12;

public class Part1 : ISolution
{
    public object? Answer()
    {
        return this.ReadInputLines("input.txt")
            .Select(line => new Record(line))
            .Sum(PossibleArrangements);
    }

     struct Record
     {
         public string Springs { get; }
         public List<int> Groups { get; }

         public Record(string springs, List<int> groups)
         {
             Springs = springs;
             Groups = groups;
         }
         
         public Record(string line) {
             var components = line.SplitBySpaces();
             var springs = components[0];
             var groups = components[1].Split(",").Select(Int32.Parse).ToList();
         
             Springs = springs;
             Groups = groups;
         }

         public override string ToString()
         {
             return $"{Springs} {String.Join(",", Groups)}";
         }
     }

     int PossibleArrangements(Record record)
     {
         if (record.Groups.Count == 0)
         {
             return record.Springs.Any(c => c == '#') ? 0 : 1;
         }

         var size = record.Groups[0];
         var sp = record.Springs;
         var total = 0;
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
                         total += PossibleArrangements(new Record(
                             sp.Substring(i + size + 1),
                             record.Groups.Skip(1).ToList()));
                     }
                 }
                 else
                 {
                     total += record.Groups.Count == 1 ? 1 : 0;
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