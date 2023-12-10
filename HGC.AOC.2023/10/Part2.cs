using System.Diagnostics;
using HGC.AOC.Common;

namespace HGC.AOC._2023._10;

public class Part2 : ISolution
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

                        if (node.North != null)
                        {
                            node.Symbol = 'L';
                        }
                    }
                    
                    if (x < line.Length - 1 && nodes[x + 1, y].HasNorth)
                    {
                        nodes[x + 1, y].North = node;
                        node.South = nodes[x + 1, y];

                        if (node.North != null)
                        {
                            node.Symbol = '|';
                        }
                        
                        if (node.East != null)
                        {
                            node.Symbol = 'F';
                        }
                    }

                    if (y > 0 && nodes[x, y - 1].HasEast)
                    {
                        nodes[x, y - 1].East = node;
                        node.West = nodes[x, y - 1];

                        if (node.North != null)
                        {
                            node.Symbol = 'J';
                        }
                        
                        if (node.East != null)
                        {
                            node.Symbol = '-';
                        }
                        
                        if (node.South != null)
                        {
                            node.Symbol = '7';
                        }
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

        var loopA = new List<Node> {startNode};
        var loopB = new List<Node>();
        
        var distance = 1;
        while (nodeA != nodeB)
        {
            loopA.Add(nodeA);
            loopB.Add(nodeB);
            
            var newA = nodeA.Neighbours.Single(n => n != lastA);
            var newB = nodeB.Neighbours.Single(n => n != lastB);
            
            lastA = nodeA;
            lastB = nodeB;

            nodeA = newA;
            nodeB = newB;
            
            ++distance;
        }
        
        loopA.Add(nodeA);
        loopB.Reverse();
        loopA.AddRange(loopB);
        loopA.Add(startNode);

        var leftQ = new Queue<Node>();
        var rightQ = new Queue<Node>();
        for (var i = 1; i < loopA.Count; ++i)
        {   
            var curr = loopA[i];
            var prev = loopA[i - 1];

            if ((curr == prev.West && curr.Symbol is '-' or 'F') ||
                (curr == prev.North && curr.Symbol is '7'))
            {
                if (curr.X > 0)
                {
                    rightQ.Enqueue(nodes[curr.X - 1, curr.Y]);
                }
            }
            if ((curr == prev.North && curr.Symbol is '|' or '7') ||
                (curr == prev.East && curr.Symbol is 'J'))
            {
                if (curr.Y < input[0].Length - 1)
                {
                    rightQ.Enqueue(nodes[curr.X, curr.Y + 1]);
                }
            }
            if ((curr == prev.East && curr.Symbol is '-' or 'J') ||
                (curr == prev.South && curr.Symbol is 'L'))
            {
                if (curr.X < input.Length - 1)
                {
                    rightQ.Enqueue(nodes[curr.X + 1, curr.Y]);
                }
            }
            if ((curr == prev.South && curr.Symbol is '|' or 'L') ||
                (curr == prev.West && curr.Symbol is 'F'))
            {
                if (curr.Y > 0)
                {
                    rightQ.Enqueue(nodes[curr.X, curr.Y - 1]);
                }
            }
            
            if ((curr == prev.East && curr.Symbol is '-' or '7') ||
                (curr == prev.North && curr.Symbol is 'F'))
            {
                if (curr.X > 0)
                {
                    leftQ.Enqueue(nodes[curr.X - 1, curr.Y]);
                }
            }
            if ((curr == prev.South && curr.Symbol is '|' or 'J') ||
                (curr == prev.East && curr.Symbol is '7'))
            {
                if (curr.Y < input[0].Length - 1)
                {
                    leftQ.Enqueue(nodes[curr.X, curr.Y + 1]);
                }
            }
            if ((curr == prev.West && curr.Symbol is '-' or 'L') ||
                (curr == prev.South && curr.Symbol is 'J'))
            {
                if (curr.X < input.Length - 1)
                {
                    leftQ.Enqueue(nodes[curr.X + 1, curr.Y]);
                }
            }
            if ((curr == prev.North && curr.Symbol is '|' or 'F') ||
                (curr == prev.West && curr.Symbol is 'L'))
            {
                if (curr.Y > 0)
                {
                    leftQ.Enqueue(nodes[curr.X, curr.Y - 1]);
                }
            }
        }
        
        void FillSide(char fill, Queue<Node> queue)
        {
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (node.Symbol != fill && !loopA.Contains(node))
                {
                    node.Symbol = fill;
                    if (node.X > 0)
                    {
                        queue.Enqueue(nodes[node.X - 1, node.Y]);
                    }

                    if (node.X < input.Length - 1)
                    {
                        queue.Enqueue(nodes[node.X + 1, node.Y]);
                    }

                    if (node.Y > 0)
                    {
                        queue.Enqueue(nodes[node.X, node.Y - 1]);
                    }

                    if (node.Y < input[0].Length - 1)
                    {
                        queue.Enqueue(nodes[node.X, node.Y + 1]);
                    }
                }
            }
        }
        
        FillSide('#', rightQ);
        FillSide('*', leftQ);
        
        var countR = 0;
        var countL = 0;
        var countOrphan = 0;
        for (var x = 0; x < input.Length; ++x)
        {
            Console.WriteLine();
            for (var y = 0; y < input[x].Length; ++y)
            {
                var node = nodes[x, y];
                var symbol = node.Symbol;
                if (node.Symbol == '#')
                {
                    countR++;
                }
                else if (node.Symbol == '*')
                {
                    countL++;
                }
                else if (!loopA.Contains(node))
                {
                    symbol = '!';
                    countOrphan++;
                }
                Console.Write(symbol);
            }
        }
        
        Console.WriteLine(countOrphan);
        return Math.Min(countR, countL) + countOrphan;
    }

    class Node
    {
        public char Symbol { get; set; }
        public string Loc { get; }
        public int X { get; }
        public int Y { get; }

        public Node North;
        public Node East;
        public Node South;
        public Node West;

        public Node(char symbol, int x, int y)
        {
            this.Symbol = symbol;
            this.Loc = $"({x}, {y})";
            this.X = x;
            this.Y = y;
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