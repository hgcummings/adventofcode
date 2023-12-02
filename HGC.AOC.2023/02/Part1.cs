using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2023._02;

public class Part1 : ISolution
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
            }));

        // only 12 red cubes, 13 green cubes, and 14 blue cubes
        const int maxRed = 12;
        const int maxGreen = 13;
        const int maxBlue = 14;

        var gameId = 1;
        var sum = 0;
        foreach (var game in games)
        {
            if (!game.Any(hand => hand.Red > maxRed || hand.Green > maxGreen || hand.Blue > maxBlue))
            {
                sum += gameId;
            }

            gameId++;
        }

        return sum;
    }
    
    public struct Hand
    {
        public int Red;
        public int Green;
        public int Blue;
    }
}