using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2023._02;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var games = input.Select(line =>
            line.Split(":")[1].Split(";").Select(handStr =>
            {
                var hand = new Hand();

                foreach (var colStr in handStr.Split(","))
                {
                    var parts = colStr.Trim().Split(" ").ToArray();
                    switch (parts[1])
                    {
                        case "red":
                            hand.Red = Int32.Parse(parts[0]);
                            break;
                        case "green":
                            hand.Green = Int32.Parse(parts[0]);
                            break;
                        case "blue":
                            hand.Blue = Int32.Parse(parts[0]);
                            break;
                        default:
                            throw new Exception("Unrecognised colour " + parts[1]);
                    }
                }

                return hand;
            }).ToArray());

        return games.Select(game =>
            game.Max(h => h.Red) * game.Max(h => h.Green) * game.Max(h => h.Blue)).Sum();
    }
    
    public struct Hand
    {
        public int Red;
        public int Green;
        public int Blue;
    }
}