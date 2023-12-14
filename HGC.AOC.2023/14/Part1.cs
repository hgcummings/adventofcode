using HGC.AOC.Common;

namespace HGC.AOC._2023._14;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var map = this.ReadInputLines("input.txt").Select(line => line.ToArray()).ToArray();

        Tilt(map, Direction.North);

        return map.Select((row, i) => (map.Length - i) * row.Count(c => c == 'O')).Sum();
    }

    private void Tilt(char[][] map, Direction dir)
    {
        var settled = false;
        while (!settled)
        {
            switch (dir)
            {
                case(Direction.North):
                    settled = TiltNorth(map);
                    break;
                
                case(Direction.East):
                    settled = TiltEast(map);
                    break;
                
                case(Direction.South):
                    settled = TiltSouth(map);
                    break;
                
                case(Direction.West):
                    settled = TiltWest(map);
                    break;
            }
        }
    }

    enum Direction
    {
        North,
        East,
        South,
        West
    }

    bool TiltNorth(char[][] map)
    {
        return TiltVertical(map, 'O', '.');
    }

    bool TiltSouth(char[][] map)
    {
        return TiltVertical(map, '.', 'O');
    }

    bool TiltEast(char[][] map)
    {
        return TiltHorizontal(map, 'O', '.');
    }

    bool TiltWest(char[][] map)
    {
        return TiltHorizontal(map, '.', '0');
    }

    bool TiltVertical(char[][] map, char toMoveNorth, char toMoveSouth)
    {
        var settled = true;

        for (var x = 0; x < map[0].Length; ++x)
        {
            for (var y = 1; y < map.Length; ++y)
            {
                if (map[y][x] == toMoveNorth && map[y - 1][x] == toMoveSouth)
                {
                    (map[y][x], map[y - 1][x]) = (map[y - 1][x], map[y][x]);
                    settled = false;
                }
            }
        }

        return settled;
    }

    bool TiltHorizontal(char[][] map, char toMoveWest, char toMoveEast)
    {
        var settled = true;

        for (var x = 1; x < map[0].Length; ++x)
        {
            for (var y = 0; y < map.Length; ++y)
            {
                if (map[y][x] == toMoveWest && map[y][x - 1] == toMoveEast)
                {
                    (map[y][x], map[y][x - 1]) = (map[y][x - 1], map[y][x]);
                    settled = false;
                }
            }
        }

        return settled;
    }
}