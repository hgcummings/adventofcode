﻿using System.Drawing;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2022._11;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInput("input.txt");

        var monkeyExpr = new Regex("Monkey [0-9]+:\\s*" +
                                   "Starting items: (?'Items'[0-9, ]+)\\s*" +
                                   "Operation: new = old (?'Operator'[\\*\\+]) (?'Operand'[0-9a-z]+)\\s*" +
                                   "Test: divisible by (?'TestFactor'[0-9]+)\\s*" +
                                    "If true: throw to monkey (?'TrueTarget'[0-9]+)\\s*" +
                                    "If false: throw to monkey (?'FalseTarget'[0-9]+)"
                                   );

        var monkeys = monkeyExpr.Matches(input)
            .Select(match => new Monkey(match.Parse<MonkeyData>())).ToList();

        for (var round = 0; round < 20; ++round)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.Items.Count > 0)
                {
                    var value = monkey.Items.Dequeue();
                    monkey.InspectionCount++;
                    value = monkey.Operation(value);
                    value /= 3;
                    var target = value % monkey.TestFactor == 0 ? monkey.TrueTarget : monkey.FalseTarget;
                    monkeys[target].Items.Enqueue(value);
                }
            }
        }
        
        var inspectionCounts = monkeys.Select(m => m.InspectionCount).OrderByDescending(x => x).ToList();
        return inspectionCounts[0] * inspectionCounts[1];
    }

    public class Monkey
    {
        public Monkey(MonkeyData data)
        {
            Items = new Queue<int>(data.Items.Split(",").Select(x => Int32.Parse(x.Trim())));
            if (data.Operand == "old")
            {
                Operation = data.Operator switch
                {
                    "+" => old => old + old,
                    "*" => old => old * old
                };
            }
            else
            {
                var operand = Int32.Parse(data.Operand);
                Operation = data.Operator switch
                {
                    "+" => old => old + operand,
                    "*" => old => old * operand
                };
            }

            TestFactor = data.TestFactor;
            TrueTarget = data.TrueTarget;
            FalseTarget = data.FalseTarget;

            InspectionCount = 0;
        }
        
        public Queue<int> Items { get; }
        public Func<int, int> Operation { get; }
        public int TestFactor { get; }
        public int TrueTarget { get; }
        public int FalseTarget { get; }
        public int InspectionCount { get; set; }
    }

    public class MonkeyData
    {
        public string Items { get; set; }
        public string Operator { get; set; }
        public string Operand { get; set; }
        public int TestFactor { get; set; }
        public int TrueTarget { get; set; }
        public int FalseTarget { get; set; }
    }
}