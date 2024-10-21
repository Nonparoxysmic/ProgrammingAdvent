// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal class Day07 : Day
{
    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        Directory root = BuildFilesystem(input);
        int sumOfSmallDirectories = Directory.AllDirectories
            .Where(d => d.Size <= 100_000).Sum(d => d.Size);
        int spaceNeeded = 30_000_000 - (70_000_000 - root.Size);
        int sizeToDelete = Directory.AllDirectories.Select(d => d.Size)
            .Where(s => s >= spaceNeeded).Min();

        return result.WriteAnswers(sumOfSmallDirectories, sizeToDelete);
    }

    private static Directory BuildFilesystem(string[] input)
    {
        Directory.AllDirectories.Clear();
        Directory root = new();
        Directory current = root;
        foreach (string line in input)
        {
            string[] terms = line.Split();
            if (terms[0] == "$" && terms[1] == "cd")
            {
                if (terms[2] == ".." && current.Parent is not null)
                {
                    current = current.Parent;
                }
                else if (terms[2] == "/")
                {
                    current = root;
                }
                else
                {
                    current = current.Subdirectories[terms[2]];
                }
                continue;
            }
            if (terms[0] == "dir")
            {
                current.Subdirectories.Add(terms[1], new Directory(terms[1], current));
                continue;
            }
            if (int.TryParse(terms[0], out int size))
            {
                current.Files.Add(new File(terms[1], current, size));
            }
        }
        root.CalculateSize();
        return root;
    }

    private class Directory
    {
        public static List<Directory> AllDirectories = [];

        public string Name { get; }
        public Directory? Parent { get; }
        public int Size { get; private set; }
        public Dictionary<string, Directory> Subdirectories { get; } = [];
        public List<File> Files { get; } = [];

        public Directory(string name = "", Directory? parent = null)
        {
            Name = name;
            Parent = parent;
            AllDirectories.Add(this);
        }

        public int CalculateSize()
        {
            Size = 0;
            foreach (var kvp in Subdirectories)
            {
                Size += kvp.Value.CalculateSize();
            }
            foreach (File file in Files)
            {
                Size += file.Size;
            }
            return Size;
        }
    }

    private class File(string name, Directory parent, int size)
    {
        public string Name { get; } = name;
        public Directory Parent { get; } = parent;
        public int Size { get; } = size;
    }
}
