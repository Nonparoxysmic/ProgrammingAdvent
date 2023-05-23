// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day17 : Day
{
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
            }
        }

        return output.WriteAnswers(sumOfAlignmentParameters, null);
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
}
