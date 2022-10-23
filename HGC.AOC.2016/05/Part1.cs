using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2016._05;

public class Part1 : ISolution
{
    public string? Answer()
    {
        var input = "ffykfhsq";
        
        var password = "";
        var md5 = MD5.Create();

        for (var i = 0; password.Length < 8; ++i)
        {
            md5.Initialize();
            md5.ComputeHash(Encoding.ASCII.GetBytes(input + i));
            var hash = Convert.ToHexString(md5.Hash!);
            if (hash.StartsWith("00000"))
            {
                password += hash[5];
                Console.WriteLine(password);
            }
        }

        return password;
    }
}