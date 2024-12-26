// using HGC.AOC.Common;
//
// namespace HGC.AOC._2024._25;
//
// public class Part1 : ISolution
// {
//     public object? Answer()
//     {
//         bool isKey = false;
//         
//         var current = new List<string>();
//
//         var locks = new List<List<int>>();
//         var keys = new List<List<int>>();
//         
//         foreach (var line in this.ReadInputLines("input.txt"))
//         {
//             if (line == String.Empty)
//             {
//                 if (current.Count > 0)
//                 {
//                     var newEntry = new List<int>();
//                     for (var i = 0; i < current[0].Length; ++i)
//                     {
//                         newEntry.Add(current.Count(row => row[i] == '#') - 1);
//                     }
//
//                     if (isKey)
//                     {
//                         keys.Add(newEntry);
//                     }
//                     else
//                     {
//                         locks.Add(newEntry);
//                     }
//                 }
//
//                 current = new List<string>();
//                 continue;
//             }
//             
//             if (current.Count == 0)
//             {
//                 isKey = line.Contains('.');
//             }
//             current.Add(line);
//         }
//
//         return keys.Sum(k => locks.Count(l => IsCompatible(l, k)));
//     }
//
//     bool IsCompatible(List<int> l, List<int> k)
//     {
//         for (var i = 0; i < k.Count; ++i)
//         {
//             if (l[i] + k[i] > 5)
//             {
//                 return false;
//             }
//         }
//
//         return true;
//     }
// }