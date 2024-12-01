using System.Collections;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2016._11;

public struct State
{
    private sealed class StateEqualityComparer : IEqualityComparer<State>
    {
        public bool Equals(State x, State y)
        {
            return x.ElevatorPosition == y.ElevatorPosition &&
                   x.GeneratorPositions.SequenceEqual(y.GeneratorPositions) &&
                   x.MicrochipPositions.SequenceEqual(x.MicrochipPositions);
        }

        public int GetHashCode(State obj)
        {
            return HashCode.Combine(
                obj.ElevatorPosition,
                obj.GeneratorPositions[0],
                obj.MicrochipPositions[0]);
        }
    }

    public static IEqualityComparer<State> StateComparer { get; } = new StateEqualityComparer();

    public int ElevatorPosition;
    public int[] GeneratorPositions;
    public int[] MicrochipPositions;

    public State(int elevatorPosition, int[] generatorPositions, int[] microchipPositions)
    {
        ElevatorPosition = elevatorPosition;
        GeneratorPositions = generatorPositions;
        MicrochipPositions = microchipPositions;
    }
}

public class Part1 : ISolution
{
    public object? Answer()
    {
        string[] elements = new[]
        {
            "polonium",
            "thulium",
            "promethium",
            "ruthenium",
            "cobalt",
            "polonium",
            "promethium"
        };

        var initialState = new State(1, new int[elements.Length], new int[elements.Length]);
        var floor = 1;
        foreach (var line in this.ReadInputLines("input.txt"))
        {
            for (var i = 0; i < elements.Length; ++i)
            {
                var element = elements[i];
                if (line.Contains($"{element} generator"))
                {
                    initialState.GeneratorPositions[i] = floor;
                }

                if (line.Contains($"{element}-compatible microchip"))
                {
                    initialState.MicrochipPositions[i] = floor;
                }
            }
            
            ++floor;
        }

        return null;
    }
}