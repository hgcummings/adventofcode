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
        public int[] Groups { get; }

        public Record(string springs, int[] groups)
        {
            Springs = springs;
            Groups = groups;
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

    int PossibleArrangements(Record record)
    {
        return PossibleArrangements(record.Springs, record.Groups);
    }
    
    int PossibleArrangements(ReadOnlySpan<char> springs, ReadOnlySpan<int> groups)
    {
        if (groups.Length == 0)
        {
            return springs.Contains('#') ? 0 : 1;
        }

        var size = groups[0];
        var total = 0;
        for (var i = 0; i <= springs.Length - size; ++i)
        {
            if (springs[i] == '.')
            {
                continue;
            }

            var canFit = true;
            for (var j = 0; j < size; ++j)
            {
                if (springs[i + j] == '.')
                {
                    canFit = false;
                    break;
                }
            }

            if (canFit)
            {
                if (i + size < springs.Length)
                {
                    if (springs[i + size] != '#')
                    {
                        total += PossibleArrangements(
                            springs[(i + size + 1)..],
                            groups[1..]);
                    }
                }
                else
                {
                    total += groups.Length == 1 ? 1 : 0;
                }
            }

            if (springs[i] == '#')
            {
                break;
            }
        }

        return total;
    }
}