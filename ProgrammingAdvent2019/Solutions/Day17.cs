﻿// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day17 : Day
{
    private static readonly Vector2Int[] _steps = new Vector2Int[]
    {
        new Vector2Int(-1,  0),
        new Vector2Int( 0,  1),
        new Vector2Int( 1,  0),
        new Vector2Int( 0, -1)
    };

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0 || inputLines[0].Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        if (!ValidateIntcodeInput(inputLines, out errorMessage))
        {
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();
        if (!TryGetMap(inputLines[0], out char[,] map))
        {
            return output.WriteError("Program did not produce an expected view of the scaffolds.");
        }

        // Part One
        int sumOfAlignmentParameters = 0;
        int robotStartX = -1, robotStartY = -1;
        for (int y = 1; y < map.GetLength(1) - 1; y++)
        {
            for (int x = 1; x < map.GetLength(0) - 1; x++)
            {
                if (map[x, y] == '#' && map[x - 1, y] == '#' && map[x, y - 1] == '#'
                     && map[x + 1, y] == '#' && map[x, y + 1] == '#')
                {
                    map[x, y] = 'O';
                    sumOfAlignmentParameters += x * y;
                }
                if (map[x, y] == '^' || map[x, y] == 'v' || map[x, y] == '<' || map[x, y] == '>')
                {
                    robotStartX = x;
                    robotStartY = y;
                }
            }
        }

        // Part Two
        if (robotStartX < 0)
        {
            return output.WriteAnswers(sumOfAlignmentParameters, "Error: Robot not found.");
        }
        List<string> fullPath = GetPath(map, robotStartX, robotStartY);
        if (!TryPathToProgramInputs(fullPath, out List<int>? programInput, out string pathError)
            || programInput is null)
        {
            return output.WriteAnswers(sumOfAlignmentParameters, pathError);
        }
        Day09.Day09Program program = new(inputLines[0]);
        program.WriteMemory(0, 2);
        foreach (int i in programInput)
        {
            program.EnqueueInput(i);
        }
        while (program.Tick()) { }
        if (program.Status == Day09.Day09Program.ProgramStatus.Error)
        {
            return output.WriteAnswers(sumOfAlignmentParameters, $"Program Error: {program.Error}");
        }
        if (program.Status == Day09.Day09Program.ProgramStatus.Waiting)
        {
            string error = "Program Error: Program is stuck waiting for input.";
            return output.WriteAnswers(sumOfAlignmentParameters, error);
        }
        if (program.OutputCount == 0)
        {
            string error = "Program Error: Program did not produce output.";
            return output.WriteAnswers(sumOfAlignmentParameters, error);
        }
        while (program.OutputCount > 1)
        {
            program.DequeueOutput();
        }
        long dustCollected = program.DequeueOutput();

        return output.WriteAnswers(sumOfAlignmentParameters, dustCollected);
    }

    private static bool TryGetMap(string intcode, out char[,] map)
    {
        Day09.Day09Program program = new(intcode);
        while (program.Tick()) { }
        if (program.Status == Day09.Day09Program.ProgramStatus.Error ||
            program.Status == Day09.Day09Program.ProgramStatus.Waiting ||
            program.OutputCount == 0)
        {
            map = new char[0, 0];
            return false;
        }
        StringBuilder sb = new();
        while (program.OutputCount > 0)
        {
            char c = (char)program.DequeueOutput();
            switch (c)
            {
                case '\n':
                case '#':
                case '.':
                case '^':
                case 'v':
                case '<':
                case '>':
                case 'X':
                    sb.Append(c);
                    break;
                default:
                    sb.Append('?');
                    break;
            }
        }
        string[] lines = sb.ToString().ToLines();
        if (lines[0].Length == 0)
        {
            map = new char[0, 0];
            return false;
        }
        int matchingLines = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Length == lines[0].Length)
            {
                matchingLines++;
            }
            else
            {
                break;
            }
        }
        if (matchingLines < lines.Length - 1 || lines[0].Length < 5 || matchingLines < 5)
        {
            map = new char[0, 0];
            return false;
        }
        map = new char[lines[0].Length, matchingLines];
        for (int y = 0; y < matchingLines; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                map[x, y] = lines[y][x];
            }
        }
        return true;
    }

    private static char LookAtMap(char[,] map, int x, int y)
    {
        if (x < 0 || y < 0 || x >= map.GetLength(0) || y >= map.GetLength(1))
        {
            return '.';
        }
        return map[x, y];
    }

    private static List<string> GetPath(char[,] map, int startX, int startY)
    {
        int currentX = startX, currentY = startY;
        int currentDirection = map[startX, startY] switch
        {
            '<' => 0,
            'v' => 1,
            '>' => 2,
            '^' => 3,
            _ => throw new InvalidOperationException(nameof(GetPath))
        };
        List<string> path = new();
        int timeout = 0;
        while (timeout++ < 10_000)
        {
            // Move forward if possible.
            int steps = 0;
            while (timeout++ < 10_000)
            {
                int lookX = currentX + _steps[currentDirection].X;
                int lookY = currentY + _steps[currentDirection].Y;
                if (LookAtMap(map, lookX, lookY) == '.')
                {
                    break;
                }
                currentX = lookX;
                currentY = lookY;
                steps++;
            }
            if (steps > 0)
            {
                path.Add(steps.ToString());
            }
            // Turn if possible, otherwise stop.
            int leftDirection = (currentDirection + 1) % 4;
            int lookLeftX = currentX + _steps[leftDirection].X;
            int lookLeftY = currentY + _steps[leftDirection].Y;
            if (LookAtMap(map, lookLeftX, lookLeftY) != '.')
            {
                currentDirection = leftDirection;
                path.Add("L");
                continue;
            }
            int rightDirection = (currentDirection + 3) % 4;
            int lookRightX = currentX + _steps[rightDirection].X;
            int lookRightY = currentY + _steps[rightDirection].Y;
            if (LookAtMap(map, lookRightX, lookRightY) != '.')
            {
                currentDirection = rightDirection;
                path.Add("R");
                continue;
            }
            break;
        }
        return path;
    }

    private static bool TryPathToProgramInputs(List<string> fullPath, out List<int>? inputs, out string error)
    {
        inputs = null;
        if (fullPath.Count == 0)
        {
            error = $"Error: Empty path passed to {nameof(TryPathToProgramInputs)}().";
            return false;
        }
        if (TrySimpleRoutines(fullPath, out inputs))
        {
            error = string.Empty;
            return true;
        }

        int maxTermsA = 0;
        for (int i = 10; i > 0; i--)
        {
            int len = i - 1;
            for (int j = 0; j < i; j++)
            {
                len += fullPath[j].Length;
            }
            if (len <= 20)
            {
                maxTermsA = i;
                break;
            }
        }
        int maxTermsC = 0;
        for (int i = 10; i > 0; i--)
        {
            int len = i - 1;
            for (int j = 1; j <= i; j++)
            {
                len += fullPath[^j].Length;
            }
            if (len <= 20)
            {
                maxTermsC = i;
                break;
            }
        }
        if (maxTermsA == 0 || maxTermsC == 0)
        {
            error = "Error: Invalid path.";
            return false;
        }
        for (int A = maxTermsA; A > 0; A--)
        {
            for (int C = maxTermsC; C > 0; C--)
            {
                if (TryRoutines(fullPath, A, C, out inputs))
                {
                    error = string.Empty;
                    return true;
                }
            }
        }
        error = "No valid solution found.";
        return false;
    }

    private static bool TrySimpleRoutines(List<string> fullPath, out List<int> programInput)
    {
        int fullPathLength = fullPath.Sum(s => s.Length) + fullPath.Count - 1;
        if (fullPathLength > 62)
        {
            programInput = Array.Empty<int>().ToList();
            return false;
        }
        List<string>[] functions = new List<string>[]
        {
            new List<string>(),
            new List<string>(),
            new List<string>()
        };
        int[] lengths = new int[] { -1, -1, -1 };
        int current = 0;
        for (int i = 0; i < fullPath.Count; i++)
        {
            if (lengths[current] + fullPath[i].Length + 1 <= 20)
            {
                functions[current].Add(fullPath[i]);
                lengths[current] += fullPath[i].Length + 1;
            }
            else
            {
                current++;
                if (current < 3)
                {
                    functions[current].Add(fullPath[i]);
                    lengths[current] += fullPath[i].Length + 1;
                }
            }
            if (current > 2)
            {
                programInput = Array.Empty<int>().ToList();
                return false;
            }
        }
        List<string> main = new();
        for (int i = 0; i < functions.Length; i++)
        {
            if (lengths[i] < 0)
            {
                functions[i].Add("L");
            }
            else
            {
                string functionName = i switch
                {
                    0 => "A",
                    1 => "B",
                    2 => "C",
                    _ => "X"
                };
                if (functionName != "X")
                {
                    main.Add(functionName);
                }
            }
        }
        if (main.Count == 0)
        {
            programInput = Array.Empty<int>().ToList();
            return false;
        }
        programInput = CombineListsToProgramInput(main, functions);
        return true;
    }

    private static List<int> CombineListsToProgramInput(List<string> main, List<string>[] functions)
    {
        string combination = CombineLists(main, functions);
        List<int> result = new();
        foreach (char c in combination)
        {
            result.Add(c);
        }
        return result;
    }

    private static string CombineLists(List<string> main, List<string>[] functions)
    {
        if (functions.Length != 3)
        {
            throw new ArgumentException($"Day 17: Invalid functions array passed to {nameof(CombineLists)}().");
        }
        for (int i = 0; i < functions.Length; i++)
        {
            if (functions[i].Count == 0)
            {
                functions[i].Add("L");
            }
        }
        string M = string.Join(',', main);
        string A = string.Join(',', functions[0]);
        string B = string.Join(',', functions[1]);
        string C = string.Join(',', functions[2]);
        return $"{M}\n{A}\n{B}\n{C}\nn\n";
    }

    private static bool TryRoutines(List<string> fullPath, int lengthA, int lengthC, out List<int>? programInput)
    {
        programInput = null;

        List<string>[] functions = new List<string>[3];
        functions[0] = new();
        functions[1] = new();
        functions[2] = new();
        for (int i = 0; i < lengthA; i++)
        {
            functions[0].Add(fullPath[i]);
        }
        for (int i = fullPath.Count - lengthC; i < fullPath.Count; i++)
        {
            functions[2].Add(fullPath[i]);
        }
        List<string> path = new(fullPath);
        path.ReplaceSequence(functions[0], "A");
        path.ReplaceSequence(functions[2], "C");

        int totalGaps = path.Count(x => x != "A" && x != "C");
        if (totalGaps == 0)
        {
            List<string> main = MainRoutine(path, functions);
            programInput = CombineListsToProgramInput(main, functions);
            return true;
        }

        if (FindValidB(path, functions))
        {
            List<string> main = MainRoutine(path, functions);
            programInput = CombineListsToProgramInput(main, functions);
            return true;
        }

        return false;
    }

    private static List<string> MainRoutine(List<string> path, List<string>[] functions)
    {
        List<string> main = new();
        int i = 0;
        while (i < path.Count)
        {
            switch (path[i])
            {
                case "A":
                case "B":
                case "C":
                    main.Add(path[i]);
                    i += functions[path[i][0] - 65].Count;
                    break;
                default:
                    i = int.MaxValue;
                    break;
            }
        }
        return main;
    }

    private static bool FindValidB(List<string> path, List<string>[] functions)
    {
        List<(int position, int length)> gaps = IdentifyGaps(path);
        foreach ((int position, _) in gaps)
        {
            if (path[position] != path[gaps[0].position])
            {
                return false;
            }
        }
        List<string> B = new();
        for (int i = 0; i < Math.Min(10, gaps[0].length); i++)
        {
            string nextTerm = path[gaps[0].position + i];
            if (nextTerm == "A" || nextTerm == "C")
            {
                break;
            }
            B.Add(nextTerm);
            List<string> testPath = new(path);
            testPath.ReplaceSequence(B, "B");
            int remaining = testPath.Count(x => x != "A" && x != "B" && x != "C");
            if (remaining == 0)
            {
                int lengthB = B.Sum(x => x.ToString().Length) + B.Count - 1;
                if (lengthB <= 20)
                {
                    functions[1] = B;
                    path.ReplaceSequence(B, "B");
                    return true;
                }
                return false;
            }
        }
        return false;
    }

    private static List<(int, int)> IdentifyGaps(List<string> path)
    {
        List<(int, int)> output = new();
        int startOfGap = 0;
        for (int i = 1; i < path.Count; i++)
        {
            if ((path[i - 1] == "A" || path[i - 1] == "C")
                && path[i] != "A" && path[i] != "C")
            {
                startOfGap = i;
                continue;
            }
            if ((path[i] == "A" || path[i] == "C")
                && path[i - 1] != "A" && path[i - 1] != "C")
            {
                output.Add((startOfGap, i - startOfGap));
            }
        }
        return output;
    }
}
