// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day23 : Day
{
    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        if (input[0].Length < 5)
        {
            errorMessage = $"Input \"{input[0].Left(20, true)}\" is too short.";
            return false;
        }
        HashSet<char> digits = new();
        foreach (char c in input[0])
        {
            if (c < '0' || c > '9')
            {
                errorMessage = $"Invalid character \'{c}\' in input.";
                return false;
            }
            if (!digits.Add(c))
            {
                errorMessage = $"Input contains duplicate character \'{c}\'.";
                return false;
            }
        }
        for (int i = 1; i <= digits.Count; i++)
        {
            if (!digits.Contains((char)(i + '0')))
            {
                errorMessage = $"Input does not contain digits 1 through {digits.Count}.";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        int[] state = ReadInput(input[0]);
        state = DoMoves(100, state);
        string partOneAnswer = CupOrderAfterOne(state);

        return output.WriteAnswers(partOneAnswer, null);
    }

    private static int[] ReadInput(string input)
    {
        int[] state = new int[input.Length + 1];
        // Index zero contains the label of the current cup.
        state[0] = input[0] - '0';
        for (int i = 0; i < input.Length; i++)
        {
            int cup = input[i] - '0';
            int nextCup = input[(i + 1) % input.Length] - '0';
            state[cup] = nextCup;
        }
        return state;
    }

    private static int[] DoMoves(int iterations, int[] state)
    {
        int highestCard = state.Length - 1;
        for (int i = 0; i < iterations; i++)
        {
            // Get the labels of the current and next three cups.
            int current = state[0];
            int next = state[current];
            int nextNext = state[next];
            int nextNextNext = state[nextNext];
            // Remove the three following cups from the loop.
            state[current] = state[nextNextNext];
            // Choose the destination cup.
            int destination = current == 1 ? highestCard : current - 1;
            while (destination == next || destination == nextNext || destination == nextNextNext)
            {
                destination--;
                if (destination < 1)
                {
                    destination = highestCard;
                }
            }
            // Insert the three picked up cups after the destination cup.
            state[nextNextNext] = state[destination];
            state[destination] = next;
            // Choose the new current cup.
            state[0] = state[current];
        }
        return state;
    }

    private static string CupOrderAfterOne(int[] state)
    {
        List<char> output = new();
        int current = state[1];
        while (current != 1 && output.Count < 32)
        {
            output.Add((char)(current + '0'));
            current = state[current];
        }
        return string.Join(null, output);
    }
}
