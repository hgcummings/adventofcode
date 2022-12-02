using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2016._10;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var logicRules = new Dictionary<int, Action<int, int>>();

        var inputRegex = new Regex("value (?'value'[0-9]+) goes to bot (?'bot'[0-9]+)");
        var logicRegex = new Regex("bot (?'bot'[0-9]+) gives low to (?'lowType'(bot|output)) (?'lowIndex'[0-9]+) and high to (?'highType'(bot|output)) (?'highIndex'[0-9]+)");

        var inputs = new List<Action>();
        var bots = new Dictionary<int, int>();
        var outputs = new Dictionary<int, int>();

        void SendValueToBot(int value, int index)
        {
            if (bots.ContainsKey(index))
            {
                var low = Math.Min(value, bots[index]);
                var high = Math.Max(value, bots[index]);
                
                bots.Remove(index);
                logicRules[index](low, high);
            }
            else
            {
                bots[index] = value;
            }
        }

        void SendValueToOutput(int value, int index)
        {
            outputs[index] = value;
        }

        Action<int> ParseAction(Match match, string prefix)
        {
            var targetType = match.Groups[prefix + "Type"].Value;
            if (targetType == "bot")
            {
                return value => SendValueToBot(value, match.GetInt(prefix + "Index"));
            }
            if (targetType == "output")
            {
                return value => SendValueToOutput(value, match.GetInt(prefix + "Index"));
            }

            throw new Exception($"Invalid target type {targetType}");
        }
        
        foreach (var line in this.ReadInputLines("input.txt"))
        {
            var match = inputRegex.Match(line);
            if (match.Success)
            {
                int value = match.GetInt("value");
                int bot = match.GetInt("bot");
                
                inputs.Add(() => SendValueToBot(value, bot));
            }
            else
            {
                match = logicRegex.Match(line);
                if (match.Success)
                {
                    Action<int> lowAction = ParseAction(match, "low");
                    Action<int> highAction = ParseAction(match, "high");
                    
                    logicRules[match.GetInt("bot")] = (low, high) =>
                    {
                        lowAction(low);
                        highAction(high);
                    };
                }
                else
                {
                    throw new Exception($"Invalid instruction '{line}'");
                }
            }
        }

        foreach (var input in inputs)
        {
            input();
        }
        
        return String.Join(',', outputs);
    }
}