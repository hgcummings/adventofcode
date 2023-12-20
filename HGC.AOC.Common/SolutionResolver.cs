using System.Reflection;

namespace HGC.AOC.Common;

public static class SolutionResolver
{
    public static ISolution GetCurrentSolution()
    {
        Type solutionClass = Assembly.GetCallingAssembly().GetTypes()
            .Where(type => type.IsAssignableTo(typeof (ISolution)))
            .OrderByDescending(type => type.FullName)
            .First();

        return (ISolution) Activator.CreateInstance(solutionClass)!;
    }

    public static ISolution GetDayPartSolution(int day, int part)
    {
        Type solutionClass = Assembly.GetCallingAssembly().GetTypes()
            .Single(type => type.IsAssignableTo(typeof (ISolution)) &&
                            type.Namespace!.EndsWith($"_{day:00}") &&
                            type.Name == $"Part{part}");

        return (ISolution) Activator.CreateInstance(solutionClass)!;
    }
}