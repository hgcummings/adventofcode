using System.Collections.Concurrent;
using System.Drawing;
using HGC.AOC.Common;

namespace HGC.AOC._2022._24;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt").ToList();

        var start = new Point(input[0].IndexOf(".") - 1, -1);
        var end = new Point(input[^1].IndexOf(".") - 1, input.Count - 2);

        var width = input[0].Length - 2;
        var height = input.Count - 2;
        
        var blizzards = new Dictionary<int, Dictionary<Point, short>>();
        blizzards[0] = ParseInitialBlizzards(input);
        
        var search = new PriorityQueue<Expedition, int>();
        var initialState = new Expedition
        {
            Minute = 0,
            Position = start
        };
        search.Enqueue(initialState, 0);

        var visited = new HashSet<Expedition>();
        var best = 400;//Int32.MaxValue;

        void EnqueueIfCandidate(Expedition e)
        {
            if (!visited.Contains(e))
            {
                var minTimeRemaining = Math.Abs(end.X - e.Position.X) +
                                       Math.Abs(end.Y - e.Position.Y);
                if (e.Minute + minTimeRemaining < best)
                {
                    search.Enqueue(e, (minTimeRemaining * 1000) + e.Minute);
                }
            }
        }
        
        while (search.TryDequeue(out var expedition, out _))
        {
            if (expedition.Position == end)
            {
                best = expedition.Minute;
                Console.WriteLine(best);
                continue;
            }

            if (!visited.Add(expedition))
            {
                continue;
            }
            
            if (GetBlizzards(expedition.Minute).ContainsKey(expedition.Position))
            {
                continue;
            }

            EnqueueIfCandidate(expedition with { Minute = expedition.Minute + 1});
            if (expedition.Position == start)
            {
                EnqueueIfCandidate(new Expedition
                {
                    Minute = expedition.Minute + 1,
                    Position = expedition.Position with { Y = expedition.Position.Y + 1 }
                });
            }
            else
            {
                if (expedition.Position.X > 0)
                {
                    EnqueueIfCandidate(new Expedition
                    {
                        Minute = expedition.Minute + 1,
                        Position = expedition.Position with { X = expedition.Position.X - 1 }
                    });
                }
                if (expedition.Position.Y > 0)
                {
                    EnqueueIfCandidate(new Expedition
                    {
                        Minute = expedition.Minute + 1,
                        Position = expedition.Position with { Y = expedition.Position.Y - 1 }
                    });
                }
                if (expedition.Position.X < width - 1)
                {
                    EnqueueIfCandidate(new Expedition
                    {
                        Minute = expedition.Minute + 1,
                        Position = expedition.Position with { X = expedition.Position.X + 1 }
                    });
                }
                if (expedition.Position.Y < height - 1 || expedition.Position.X == end.X)
                {
                    EnqueueIfCandidate(new Expedition
                    {
                        Minute = expedition.Minute + 1,
                        Position = expedition.Position with { Y = expedition.Position.Y + 1 }
                    });
                }
            }

            if (visited.Count % 1000000 == 0)
            {
                Console.WriteLine($"Visited {visited.Count}. Queued {search.Count}. Position {expedition.Position.X},{expedition.Position.Y}. Time {expedition.Minute} (best {best})");
            }
        }

        Dictionary<Point, short> GetBlizzards(int minute)
        {
            while (!blizzards.ContainsKey(minute))
            {
                var latest = blizzards.Keys.Max();
                blizzards[latest + 1] = GenerateNextBlizzards(blizzards[latest], width, height);
            }

            return blizzards[minute];
        }



        return best;
    }

    private Dictionary<Point,short> GenerateNextBlizzards(Dictionary<Point,short> last, int width, int height)
    {
        var next = new Dictionary<Point, short>();
        foreach (var entry in last)
        {
            var p = entry.Key;
            for (short d = 1; d <= 8; d *= 2)
            {
                if ((d & entry.Value) != 0)
                {
                    var position = new Point
                    {
                        X = (p.X + d switch { 1 => 1, 4 => -1, _ => 0 } + width) % width,
                        Y = (p.Y + d switch { 2 => 1, 8 => -1, _ => 0 } + height) % height
                    };
                    if (next.ContainsKey(position))
                    {
                        next[position] += d;
                    }
                    else
                    {
                        next[position] = d;
                    }
                }
            }
        }

        return next;
    }

    private static Dictionary<Point, short> ParseInitialBlizzards(List<string> input)
    {
        var blizzards = new Dictionary<Point, short>();

        for (var i = 1; i < input.Count - 1; ++i)
        {
            for (var j = 1; j < input[i].Length - 1; ++j)
            {
                if (input[i][j] != '.')
                {
                    blizzards.Add(
                        new Point(j - 1, i - 1),
                        input[i][j] switch { '>' => 1, 'v' => 2, '<' => 4, '^' => 8 }
                    );
                }
            }
        }

        return blizzards;
    }

    struct Expedition : IEquatable<Expedition>
    {
        public bool Equals(Expedition other)
        {
            return Minute == other.Minute && Position.Equals(other.Position);
        }

        public override bool Equals(object? obj)
        {
            return obj is Expedition other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Minute, Position);
        }

        public int Minute;
        public Point Position;
    }
}