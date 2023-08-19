// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day24 : Day
{
    private static readonly Regex _validLine = new("^[#.]{5}$");
    private static readonly uint _maskGrid = 0x1FFFFFF;
    private static readonly uint _maskL = 0b01111_01111_01111_01111_01111;
    private static readonly uint _maskR = 0b11110_11110_11110_11110_11110;

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        if (inputLines.Length != 5)
        {
            errorMessage = "Input has the wrong number of lines.";
            return false;
        }
        foreach (string line in inputLines)
        {
            if (line.Length == 0)
            {
                errorMessage = "Input contains an empty line.";
                return false;
            }
            if (!_validLine.IsMatch(line))
            {
                errorMessage = $"Input line \"{line.Left(20, true)}\" is not valid.";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        uint currentState = ReadInput(inputLines);
        HashSet<uint> seenStates = new();
        while (seenStates.Add(currentState))
        {
            currentState = NextState(currentState);
        }
        uint partOneAnswer = currentState;

        return output.WriteAnswers(partOneAnswer, null);
    }

    private static uint ReadInput(string[] inputLines)
    {
        uint output = 0;
        int i = 0;
        for (int y = 0; y < inputLines.Length; y++)
        {
            for (int x = 0; x < inputLines[y].Length; x++)
            {
                if (inputLines[y][x] == '#')
                {
                    output |= 1U << i;
                }
                i++;
            }
        }
        return output;
    }

    private static uint NextState(uint currentState)
    {
        //   0  1  2  3  4
        //   5  6  7  8  9
        //  10 11 12 13 14
        //  15 16 17 18 19
        //  20 21 22 23 24

        uint up = currentState >> 5;
        uint down = (currentState << 5) & _maskGrid;
        uint left = (currentState >> 1) & _maskL;
        uint right = (currentState << 1) & _maskR;

        uint neighbors0 = 0;
        uint neighbors1 = 0;
        uint neighbors2 = 0;
        uint carry0 = neighbors0 & up;
        uint carry1 = neighbors1 & carry0;
        neighbors0 ^= up;
        neighbors1 ^= carry0;
        neighbors2 |= carry1;
        carry0 = neighbors0 & down;
        carry1 = neighbors1 & carry0;
        neighbors0 ^= down;
        neighbors1 ^= carry0;
        neighbors2 |= carry1;
        carry0 = neighbors0 & left;
        carry1 = neighbors1 & carry0;
        neighbors0 ^= left;
        neighbors1 ^= carry0;
        neighbors2 |= carry1;
        carry0 = neighbors0 & right;
        carry1 = neighbors1 & carry0;
        neighbors0 ^= right;
        neighbors1 ^= carry0;
        neighbors2 |= carry1;

        uint survivingBugs = currentState & neighbors0 & ~neighbors1 & ~neighbors2;
        uint newBugs = ~currentState & (neighbors0 ^ neighbors1) & ~neighbors2;
        return survivingBugs | newBugs;
    }
}
