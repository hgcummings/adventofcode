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
}