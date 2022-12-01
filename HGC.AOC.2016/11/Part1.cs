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
            return x.GeneratorPositions.SequenceEqual(y.GeneratorPositions) &&
                   x.MicrochipPositions.SequenceEqual(x.MicrochipPositions);
        }

        public int GetHashCode(State obj)
        {
            return HashCode.Combine(obj.GeneratorPositions[0], obj.MicrochipPositions[0]);
        }
    }

    public static IEqualityComparer<State> StateComparer { get; } = new StateEqualityComparer();

    public int[] GeneratorPositions;
    public int[] MicrochipPositions;

    public State(int[] generatorPositions, int[] microchipPositions)
    {
        GeneratorPositions = generatorPositions;
        MicrochipPositions = microchipPositions;
    }
}

public class Part1 : ISolution
{
    public string? Answer()
    {
        return null;
    }
}