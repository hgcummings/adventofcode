using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2025._11;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var connections = new Dictionary<string, List<string>>();

        foreach (var line in this.ReadInputLines())
        {
            var parts = line.
                Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            
            connections.Add(parts[0].Substring(0, parts[0].IndexOf(':')), parts.Skip(1).ToList());
        }

        
        int RoutesFrom(string start)
        {
            if (start == "out")
            {
                return 1;
            }

            return connections[start].Sum(RoutesFrom);
        }
        
        return RoutesFrom("you");
    }

}