using HGC.AOC.Common;

namespace HGC.AOC._2016._07;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        return input.Count(addr => SupportsTls(addr.Trim())).ToString();
    }

    public bool SupportsTls(string ip)
    {
        char[] buff = { ' ', ' ', ' ' };
        int brackets = 0;
        bool hasAbba = false;

        foreach (var c in ip)
        {
            if (c == '[')
            {
                brackets++;
            }
            else if (c == ']')
            {
                brackets--;
            }

            if (c == buff[0] && c != buff[1] && buff[1] == buff[2])
            {
                if (brackets > 0)
                {
                    return false;
                }
                else
                {
                    hasAbba = true;
                }
            }

            for (var i = 0; i < buff.Length-1; ++i)
            {
                buff[i] = buff[i + 1];
            }

            buff[^1] = c;
        }

        return hasAbba;
    }
}