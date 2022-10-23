using HGC.AOC.Common;

namespace HGC.AOC._2016._07;

public class Part2 : ISolution
{
    public string? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        return input.Count(addr => SupportsTls(addr.Trim())).ToString();
    }

    public bool SupportsTls(string ip)
    {
        char[] buff = { ' ', ' ' };
        int brackets = 0;
        var abas = new List<string>();
        var babs = new List<string>();

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

            if (c == buff[0] && c != buff[1])
            {
                if (brackets == 0)
                {
                    abas.Add(String.Concat(buff[0], buff[1]));
                }
                else
                {
                    // Store backwards to make comparing later simpler
                    babs.Add(String.Concat(buff[1], buff[0]));
                }
            }

            for (var i = 0; i < buff.Length-1; ++i)
            {
                buff[i] = buff[i + 1];
            }

            buff[^1] = c;
        }

        return abas.Any(aba => babs.Contains(aba));
    }
}