using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2025._10;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var machines = this.ReadInputLines("input.txt").Select(Machine.Parse);

        return machines.Sum(Solve);
    }

    public int Solve(Machine machine)
    {
        var dist = new Dictionary<long, int>
        {
            [0] = 0,
            [machine.Target] = Int32.MaxValue
        };

        var queue = new Queue<long>();
        queue.Enqueue(0);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            if (dist[node] >= dist[machine.Target] - 1)
            {
                continue;
            }
            
            foreach (var button in machine.Buttons)
            {
                var next = node ^ button;
                
                if (dist.ContainsKey(next) && dist[next] < dist[node] + 1)
                {
                    continue;
                }

                dist[next] = dist[node] + 1;
                queue.Enqueue(next);
            }
        }

        return dist[machine.Target];
    }

    public struct Machine
    {
        public long Target;
        public List<long> Buttons;
        public List<int> Joltages;

        public static Machine Parse(string input)
        {
            var parts = input.Split(' ');
            var target = ParseTarget(parts[0]);
            var buttons = parts.Skip(1).Take(parts.Length - 2).Select(ParseButton).ToList();
            var joltages = ParseDigits(parts[^1].Trim('{', '}')).ToList();
            return new Machine { Target = target, Buttons = buttons, Joltages = joltages };
        }

        private static long ParseTarget(string input)
        {
            var pattern = input.Trim('[', ']');
            return Enumerable.Range(0, pattern.Length)
                .Sum(i => pattern[i] == '#' ? 1 << i : 0);
        }

        private static long ParseButton(string input)
        {
            return ParseDigits(input.Trim('(', ')'))
                .Aggregate(0L, (acc, cur) => acc + (1 << cur));
        }

        private static IEnumerable<int> ParseDigits(string input)
        {
            return input.Split(',').Select(Int32.Parse);
        }
    }
}