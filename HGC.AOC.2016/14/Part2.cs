using System.Collections.Immutable;
using System.Drawing;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2016._14;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = "ahsbgdzn";

        var potentialKeys = new Dictionary<int, string>();
        var matchedKeys = new HashSet<int>();
        var actualKeys = new List<int>();

        var md5 = MD5.Create();
        md5.Initialize();

        var keyRegex = new Regex("([a-f0-9])\\1{2,}");
        
        for (var i = 0;; ++i)
        {
            if (potentialKeys.Remove(i - 1001))
            {
                if (matchedKeys.Contains(i - 1001))
                {
                    actualKeys.Add(i - 1001);
                    Console.WriteLine(actualKeys.Count);
                    if (actualKeys.Count == 64)
                    {
                        return i - 1001;
                    }
                }
            }
            
            md5.Initialize();
            string hash = input + i;
            for (var j = 0; j < 2017; ++j)
            {
                md5.ComputeHash(Encoding.ASCII.GetBytes(hash));
                hash = Convert.ToHexString(md5.Hash!).ToLower();
            }

            foreach (var key in potentialKeys)
            {
                if (hash.Contains(key.Value))
                {
                    matchedKeys.Add(key.Key);
                }
            }

            var match = keyRegex.Match(hash);
            if (match.Success)
            {
                potentialKeys.Add(i, new String(match.Value[0], 5));
            }
        }
    }
}