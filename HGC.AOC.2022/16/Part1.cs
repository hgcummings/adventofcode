using System.Text.RegularExpressions;
using HGC.AOC.Common;
using Combinatorics.Collections;

namespace HGC.AOC._2022._16;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var valves = ParseInput("input.txt");

        var startingValve = valves.Single(v => v.Id == "AA");
        var flowValves = valves.Where(v => v.FlowRate > 0).ToList();

        var shortestPaths = CalculateShortestPaths(startingValve, flowValves);

        return FindPermutationSolution(startingValve, flowValves, shortestPaths);
        // return FindBruteForceSolution(startingValve, flowValves, shortestPaths);
    }

    private static int FindPermutationSolution(
        Valve startingValve,
        IList<Valve> flowValves,
        Dictionary<string, Dictionary<string, int>> shortestPaths)
    {
        var unvisitedValves = new List<Valve>(flowValves);

        IReadOnlyList<Valve> currentPermutation = unvisitedValves.OrderByDescending(v => v.FlowRate).ToArray();
        var bestScore = CalculateTotalFlow(startingValve, currentPermutation, shortestPaths);

        bool improved;
        do
        {
            Console.WriteLine(String.Join(", ", currentPermutation.Select(v => v.Id)));
            var bestPermutation = currentPermutation;
            improved = false;
            for (var i = 0; i < currentPermutation.Count; ++i)
            {
                for (var j = i + 1; j < currentPermutation.Count; ++j)
                {
                    var candidatePermutation = new List<Valve>(currentPermutation)
                    {
                        [i] = currentPermutation[j],
                        [j] = currentPermutation[i]
                    };
                    var candidateScore = CalculateTotalFlow(startingValve, candidatePermutation, shortestPaths);
                    if (candidateScore > bestScore)
                    {
                        bestPermutation = candidatePermutation;
                        bestScore = candidateScore;
                        improved = true;
                    }
                }

                
            }

            currentPermutation = bestPermutation;
        } while (improved);
        
        Console.WriteLine(String.Join(", ", currentPermutation.Select(v => v.Id)));
        return bestScore;
    }

    private List<Valve> ParseInput(string inputFile)
    {
        var input = this.ReadInputLines(inputFile);

        var valveRegex =
            new Regex(
                "Valve (?'Id'[A-Z]+) has flow rate=(?'FlowRate'[0-9]+); tunnels? leads? to valves? (?'Neighbours'[A-Z, ]+)");

        var valveData = input.Select(line => valveRegex.Match(line).Parse<ValveData>()).ToList();
        var valves = new List<Valve>();

        foreach (var data in valveData)
        {
            valves.Add(new Valve(data.Id, data.FlowRate));
        }

        foreach (var data in valveData)
        {
            var valve = valves.Single(v => v.Id == data.Id);
            foreach (var neighbourId in data.Neighbours.Split(",").Select(id => id.Trim()))
            {
                valve.Neighbours.Add(valves.Single(v => v.Id == neighbourId));
            }
        }

        return valves;
    }

    private Dictionary<string, Dictionary<string, int>> CalculateShortestPaths(Valve startingValve, List<Valve> flowValves)
    {
        var shortestPaths = new Dictionary<string, Dictionary<string, int>>();

        shortestPaths[startingValve.Id] = new Dictionary<string, int>();
        foreach (var flowValve in flowValves)
        {
            shortestPaths[startingValve.Id][flowValve.Id] = FindShortestPath(startingValve, flowValve)!.Value;
            shortestPaths[flowValve.Id] = new Dictionary<string, int>();
        }

        var pairs = new Combinations<Valve>(flowValves, 2);
        foreach (var pair in pairs)
        {
            var shortestPath = FindShortestPath(pair[0], pair[1])!.Value;
            shortestPaths[pair[0].Id][pair[1].Id] = shortestPath;
            shortestPaths[pair[1].Id][pair[0].Id] = shortestPath;
        }

        return shortestPaths;
    }

    private static int FindBruteForceSolution(
        Valve startingValve,
        IEnumerable<Valve> flowValves,
        Dictionary<string, Dictionary<string, int>> shortestPaths)
    {
        var maxTotalFlow = 0;
        string? bestPath = null;

        var permutations = new Permutations<Valve>(flowValves);
        var count = 0;
        foreach (var permutation in permutations)
        {
            count++;
            if (count % 1000000 == 0)
            {
                Console.WriteLine($"{count} / {permutations.Count}");
            }

            var totalFlow = CalculateTotalFlow(startingValve, permutation, shortestPaths);

            if (totalFlow > maxTotalFlow)
            {
                maxTotalFlow = totalFlow;
                bestPath = String.Join(", ", permutation.Select(v => v.Id));
            }
        }

        Console.WriteLine(bestPath);
        return maxTotalFlow;
    }

    private static int CalculateTotalFlow(Valve startingValve, IReadOnlyList<Valve> permutation, Dictionary<string, Dictionary<string, int>> shortestPaths)
    {
        var remainingMinutes = 30;
        var currentValve = startingValve;

        var (totalFlow, _) = CalculateTotalFlow(permutation, shortestPaths, remainingMinutes, currentValve);
        return totalFlow;
    }

    private static (int, int) CalculateTotalFlow(
        IReadOnlyList<Valve> permutation,
        Dictionary<string, Dictionary<string, int>> shortestPaths,
        int remainingMinutes,
        Valve currentValve)
    {
        var totalFlow = 0;
        foreach (var valve in permutation)
        {
            remainingMinutes -= shortestPaths[currentValve.Id][valve.Id] + 1;

            if (remainingMinutes > 1)
            {
                totalFlow += valve.FlowRate * remainingMinutes;
            }
            else
            {
                break;
            }

            currentValve = valve;
        }

        return (totalFlow, remainingMinutes);
    }

    private int? FindShortestPath(Valve from, Valve to, params Valve[] visited)
    {
        if (from == to)
        {
            return 0;
        }

        var newVisited = visited.Concat(new[] { from }).ToArray();
        return from.Neighbours
            .Where(n => !visited.Contains(n))
            .Select(n => 1 + FindShortestPath(n, to, newVisited))
            .Where(l => l.HasValue)
            .OrderBy(l => l).FirstOrDefault();
    }

    private class Valve : IComparable<Valve>
    {
        public Valve(string id, int flowRate)
        {
            Id = id;
            FlowRate = flowRate;
            Neighbours = new List<Valve>();
        }
        
        public string Id { get; }
        public int FlowRate { get; }
        public List<Valve> Neighbours { get; }

        public int CompareTo(Valve? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return string.Compare(Id, other.Id, StringComparison.Ordinal);
        }
    }
    
    private class ValveData
    {
        public string Id { get; set; } = null!;
        public string Neighbours { get; set; } = null!;
        public int FlowRate { get; set; } = 0;
    }
}