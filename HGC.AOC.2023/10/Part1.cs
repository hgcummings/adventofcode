using HGC.AOC.Common;

namespace HGC.AOC._2023._10;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt").ToArray();

        var nodes = new Node[input.Length,input[0].Length];
        Node startNode = null;
        
        for (var x = 0; x < input.Length; ++x)
        {
            var line = input[x]; 
            for (var y = 0; y < line.Length; ++y)
            {
                nodes[x, y] = new Node(line[y], x, y);
                if (line[y] == 'S')
                {
                    startNode = nodes[x, y];
                }
            }
        }
        
        for (var x = 0; x < input.Length; ++x)
        {
            var line = input[0]; 
            for (var y = 0; y < line.Length; ++y)
            {
                var node = nodes[x, y];

                if (node == startNode)
                {
                    if (x > 0 && nodes[x - 1, y].HasSouth)
                    {
                        nodes[x - 1, y].South = node;
                        node.North = nodes[x - 1, y];
                    }
                    
                    if (y < input[0].Length - 1 && nodes[x , y + 1].HasWest)
                    {
                        nodes[x, y + 1].West = node;
                        node.East = nodes[x, y + 1];
                    }
                    
                    if (x < line.Length - 1 && nodes[x + 1, y].HasNorth)
                    {
                        nodes[x + 1, y].North = node;
                        node.South = nodes[x + 1, y];
                    }

                    if (y > 0 && nodes[x, y - 1].HasEast)
                    {
                        nodes[x, y - 1].East = node;
                        node.West = nodes[x, y - 1];
                    }
                    
                    continue;
                }
                
                if (node.HasNorth && x > 0 && nodes[x - 1, y].HasSouth)
                {
                    node.North = nodes[x - 1, y];
                }
                if (node.HasEast && y < line.Length - 1 && nodes[x, y + 1].HasWest)
                {
                    node.East = nodes[x, y + 1];
                }
                if (node.HasSouth && x < input.Length - 1 && nodes[x + 1, y].HasNorth)
                {
                    node.South = nodes[x + 1, y];
                }
                if (node.HasWest && y > 0 && nodes[x, y - 1].HasEast)
                {
                    node.West = nodes[x, y - 1];
                }
            }
        }

        var lastA = startNode;
        var lastB = startNode;
        var nodeA = startNode.Neighbours.First();
        var nodeB = startNode.Neighbours.Last();

        var distance = 1;
        while (nodeA != nodeB)
        {
            var newA = nodeA.Neighbours.Single(n => n != lastA);
            var newB = nodeB.Neighbours.Single(n => n != lastB);
            
            lastA = nodeA;
            lastB = nodeB;

            nodeA = newA;
            nodeB = newB;
            Console.WriteLine($"A: {lastA.Loc} -> {nodeA.Loc}");
            Console.WriteLine($"B: {lastB.Loc} -> {nodeB.Loc}");
            ++distance;
        }
        
        return distance;
    }

    class Node
    {
        public char Symbol { get; }
        public string Loc { get; }

        public Node North;
        public Node East;
        public Node South;
        public Node West;

        public Node(char symbol, int x, int y)
        {
            this.Symbol = symbol;
            this.Loc = $"({x}, {y})";
        }

        public IEnumerable<Node> Neighbours
        {
            get
            {
                if (North != null) yield return North;
                if (East != null) yield return East;
                if (South != null) yield return South;
                if (West != null) yield return West;
            }
        }

        public bool HasNorth => Symbol is '|' or 'J' or 'L';
        public bool HasEast => Symbol is '-' or 'L' or 'F';
        public bool HasSouth => Symbol is '|' or 'F' or '7';
        public bool HasWest => Symbol is '-' or '7' or 'J';

        public override string ToString()
        {
            return $"{Loc}, {Symbol}";
        }
    }
}