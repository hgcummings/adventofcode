using HGC.AOC.Common;

namespace HGC.AOC._2023._17;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var map = this.ReadInputLines("input.txt")
            .Select(l => l.Select(c => c - 48).ToArray())
            .ToArray();

        var start = new Node(0, 0, Dir.None, 0);

        var distances = new Dictionary<Node, int>();
        distances[start] = 0;

        var queue = new PriorityQueue<Node, int>();
        queue.Enqueue(start, 0);
        
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            
            if (node.X == map[0].Length - 1 && node.Y == map.Length - 1)
            {
                return distances[node];
            }

            foreach (var neighbour in node.Neighbours(map))
            {
                var distanceViaNode = distances[node] + map[neighbour.Y][neighbour.X];
                if (distanceViaNode < distances.GetValueOrDefault(neighbour, Int32.MaxValue))
                {
                    distances[neighbour] = distanceViaNode;
                    queue.Enqueue(neighbour, distanceViaNode);
                }
            }
        }

        return null;
    }

    struct Node
    {
        public bool Equals(Node other)
        {
            return X == other.X && Y == other.Y && LastStep == other.LastStep && StepsSinceTurn == other.StepsSinceTurn;
        }

        public override bool Equals(object? obj)
        {
            return obj is Node other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, (int)LastStep, StepsSinceTurn);
        }

        public int X { get; }
        public int Y { get; }
        public Dir LastStep { get; }
        public int StepsSinceTurn { get; }

        public Node(int x, int y, Dir lastStep, int stepsSinceTurn)
        {
            X = x;
            Y = y;
            LastStep = lastStep;
            StepsSinceTurn = stepsSinceTurn;
        }

         public IEnumerable<Node> Neighbours(int[][] map)
         {
             List<Dir> allowedDirections;

             if (LastStep == Dir.None)
             {
                 allowedDirections = [Dir.Right, Dir.Down, Dir.Left, Dir.Up];
             }
             else
             {
                 allowedDirections = [Dir.Right, Dir.Down, Dir.Left, Dir.Up];
                 allowedDirections.Remove(Reverse(LastStep));
                 if (StepsSinceTurn >= 3)
                 {
                     allowedDirections.Remove(LastStep);
                 }
             }
             
             if (X < map[0].Length - 1 && allowedDirections.Contains(Dir.Right)) {
                 yield return Neighbour(X + 1, Y, Dir.Right);
             }

             if (Y < map.Length - 1 && allowedDirections.Contains(Dir.Down))
             {
                 yield return Neighbour(X, Y + 1, Dir.Down);
             }

             if (X > 0 && allowedDirections.Contains(Dir.Left))
             {
                 yield return Neighbour(X - 1, Y, Dir.Left);
             }

             if (Y > 0 && allowedDirections.Contains(Dir.Up))
             {
                 yield return Neighbour(X, Y - 1, Dir.Up);
             }
         }
         
         private Node Neighbour(int x, int y, Dir direction)
         {
             return new Node(
                 x,
                 y,
                 direction,
                 direction == LastStep ? StepsSinceTurn + 1 : 1);
         }
    }
    
    enum Dir
    {
        None,
        Right,
        Down,
        Left,
        Up
    }

    private static Dir Reverse(Dir dir)
    {
        return dir switch
        {
            Dir.None => Dir.None,
            Dir.Right => Dir.Left,
            Dir.Down => Dir.Up,
            Dir.Left => Dir.Right,
            Dir.Up => Dir.Down,
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }
}