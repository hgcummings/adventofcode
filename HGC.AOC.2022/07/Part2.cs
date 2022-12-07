using System.Text.RegularExpressions;
using HGC.AOC.Common;

namespace HGC.AOC._2022._07;

public class Part2 : ISolution
{
    public object? Answer()
    {
        var input = this.ReadInputLines("input.txt");

        var tld = new Directory("/");
        var currentPath = new Stack<Directory>();
        currentPath.Push(tld);

        var isListing = false;
        
        var fileExpr = new Regex("(?'Size'[0-9]+) (?'Name'.*)");
        
        foreach (var line in input)
        {
            if (isListing)
            {
                if (line.StartsWith("$"))
                {
                    isListing = false;
                }
                else
                {
                    var currentDir = currentPath.Peek();
                    if (line.StartsWith("dir "))
                    {
                        currentDir.Children.Add(new Directory(line.Trim().Substring(4)));
                    }
                    else
                    {
                        var match = fileExpr.Match(line);
                        var file = match.Parse<File>();
                        currentDir.Children.Add(file);
                    }

                    continue;
                }
            }
            
            if (line.Trim() == "$ cd /")
            {
                while (currentPath.Peek() != tld)
                {
                    currentPath.Pop();
                }
                continue;
            }
            
            if (line.Trim() == "$ cd ..")
            {
                currentPath.Pop();
                continue;
            }

            if (line.StartsWith("$ cd "))
            {
                var dirName = line.Trim().Substring(5);
                var childDir = currentPath.Peek()
                    .Children.OfType<Directory>()
                    .Single(c => c.Name == dirName);
                currentPath.Push(childDir);
                continue;
            }

            if (line.Trim() == "$ ls")
            {
                isListing = true;
                continue;
            }
            
            Console.WriteLine("Unrecognised input " + line);
        }
        
        var availableSpace = 70000000 - tld.Size;
        var minToDelete = 30000000 - availableSpace;

        return FindDirectories(tld, d => d.Size >= minToDelete).OrderBy(d => d.Size).First().Size;
    }

    IEnumerable<Directory> FindDirectories(Directory parent, Func<Directory, bool> filter)
    {
        if (filter(parent))
        {
            yield return parent;
        }
        
        foreach (var child in parent.Children)
        {
            if (child is Directory childDirectory)
            {
                foreach (var directory in FindDirectories(childDirectory, filter))
                {
                    yield return directory;
                }
            }
        }
    }

    interface Entry
    {
        public string Name { get; }
        public int Size { get; }
    }

    class Directory : Entry
    {
        public Directory(string name)
        {
            Name = name;
            Children = new List<Entry>();
        }

        public List<Entry> Children { get; }

        public string Name { get; }

        public int Size
        {
            get
            {
                return Children.Sum(c => c.Size);
            }
        }
    }

    class File : Entry
    {
        public string Name { get; set; }
        public int Size { get; set; }
    }

}