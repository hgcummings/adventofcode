﻿using HGC.AOC.Common;

namespace HGC.AOC._2022._02;

public class Part2 : ISolution
{
    public string? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var score = input.Select(CalculateScore).Sum();
        return score.ToString();
    }
    
    private int CalculateScore(string game)
    {
        var opponent = game[0] switch
        {
            'A' => Choice.Rock,
            'B' => Choice.Paper,
            'C' => Choice.Scissors
        };

        var player = game[2] switch
        {
            'X' => (Choice)(((int)opponent + 2)  % 3),
            'Y' => opponent,
            'Z' => (Choice)(((int)opponent + 1)  % 3)
        };

        var choiceScore = (int)player + 1;
        var outcomeScore = 0;
        if (player == opponent)
        {
            outcomeScore = 3;
        } else if ((int)player == ((int)opponent + 1) % 3)
        {
            outcomeScore = 6;
        }
        
        return choiceScore + outcomeScore;
    }

    enum Choice
    {
        Rock = 0,
        Paper = 1,
        Scissors = 2
    }
}