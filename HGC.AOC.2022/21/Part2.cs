using System.Linq.Expressions;
using HGC.AOC.Common;

namespace HGC.AOC._2022._21;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var monkeys = new Dictionary<string, Func<Expression<Func<double>>>>();

        foreach (var line in input)
        {
            var parts = line.Split(": ");
            var id = parts[0];
            var value = parts[1].Split(" ");
            if (value.Length == 1)
            {
                var val = Double.Parse(value[0]);
                monkeys.Add(id, () => Expression.Lambda<Func<double>>(Expression.Constant(val)));
            }
            else
            {
                if (id == "root")
                {
                    monkeys.Add(id, () => Expression.Lambda<Func<double>>(Expression.Condition(
                        Expression.Equal(monkeys[value[0]]().Body, monkeys[value[2]]().Body),
                        Expression.Constant((double) 1),
                        Expression.Constant((double) 0))));
                }
                else
                {
                    monkeys.Add(id, () => value[1] switch
                        {
                            "+" => Expression.Lambda<Func<double>>(Reduce(Expression.Add(monkeys[value[0]]().Body, monkeys[value[2]]().Body))),
                            "-" => Expression.Lambda<Func<double>>(Reduce(Expression.Subtract(monkeys[value[0]]().Body, monkeys[value[2]]().Body))),
                            "*" => Expression.Lambda<Func<double>>(Reduce(Expression.Multiply(monkeys[value[0]]().Body, monkeys[value[2]]().Body))),
                            "/" => Expression.Lambda<Func<double>>(Reduce(Expression.Divide(monkeys[value[0]]().Body, monkeys[value[2]]().Body)))
                        }
                    );
                }
            }
        }

        Expression Reduce(Expression expression)
        {
            if (expression is BinaryExpression binaryExpression)
            {
                return ReduceBinaryExpression(binaryExpression);
            }

            return expression;
        }

        Expression ReduceBinaryExpression(BinaryExpression binaryExpression)
        {
            if (binaryExpression.Left is ConstantExpression && binaryExpression.Right is ConstantExpression)
            {
                var lambda = Expression.Lambda<Func<double>>(binaryExpression);
                return Expression.Constant(lambda.Compile()());
            }

            return binaryExpression;
        }

        var parameter = Expression.Parameter(typeof(double), "humn");
        monkeys["humn"] = () => Expression.Lambda<Func<double>>(parameter);

        BinaryExpression eq = (BinaryExpression) ((ConditionalExpression) monkeys["root"]().Body).Test;

        Console.WriteLine(eq.ToString());
        while (true)
        {
            ConstantExpression c = null;
            BinaryExpression b = null;

            if (eq.Right is ConstantExpression && eq.Left is BinaryExpression)
            {
                c = (ConstantExpression) eq.Right;
                b = (BinaryExpression) eq.Left;
            }
            else if (eq.Left is ConstantExpression && eq.Right is BinaryExpression)
            {
                c = (ConstantExpression) eq.Left;
                b = (BinaryExpression) eq.Right;
            }
            
            if (b != null && c != null)
            {
                if (b.Right is ConstantExpression)
                {
                    eq = Expression.Equal(
                        b.Left,
                        Reduce(Expression.MakeBinary(b.NodeType switch
                        {
                            ExpressionType.Add => ExpressionType.Subtract,
                            ExpressionType.Multiply => ExpressionType.Divide,
                            ExpressionType.Subtract => ExpressionType.Add,
                            ExpressionType.Divide => ExpressionType.Multiply
                        }, c, b.Right))
                    );
                }
                else if (b.Left is ConstantExpression)
                {
                    eq = Expression.Equal(b.Right, Reduce(b.NodeType switch
                    {
                        ExpressionType.Add => Expression.Subtract(c, b.Left),
                        ExpressionType.Subtract => Expression.Subtract(b.Left, c),
                        ExpressionType.Multiply => Expression.Divide(c, b.Left),
                        ExpressionType.Divide => Expression.Divide(b.Left, c)
                    }));
                }
            }
            else
            {
                break;
            }
            Console.WriteLine();
            Console.WriteLine(eq.ToString());
        }

        return eq.Left is ConstantExpression
            ? ((ConstantExpression)eq.Left).Value
            : ((ConstantExpression)eq.Right).Value;
    }
}