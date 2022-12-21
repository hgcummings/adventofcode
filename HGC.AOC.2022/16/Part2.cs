using System.Text.RegularExpressions;
using HGC.AOC.Common;
using Combinatorics.Collections;

namespace HGC.AOC._2022._16;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var valves = ParseInput("input.txt");

        var startingValve = valves.Single(v => v.Id == "AA");
        var flowValves = valves.Where(v => v.FlowRate > 0).ToList();

        var shortestPaths = CalculateShortestPaths(startingValve, flowValves);
        
        return FindPermutationSolution(startingValve, flowValves, shortestPaths);
    }

    private static int FindPermutationSolution(
        Valve startingValve,
        IList<Valve> flowValves,
        Dictionary<string, Dictionary<string, int>> shortestPaths)
    {
        var unvisitedValves = new List<Valve>(flowValves);

        var orderedValves = unvisitedValves.OrderByDescending(v => v.FlowRate).ToList();
        Valve[] startingPermutation = 
            orderedValves.Where((_, i) => i % 2 == 0).Concat(orderedValves.Where((_, i) => i % 2 == 1)).ToArray();

        var currentCandidates = new List<Valve[]> { startingPermutation };
        var converged = false;
        var previousBest = 0;

        do
        {
            var nextCandidates = new List<Valve[]>(currentCandidates);

            foreach (var currentCandidate in currentCandidates)
            {
                for (var i = 0; i < currentCandidate.Length; ++i)
                {
                    for (var j = i + 1; j < currentCandidate.Length; ++j)
                    {
                        var candidatePermutation = new List<Valve>(currentCandidate)
                        {
                            [i] = currentCandidate[j],
                            [j] = currentCandidate[i]
                        };
                        nextCandidates.Add(candidatePermutation.ToArray());
                    }
                }
            }

            currentCandidates = nextCandidates
                .OrderByDescending(c => CalculateTotalFlow(startingValve, c, shortestPaths))
                .Take(128)
                .ToList();

            var best = CalculateTotalFlow(startingValve, nextCandidates[0], shortestPaths);
            if (best == previousBest)
            {
                converged = true;
            }
            else
            {
                previousBest = best;
            }
        } while (!converged);

        return previousBest;
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

    private static int CalculateTotalFlow(Valve startingValve, Valve[] permutation, Dictionary<string, Dictionary<string, int>> shortestPaths)
    {
        var remainingMinutes = 26;
        var currentValve = startingValve;

        var bestFlow = 0;
        for (var i = 1; i < permutation.Length; ++i)
        {
            var actor1Flow = CalculatePartialFlow(permutation[..i], shortestPaths, remainingMinutes, currentValve);
            var actor2Flow = CalculatePartialFlow(permutation[i..], shortestPaths, remainingMinutes, currentValve);
            var totalFlow = actor1Flow + actor2Flow;
            bestFlow = Math.Max(bestFlow, totalFlow);
        }

        return bestFlow;
    }

    private static int CalculatePartialFlow(
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

        return totalFlow;
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