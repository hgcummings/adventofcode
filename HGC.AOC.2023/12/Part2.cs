using System.Collections.Concurrent;
using System.Diagnostics;
using HGC.AOC.Common;

namespace HGC.AOC._2023._12;

public class Part2 : ISolution
{
    public object? Answer()
    {
        return this.ReadInputLines("input.txt")
            .Select(line => new Record(line))
            .Select(record => new Record(
                String.Join("?", Enumerable.Repeat(record.Springs, 5)),
                String.Join(",", Enumerable.Repeat(String.Join(",", record.Groups), 5))))
            .AsParallel()
            .Sum(record => record.PossibleArrangements());
    }

    class Record
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

        public long PossibleArrangements()
        {
            return PossibleArrangements(0, 0);
        }

        private readonly ConcurrentDictionary<(int sOff, int gOff), long> cache = new();

        long PossibleArrangements(int springsOffset, int groupsOffset)
        {
            return cache.GetOrAdd(
                (springsOffset, groupsOffset),
                key => PossibleArrangementsRaw(key.sOff, key.gOff));
        }
         
        long PossibleArrangementsRaw(int springsOffset, int groupsOffset)
        {
            var sp = Springs[springsOffset..];
            var groups = Groups[groupsOffset..];
              
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
                                springsOffset + i + size + 1,
                                groupsOffset + 1);
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

     
}