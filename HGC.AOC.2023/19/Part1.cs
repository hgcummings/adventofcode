using System.Drawing;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2023._19;

public class Part1 : ISolution
{
    public object? Answer()
    {

        var input = this.ReadInputLines("input.txt");

        var workflows = new Dictionary<string, Rule[]>();
        var parts = new List<Part>();
        
        using (var inputEnumerator = input.GetEnumerator())
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

            var partRegex =
                new Regex(@"{x=(?'X'[0-9]+),m=(?'M'[0-9]+),a=(?'A'[0-9]+),s=(?'S'[0-9]+)}");
            while (inputEnumerator.MoveNext())
            {
                var line = inputEnumerator.Current;
                if (line.Trim() == "")
                {
                    break;
                }

                var partData = partRegex.Match(line).Parse<PartData>();
                parts.Add(new Part(partData.X, partData.M, partData.A, partData.S));
            }
        }

        var accepted = new List<Part>();
        foreach (var part in parts)
        {
            var destination = "in";
            while (!(destination == "R" || destination == "A"))
            {
                var workflow = workflows[destination];
                foreach (var rule in workflow)
                {
                    if (rule.Condition(part))
                    {
                        destination = rule.Destination;
                        break;
                    }
                }
            }

            if (destination == "A")
            {
                accepted.Add(part);
            }
        }


        return accepted.Select(part => part.X + part.M + part.A + part.S).Sum();
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
            return new Rule(_ => true, parts[0]);
        }

        var conditionData = ConditionRegex.Match(parts[0]).Parse<ConditionData>();
        
        var param = Expression.Parameter(typeof(Part), "p");
        var member = Expression.Property(param, conditionData.Property);
        var constant = Expression.Constant(conditionData.Value);
        var body = Expression.MakeBinary(
            conditionData.Operator switch
            {
                ">" => ExpressionType.GreaterThan,
                "<" => ExpressionType.LessThan,
                _ => throw new Exception("Unrecognised rule type")
            },
            member, constant);
       
        return new Rule(Expression.Lambda<Func<Part, bool>>(body, param).Compile(), parts[1]);
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
        public Rule(Func<Part, bool> condition, string destination)
        {
            Condition = condition;
            Destination = destination;
        }

        public Func<Part, bool> Condition { get; }
        public string Destination { get; }
    }

    public struct Part
    {
        public Part(int x, int m, int a, int s)
        {
            X = x;
            M = m;
            A = a;
            S = s;
        }

        public int X { get; }
        public int M { get; }
        public int A { get; }
        public int S { get; }
    }

    public class PartData
    {
        public int X { get; set; }
        public int M { get; set; }
        public int A { get; set; }
        public int S { get; set; }
    }
}