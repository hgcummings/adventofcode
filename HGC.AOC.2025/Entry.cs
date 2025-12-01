using HGC.AOC.Common;

namespace HGC.AOC._2025;

public class Entry
{
    public static ISolution Current()
    {
        return SolutionResolver.GetCurrentSolution();
    }

    public static ISolution DayPart(int day, int part)
    {
        return SolutionResolver.GetDayPartSolution(day, part);
    }
}