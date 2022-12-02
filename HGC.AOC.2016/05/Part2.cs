using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using HGC.AOC.Common;

namespace HGC.AOC._2016._05;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = "ffykfhsq";
        
        var password = "qqqqqqqq";
        var md5 = MD5.Create();

        for (var i = 0; password.Contains('q'); ++i)
        {
            md5.Initialize();
            md5.ComputeHash(Encoding.ASCII.GetBytes(input + i));
            var hash = Convert.ToHexString(md5.Hash!).ToLower();
            if (hash.StartsWith("00000"))
            {
                var position = int.Parse(hash.Substring(5, 1), NumberStyles.HexNumber);
                if (position < 8 && password[position] == 'q')
                {
                    password = password.Substring(0, position) + hash[6] + password.Substring(position + 1);
                    Console.WriteLine(password);
                }
            }
        }

        return password;
    }
}