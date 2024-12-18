using System.Collections.Immutable;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2016._11;

public struct State
{
    public bool Equals(State other)
    {
        return CurrentFloor == other.CurrentFloor &&
               TopFloor == other.TopFloor &&
               ElementStates().SequenceEqual(other.ElementStates());
    }

    private IEnumerable<int> ElementStates()
    {
        var self = this;
        return Enumerable.Range(0, Elements.Count)
            .Select(i => 4 * self.GeneratorPositions[i] + self.MicrochipPositions[i])
            .OrderBy(n => n);
    }

    public override bool Equals(object? obj)
    {
        return obj is State other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(CurrentFloor, TopFloor, ElementStates().Sum());
    }

    public static readonly List<string> Elements = new(); 
    
    public readonly int CurrentFloor;
    public readonly int TopFloor;
    public readonly ImmutableList<int> GeneratorPositions;
    public readonly ImmutableList<int> MicrochipPositions;

    public State(
        int currentFloor,
        int topFloor,
        ImmutableList<int> generatorPositions,
        ImmutableList<int> microchipPositions)
    {
        CurrentFloor = currentFloor;
        TopFloor = topFloor;
        GeneratorPositions = generatorPositions;
        MicrochipPositions = microchipPositions;
    }

    public void Print()
    {
        for (var floor = TopFloor; floor > 0; --floor)
        {
            Console.Write($"F{floor} ");
            Console.Write(CurrentFloor == floor ? "E  " : ".  ");

            for (var i = 0; i < Elements.Count; ++i)
            {
                var letter = Elements[i].ToUpper()[..1];
                Console.Write(GeneratorPositions[i] == floor ? letter + "G " : ".  ");
                Console.Write(MicrochipPositions[i] == floor ? letter + "M " : ".  ");
            }
            Console.WriteLine();
        }

        Console.WriteLine();
    }

    public bool IsTarget()
    {
        return MicrochipPositions.All(m => m == 4) && GeneratorPositions.All(g => g == 4);
    }
    
    public bool IsSafe()
    {
        for (var m = 0; m < MicrochipPositions.Count; ++m)
        {
            var floor = MicrochipPositions[m];
            if (GeneratorPositions[m] != floor && GeneratorPositions.Contains(floor))
            {
                return false;
            }
        }

        return true;
    }

    public IEnumerable<State> Neighbours()
    {
        foreach (var nextFloor in NeighbouringFloors())
        {
            for (var g = 0; g < GeneratorPositions.Count; ++g)
            {
                if (GeneratorPositions[g] == CurrentFloor)
                {
                    var nextGeneratorPositions = GeneratorPositions.SetItem(g, nextFloor);
                    yield return new State(
                        nextFloor, TopFloor, nextGeneratorPositions, MicrochipPositions);
                    var microchipMove = new State(
                        CurrentFloor, TopFloor, nextGeneratorPositions, MicrochipPositions);
                    foreach (var twoItemMoves in microchipMove.SingleItemMoves(nextFloor))
                    {
                        yield return twoItemMoves;
                    }
                }
            }
            
            for (var m = 0; m < GeneratorPositions.Count; ++m)
            {
                if (MicrochipPositions[m] == CurrentFloor)
                {
                    var nextMicrochipPositions = MicrochipPositions.SetItem(m, nextFloor);
                    yield return new State(
                        nextFloor, TopFloor, GeneratorPositions, nextMicrochipPositions);
                    var microchipMove = new State(
                        CurrentFloor, TopFloor, GeneratorPositions, nextMicrochipPositions);
                    foreach (var twoItemMoves in microchipMove.SingleItemMoves(nextFloor))
                    {
                        yield return twoItemMoves;
                    }
                }
            }
        }
    }

    private IEnumerable<State> SingleItemMoves(int nextFloor)
    {
        for (var g = 0; g < GeneratorPositions.Count; ++g)
        {
            if (GeneratorPositions[g] == CurrentFloor)
            {
                yield return new State(nextFloor, TopFloor,
                    GeneratorPositions.SetItem(g, nextFloor), MicrochipPositions);
            }
        }
        for (var m = 0; m < MicrochipPositions.Count; ++m)
        {
            if (MicrochipPositions[m] == CurrentFloor)
            {
                yield return new State(nextFloor, TopFloor,
                    GeneratorPositions, MicrochipPositions.SetItem(m, nextFloor));
            }
        }
    }

    private IEnumerable<int> NeighbouringFloors()
    {
        if (CurrentFloor < TopFloor) yield return CurrentFloor + 1;
        if (CurrentFloor > 1) yield return CurrentFloor - 1;
    }
}

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInput("input.txt");
        foreach (Match match in (new Regex("(?'element'[a-z]+) generator").Matches(input)))
        {
            State.Elements.Add(match.Groups["element"].Value);
        }
        
        var initialGeneratorPositions = new int[State.Elements.Count];
        var initialMicrochipPositions = new int[State.Elements.Count];
        var floor = 1;
        
        foreach (var line in this.ReadInputLines("input.txt"))
        {
            for (var i = 0; i < State.Elements.Count; ++i)
            {
                var element = State.Elements[i];
                if (line.Contains($"{element} generator"))
                {
                    initialGeneratorPositions[i] = floor;
                }

                if (line.Contains($"{element}-compatible microchip"))
                {
                    initialMicrochipPositions[i] = floor;
                }
            }

            floor++;
        }

        var initialState = new State(
            1,
            floor - 1,
            initialGeneratorPositions.ToImmutableList(),
            initialMicrochipPositions.ToImmutableList());

        var openSet = new PriorityQueue<State, int>();
        var cameFrom = new Dictionary<State, State>();

        openSet.Enqueue(initialState, Int32.MaxValue);
        
        var gScore = new Dictionary<State, int>();
        gScore[initialState] = 0;

        int H(State state)
        {
            return state.GeneratorPositions.Concat(state.MicrochipPositions)
                .Sum(f => 4 - f) / 2;
        }

        var step = 0;
        while (openSet.Count != 0)
        {
            var current = openSet.Dequeue();
            
            if (++step % 1000 == 0)
            {
                Console.WriteLine($"{step} ({openSet.Count}) H={H(current)}");
                current.Print();
            }

            if (current.IsTarget())
            {
                var path = new List<State>();
                path.Add(current);
                while (cameFrom.ContainsKey(current))
                {
                    current = cameFrom[current];
                    path.Add(current);
                }

                return path.Count - 1;
            }

            foreach (var neighbour in current.Neighbours().Where(n => n.IsSafe()).Distinct())
            {
                var tentativeGScore = gScore[current] + 1;
                if (tentativeGScore < gScore.GetValueOrDefault(neighbour, Int32.MaxValue))
                {
                    cameFrom[neighbour] = current;
                    gScore[neighbour] = tentativeGScore;
                    openSet.Enqueue(neighbour, tentativeGScore + H(neighbour));
                }
            }
        }
        
        return null;
    }
}