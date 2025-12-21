using HGC.AOC.Common;
using Combinatorics.Collections;

namespace HGC.AOC._2025._10;

public class Part2 : ISolution
{
    public int Count;
    
    public object? Answer()
    {
        var machines = this.ReadInputLines("input.txt").Select(Machine.Parse);
        return machines.AsParallel().Sum(Solve);
    }

    public int Solve(Machine machine)
    {
        var result = SolveRecursive(machine.Buttons, machine.Target)!.Value;
        Console.WriteLine(Interlocked.Increment(ref Count));
        return result;
    }
    
    public int? SolveRecursive(List<List<int>> buttons, int[] target)
    {
        if (target.All(v => v == 0))
        {
            return 0;
        }
    
        if (buttons.Count == 0)
        {
            return null;
        }
    
        if (target.Where((t, i) => t > 0 && !buttons.Any(button => button.Contains(i))).Any())
        {
            return null;
        }
        
        var dimension = buttons.SelectMany(i => i).Distinct()
            .OrderBy(i => buttons.Count(b => b.Contains(i)))
            .ThenBy(i => target[i])
            .First();
        
        var buttonIndices = buttons
            .Select((b, i) => new { b, i })
            .Where(bi => bi.b.Contains(dimension))
            .Select(bi => bi.i)
            .ToList();
    
        var combinations = new Combinations<int>(
            buttonIndices, target[dimension], GenerateOption.WithRepetition);
    
        var newButtons = buttons.ToList();
        foreach (var buttonIndex in buttonIndices.OrderDescending())
        {
            newButtons.RemoveAt(buttonIndex);
        }

        return target[dimension] + combinations.Min(c =>
        {
            var newTarget = target.ToArray();
            foreach (var buttonIndex in c)
            {
                foreach (var targetIndex in buttons[buttonIndex])
                {
                    newTarget[targetIndex] -= 1;
                }
            }

            if (newTarget.Any(v => v < 0))
            {
                return null;
            }

            return SolveRecursive(newButtons, newTarget);
        });
    }

    public struct Machine(int[] target, List<List<int>> buttons)
    {
        public int[] Target = target;
        public List<List<int>> Buttons = buttons;

        public static Machine Parse(string input)
        {
            var parts = input.Split(' ');
            var buttons = parts.Skip(1).Take(parts.Length - 2).Select(ParseButton)
                .OrderByDescending(button => button.Count).ToList();
            var target = ParseDigits(parts[^1].Trim('{', '}')).ToArray();
            return new Machine { Target = target, Buttons = buttons };
        }

        private static List<int> ParseButton(string input)
        {
            return ParseDigits(input.Trim('(', ')')).ToList();
        }

        private static IEnumerable<int> ParseDigits(string input)
        {
            return input.Split(',').Select(Int32.Parse);
        }
    }
}