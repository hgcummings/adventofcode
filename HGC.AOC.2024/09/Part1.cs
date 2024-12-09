using HGC.AOC.Common;

namespace HGC.AOC._2024._09;

public class Part1 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInput("input.txt");

        var disk = new List<int>();

        var fileNum = 0;
        var fileNext = true;
        foreach (var c in input)
        {
            var length = c - 48;
            if (fileNext)
            {
                for (var i = 0; i < length; ++i)
                {
                    disk.Add(fileNum);
                }

                ++fileNum;
            }
            else
            {
                for (var i = 0; i < length; ++i)
                {
                    disk.Add(-1);
                }
            }

            fileNext = !fileNext;
        }
        
        // PrintDisk(disk);
        var j = disk.Count - 1;
        for (var i = 0; i < j; ++i)
        {
            if (disk[i] != -1) continue;
            while (disk[j] == -1 && j > i)
            {
                j -= 1;
            }

            disk[i] = disk[j];
            disk[j] = -1;
        }
        
        return CheckSum(disk);
    }

    long CheckSum(List<int> disk)
    {
        var sum = 0L;
        for (var i = 0; i < disk.Count; ++i)
        {
            if (disk[i] == -1)
            {
                continue;
            }

            sum += i * (long) disk[i];
        }

        return sum;
    }

    void PrintDisk(List<int> disk)
    {
        foreach (var b in disk)
        {
            if (b == -1)
            {
                Console.Write('.');
            }
            else
            {
                Console.Write(b);
            }
        }
        Console.WriteLine();
    }
}