﻿using System.Drawing;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2022._20;

public class Part2 : ISolution
{
    const long Key = 811589153;
    public object? Answer()
    {
        
        var input = this.ReadInputLines("input.txt");
        var nodes = input.Select(line => new Node { Value = long.Parse(line) * Key }).ToList();

        for (var i = 0; i < nodes.Count; ++i)
        {
            var prev = nodes[(i + nodes.Count - 1) % nodes.Count];
            var next = nodes[(i + 1) % nodes.Count];

            nodes[i].Prev = prev;
            nodes[i].Next = next;
        }

        for (var round = 0; round < 10; ++round)
        {
            // var zeroDebug = nodes.Single(n => n.Value == 0);
            // var decryptedDebug = new List<long> { 0 };
            // var currDebug = zeroDebug.Next;
            // while (currDebug != zeroDebug)
            // {
            //     decryptedDebug.Add(currDebug.Value);
            //     currDebug = currDebug.Next;
            // }
            //     
            // Console.WriteLine(String.Join(", ", decryptedDebug));
            
            foreach (var node in nodes)
            {
                if (node.Value > 0)
                {
                    var shift = node.Value % (nodes.Count - 1);
                    for (long i = 0; i < shift; ++i)
                    {
                        var prev = node.Prev;
                        var next = node.Next;
                        node.Next = next.Next;
                        next.Next.Prev = node;
                        next.Next = node;
                        next.Prev = node.Prev;
                        node.Prev = next;
                        prev.Next = next;
                    }
                }
                else if (node.Value < 0)
                {
                    var shift = Math.Abs(node.Value) % (nodes.Count - 1);
                    for (long i = 0; i < shift; ++i)
                    {
                        var next = node.Next;
                        var prev = node.Prev;
                        node.Prev = prev.Prev;
                        prev.Prev.Next = node;
                        prev.Prev = node;
                        prev.Next = node.Next;
                        node.Next = prev;
                        next.Prev = prev;
                    }
                }
            }
        }
        

        var zero = nodes.Single(n => n.Value == 0);
        var decrypted = new List<long> { 0 };
        var curr = zero.Next;
        while (curr != zero)
        {
            decrypted.Add(curr.Value);
            curr = curr.Next;
        }
        
        Console.WriteLine(String.Join(", ", decrypted));
        
        return decrypted[1000 % decrypted.Count] +
               decrypted[2000 % decrypted.Count] +
               decrypted[3000 % decrypted.Count];
    }

    public class Node
    {
        public long Value;
        public Node Prev;
        public Node Next;
    }
}