// Puzzle solution by Nonparoxysmic
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
        List<int> programInput = PathToProgramInput(fullPath);
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

    private static List<int> PathToProgramInput(List<string> fullPath)
    {
        return new List<int>() { 65, 10, 76, 10, 76, 10, 76, 10, 110, 10 }; // PLACEHOLDER
    }
}
