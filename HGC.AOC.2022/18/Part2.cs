using System.Drawing;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2022._18;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var lava = new HashSet<Cube>(input
            .Select(line => line.Trim().Split(",").Select(Int32.Parse).ToList())
            .Select(c => new Cube { X = c[0], Y = c[1], Z= c[2] }));

        var steam = new HashSet<Cube>();
        var origin = new Cube { X = 0, Y = 0, Z = 0 };

        var fillQueue = new Queue<Cube>();
        fillQueue.Enqueue(origin);

        IEnumerable<Cube> Neighbours(Cube cube)
        {
            if (cube.X > -1) { yield return cube with { X = cube.X - 1 }; }
            if (cube.X < 20) { yield return cube with { X = cube.X + 1 }; }
            if (cube.Y > -1) { yield return cube with { Y = cube.Y - 1 }; }
            if (cube.Y < 20) { yield return cube with { Y = cube.Y + 1 }; }
            if (cube.Z > -1) { yield return cube with { Z = cube.Z - 1 }; }
            if (cube.Z < 20) { yield return cube with { Z = cube.Z + 1 }; }
        }

        var contactArea = 0;
        while (fillQueue.Count > 0)
        {
            var cube = fillQueue.Dequeue();
            if (!steam.Add(cube)) {
                continue;
            }
            foreach (var neighbour in Neighbours(cube))
            {
                if (!lava.Contains(neighbour) && !steam.Contains(neighbour))
                {
                    fillQueue.Enqueue(neighbour);
                }
                else if (lava.Contains(neighbour))
                {
                    contactArea++;
                }
            }
        }

        for (var z = 0; z <= 20; ++z)
        {
            for (var y = 0; y <= 20; ++y)
            {
                Console.WriteLine(String.Join("", Enumerable.Range(0, 21).Select(x =>
                {
                    var cube = new Cube { X = x, Y = y, Z = z };
                    if (lava.Contains(cube))
                    {
                        return '#';
                    }

                    if (steam.Contains(cube))
                    {
                        return '.';
                    }

                    return ' ';
                })));
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        return contactArea;
    }

    struct Cube
    {
        public int X;
        public int Y;
        public int Z;
    }
}