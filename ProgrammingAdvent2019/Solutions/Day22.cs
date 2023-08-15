// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day22 : Day
{
    private static readonly Regex _validLine = new("^(deal into new stack|deal with increment (?<increment>[0-9]{1,2})|cut (?<cut>-?[0-9]{1,4}))$");

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        foreach (string line in inputLines)
        {
            if (!_validLine.IsMatch(line))
            {
                errorMessage = $"Invalid input line \"{line.Left(20, true)}\".";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        (int, int)[] instructions = new(int, int)[inputLines.Length];
        for (int i = 0; i < instructions.Length; i++)
        {
            string line = inputLines[i];
            Match match = _validLine.Match(line);
            if (match.Groups["increment"].Success)
            {
                int N = int.Parse(match.Groups["increment"].Value);
                instructions[i] = (2, N);
            }
            else if (match.Groups["cut"].Success)
            {
                int N = int.Parse(match.Groups["cut"].Value);
                instructions[i] = (1, N);
            }
            else
            {
                instructions[i] = (0, 0);
            }
        }

        string partOneAnswer;
        if (exampleModifier is null)
        {
            partOneAnswer = ShuffledPosition(2019, 10_007, instructions).ToString();
        }
        else
        {
            int[] results = new int[10];
            for (int i = 0; i < 10; i++)
            {
                results[ShuffledPosition(i, 10, instructions)] = i;
            }
            partOneAnswer = string.Join(' ', results);
        }

        return output.WriteAnswers(partOneAnswer, null);
    }

    private static long NewStack(long position, long deckSize)
    {
        return deckSize - position - 1;
    }

    private static long Cut(long position, long deckSize, int N)
    {
        if (N > 0)
        {
            if (position < N)
            {
                return deckSize - N + position;
            }
            else
            {
                return position - N;
            }
        }
        else if (N < 0)
        {
            if (position < deckSize + N)
            {
                return position - N;
            }
            else
            {
                return position - deckSize - N;
            }
        }
        else return position;
    }

    private static long Increment(long position, long deckSize, int N)
    {
        return (position * N) % deckSize;
    }

    private static long ApplyTechnique((int, int) technique, long position, long deckSize)
    {
        return technique.Item1 switch
        {
            2 => Increment(position, deckSize, technique.Item2),
            1 => Cut(position, deckSize, technique.Item2),
            _ => NewStack(position, deckSize),
        };
    }

    private static long ShuffledPosition(long position, long deckSize, (int, int)[] instructions)
    {
        long pos = position;
        foreach ((int, int) technique in instructions)
        {
            pos = ApplyTechnique(technique, pos, deckSize);
        }
        return pos;
    }
}
