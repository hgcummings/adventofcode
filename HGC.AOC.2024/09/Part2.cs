using HGC.AOC.Common;

namespace HGC.AOC._2024._09;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInput("input.txt");

        var disk = new List<int>();
        var fileSizes = new List<int>();
        var filePosns = new List<int>();

        var fileNext = true;
        foreach (var c in input)
        {
            var length = c - 48;
            if (fileNext)
            {
                filePosns.Add(disk.Count);
                
                for (var i = 0; i < length; ++i)
                {
                    disk.Add(fileSizes.Count);
                }

                fileSizes.Add(length);
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

        for (var f = fileSizes.Count - 1; f >= 0; --f)
        {
            var spaceStart = Int32.MaxValue;

            for (var i = 0; disk[i] != f; ++i)
            {
                if (disk[i] == -1 && spaceStart == Int32.MaxValue)
                {
                    spaceStart = i;
                }

                if (disk[i] != -1)
                {
                    spaceStart = Int32.MaxValue;
                }

                if (i - spaceStart + 1 >= fileSizes[f])
                {
                    for (var j = 0; j < fileSizes[f]; ++j)
                    {
                        disk[spaceStart + j] = f;
                        disk[filePosns[f] + j] = -1;
                    }
                    break;
                }
            }
        }
        
        // PrintDisk(disk);

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