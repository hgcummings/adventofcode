using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2022._19;

public class Part2 : ISolution
{
    private const int MaxTime = 32;
    
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var blueprintRegex = new Regex(
            "Blueprint (?'Id'[0-9]+): " +
            "Each ore robot costs (?'OreCostOre'[0-9]+) ore. " +
            "Each clay robot costs (?'ClayCostOre'[0-9]+) ore. " +
            "Each obsidian robot costs (?'ObsidianCostOre'[0-9]+) ore and (?'ObsidianCostClay'[0-9]+) clay. " +
            "Each geode robot costs (?'GeodeCostOre'[0-9]+) ore and (?'GeodeCostObsidian'[0-9]+) obsidian.");

        var blueprints = input.Select(line => new Blueprint(blueprintRegex.Match(line).Parse<BlueprintData>()));

        var total = 1;
        foreach (var blueprint in blueprints.Take(3))
        {
            var initialState = new State
            {
                Resources = new Amounts(),
                Robots = new Amounts { Ore = 1 },
                Time = 0
            };

            var best = 0;
            var search = new Stack<State>();
            search.Push(initialState);

            var maxUseable = new Amounts
            {
                Ore = blueprint.RobotCosts.Values.Max(c => c.Ore),
                Clay = blueprint.RobotCosts.Values.Max(c => c.Clay),
                Obsidian = blueprint.RobotCosts.Values.Max(c => c.Obsidian),
                Geode = Int32.MaxValue
            };

            var visited = new HashSet<State>();
            
            while (search.TryPop(out var state))
            {
                if (!visited.Add(state))
                {
                    continue;
                }
                
                var nextOptions = Enum.GetValues<ResourceType>().Where(rt =>
                    (state.Robots[rt] < maxUseable[rt]) &&
                    (blueprint.RobotCosts[rt].Clay == 0 || state.Robots.Clay > 0) &&
                    (blueprint.RobotCosts[rt].Obsidian == 0 || state.Robots.Obsidian > 0));

                var foundOptions = false;
                foreach (var build in nextOptions)
                {
                    var timeUntilBuild = 1 + Enum.GetValues<ResourceType>().Max(
                        cost => (blueprint.RobotCosts[build][cost] - state.Resources[cost]) <= 0
                            ? 0
                            : ((blueprint.RobotCosts[build][cost] - state.Resources[cost] - 1) / state.Robots[cost]) + 1);

                    if (timeUntilBuild + state.Time < MaxTime)
                    {
                        foundOptions = true;
                        search.Push(new State
                        {
                            Resources = new Amounts
                            {
                                Ore = state.Resources.Ore - 
                                      blueprint.RobotCosts[build][ResourceType.Ore] +
                                      timeUntilBuild * state.Robots.Ore,
                                Clay = state.Resources.Clay - 
                                       blueprint.RobotCosts[build][ResourceType.Clay] +
                                       timeUntilBuild * state.Robots.Clay,
                                Obsidian = state.Resources.Obsidian -
                                           blueprint.RobotCosts[build][ResourceType.Obsidian] +
                                           timeUntilBuild * state.Robots.Obsidian,
                                Geode = state.Resources.Geode - 
                                        blueprint.RobotCosts[build][ResourceType.Geode] +
                                        timeUntilBuild * state.Robots.Geode
                            },
                            Robots = new Amounts
                            {
                                Ore = state.Robots.Ore + (build == ResourceType.Ore ? 1 : 0),
                                Clay = state.Robots.Clay + (build == ResourceType.Clay ? 1 : 0),
                                Obsidian = state.Robots.Obsidian + (build == ResourceType.Obsidian ? 1 : 0),
                                Geode = state.Robots.Geode + (build == ResourceType.Geode ? 1 : 0)
                            },
                            Time = state.Time + timeUntilBuild
                        });
                    }
                }

                if (!foundOptions)
                {
                    var geodeOutput = state.Resources.Geode + ((MaxTime - state.Time) * state.Robots.Geode);
                    best = Math.Max(best, geodeOutput);
                }

                if (visited.Count % 100000 == 0)
                {
                    Console.WriteLine($"Visited {visited.Count}. Queued {search.Count}. Best {best}.");
                }
            }

            Console.WriteLine($"{blueprint.Id}: {best}");
            total *= best;
        }

        return total;
    }

    public struct Amounts : IEquatable<Amounts>
    {
        public bool Equals(Amounts other)
        {
            return Ore == other.Ore && Clay == other.Clay && Obsidian == other.Obsidian && Geode == other.Geode;
        }

        public override bool Equals(object? obj)
        {
            return obj is Amounts other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Ore, Clay, Obsidian, Geode);
        }

        public int this[ResourceType rt] => rt switch
        {
            ResourceType.Ore => this.Ore,
            ResourceType.Clay => this.Clay,
            ResourceType.Obsidian => this.Obsidian,
            ResourceType.Geode => this.Geode
        };

        public int Ore;
        public int Clay;
        public int Obsidian;
        public int Geode;
    }

    public struct State : IEquatable<State>
    {
        public bool Equals(State other)
        {
            return Robots.Equals(other.Robots) && Resources.Equals(other.Resources) && Time == other.Time;
        }

        public override bool Equals(object? obj)
        {
            return obj is State other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Robots, Resources, Time);
        }

        public Amounts Robots;
        public Amounts Resources;

        public int Time;
    }

    public enum ResourceType
    {
        Ore = 0,
        Clay = 1,
        Obsidian = 2,
        Geode = 3
    }

    public class Blueprint
    {
        public int Id { get; }
        public Dictionary<ResourceType, Amounts> RobotCosts;

        public Blueprint(BlueprintData data)
        {
            Id = data.Id;
            RobotCosts = new Dictionary<ResourceType, Amounts>();
            RobotCosts[ResourceType.Ore] = new Amounts { Ore = data.OreCostOre };
            RobotCosts[ResourceType.Clay] = new Amounts { Ore = data.ClayCostOre };
            RobotCosts[ResourceType.Obsidian] = new Amounts { Ore = data.ObsidianCostOre, Clay = data.ObsidianCostClay };
            RobotCosts[ResourceType.Geode] = new Amounts { Ore = data.GeodeCostOre, Obsidian = data.GeodeCostObsidian };
        }
    }


    public class BlueprintData
    {
        public int Id { get; set; }
        public int OreCostOre { get; set; }
        public int ClayCostOre { get; set; }
        public int ObsidianCostOre { get; set; }
        public int ObsidianCostClay { get; set; }
        public int GeodeCostOre { get; set; }
        public int GeodeCostObsidian { get; set; }
    }
}