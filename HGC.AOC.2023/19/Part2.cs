using System.Drawing;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2023._19;

public class Part2 : ISolution
{
    public object? Answer()
    {

        var inputLines = this.ReadInputLines("input.txt");

        var workflows = new Dictionary<string, Rule[]>();
        
        using (var inputEnumerator = inputLines.GetEnumerator())
        {
            var workflowRegex = new Regex(@"(?'Name'[a-z]+)\{(?'Rules'.*)\}");
            while (inputEnumerator.MoveNext())
            {
                var line = inputEnumerator.Current;
                if (line.Trim() == "")
                {
                    break;
                }

                var workflowData = workflowRegex.Match(line).Parse<WorkflowData>();
                var rules = workflowData.Rules.Split(",").Select(ParseRule).ToArray();

                workflows[workflowData.Name] = rules;
            }
        }

        var accepted = new List<PartRange>();
        var fullRange = new PartRange(
            new Range(1, 4001), new Range(1, 4001), new Range(1, 4001), new Range(1, 4001));
        var inProgress = new Stack<(string location, PartRange range)>();
        inProgress.Push(("in", fullRange));

        while (inProgress.TryPop(out var input))
        {
            var workflow = workflows[input.location];
            PartRange? remainingRange = input.range;
            foreach (var rule in workflow)
            {
                (var matchingRange, remainingRange) = Split(remainingRange.Value, rule.Condition);
                if (rule.Destination == "A")
                {
                    accepted.Add(matchingRange);
                }
                else if (rule.Destination != "R")
                {
                    inProgress.Push((rule.Destination, matchingRange));
                }

                if (remainingRange == null)
                {
                    break;
                }
            }
        }

        return accepted.Sum(partRange =>
            Size(partRange.X) * Size(partRange.M) * Size(partRange.A) * Size(partRange.S));
    }

    public (PartRange matchingRange, PartRange? remainingRange) 
        Split(PartRange range, ConditionData? condition)
    {
        if (condition == null)
        {
            return (range, null);
        }

        return condition.Property switch
        {
            "x" => (
                new PartRange(Include(range.X, condition), range.M, range.A, range.S),
                new PartRange(Exclude(range.X, condition), range.M, range.A, range.S)),
            "m" => (
                new PartRange(range.X, Include(range.M, condition), range.A, range.S),
                new PartRange(range.X, Exclude(range.M, condition), range.A, range.S)),
            "a" => (
                new PartRange(range.X, range.M, Include(range.A, condition), range.S),
                new PartRange(range.X, range.M, Exclude(range.A, condition), range.S)),
            "s" => (
                new PartRange(range.X, range.M, range.A, Include(range.S, condition)),
                new PartRange(range.X, range.M, range.A, Exclude(range.S, condition))),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public Range Include(Range range, ConditionData condition)
    {
        if (condition.Operator == "<")
        {
            if (range.End.Value <= condition.Value)
            {
                return range;
            }
            return new Range(range.Start, condition.Value);
        }

        if (condition.Operator == ">")
        {
            if (range.Start.Value > condition.Value)
            {
                return range;
            }

            return new Range(condition.Value + 1, range.End);
        }

        throw new Exception("Unrecognised rule");
    }
    
    public Range Exclude(Range range, ConditionData condition)
    {
        if (condition.Operator == "<")
        {
            if (range.Start.Value >= condition.Value)
            {
                return range;
            }

            return new Range(condition.Value, range.End);
        }

        if (condition.Operator == ">")
        {
            if (range.End.Value <= condition.Value)
            {
                return range;
            }
            return new Range(range.Start, condition.Value + 1);
        }

        throw new Exception("Unrecognised rule");
    }

    public long Size(Range range)
    {
        return range.End.Value - range.Start.Value;
    }

    public class WorkflowData
    {
        public string Name { get; set; }
        public string Rules { get; set; }
    }

    public Rule ParseRule(string rule)
    {
        var parts = rule.Split(":");
        if (parts.Length == 1)
        {
            return new Rule(null, parts[0]);
        }

        var conditionData = ConditionRegex.Match(parts[0]).Parse<ConditionData>();
        return new Rule(conditionData, parts[1]);
    }

    private static Regex ConditionRegex =
        new Regex(@"(?'Property'[xmas])(?'Operator'[<>])(?'Value'[0-9]+)");

    public class ConditionData
    {
        public string Property { get; set; }
        public string Operator { get; set; }
        public int Value { get; set; }
    }

    public class Rule
    {
        public Rule(ConditionData? condition, string destination)
        {
            Condition = condition;
            Destination = destination;
        }

        public ConditionData? Condition { get; }
        public string Destination { get; }
    }

    public struct PartRange
    {
        public PartRange(Range x, Range m, Range a, Range s)
        {
            X = x;
            M = m;
            A = a;
            S = s;
        }

        public Range X { get; }
        public Range M { get; }
        public Range A { get; }
        public Range S { get; }
    }
}