// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using System.Text.RegularExpressions;
using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal partial class Day05 : Day
{
    public static readonly Regex ValidMoveLine = ValidMoveLineRegex();

    [GeneratedRegex("^move (?<quantity>[0-9]{1,2}) from (?<start>[0-9]) to (?<end>[0-9])$")]
    private static partial Regex ValidMoveLineRegex();

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        List<Stack<char>> stacks = InputStacks(input);
        List<(int, int, int)> moves = InputMoves(input);
        foreach ((int quantity, int start, int end) in moves)
        {
            for (int i = 0; i < quantity; i++)
            {
                stacks[end - 1].Push(stacks[start - 1].Pop());
            }
        }
        char[] stackTops = stacks.Select(s => s.Peek()).ToArray();

        stacks = InputStacks(input);
        Stack<char> temp = [];
        foreach ((int quantity, int start, int end) in moves)
        {
            for (int i = 0; i < quantity; i++)
            {
                temp.Push(stacks[start - 1].Pop());
            }
            for (int i = 0; i < quantity; i++)
            {
                stacks[end - 1].Push(temp.Pop());
            }
        }
        char[] correctStackTops = stacks.Select(s => s.Peek()).ToArray();

        return result.WriteAnswers(stackTops, correctStackTops);
    }

    private static List<Stack<char>> InputStacks(string[] input)
    {
        int baseLine = 0;
        for (; baseLine < input.Length; baseLine++)
        {
            if (input[baseLine].StartsWith(" 1   2   3"))
            {
                baseLine--;
                break;
            }
        }
        List<Stack<char>> stacks = [];
        for (int x = 1; x < input[baseLine].Length; x += 4)
        {
            Stack<char> stack = [];
            for (int y = baseLine; y >= 0 && x < input[y].Length && input[y][x] != ' '; y--)
            {
                stack.Push(input[y][x]);
            }
            stacks.Add(stack);
        }
        return stacks;
    }

    private static List<(int, int, int)> InputMoves(string[] input)
    {
        List<(int, int, int)> moves = [];
        foreach (string line in input)
        {
            Match match = ValidMoveLine.Match(line);
            if (match.Success)
            {
                int quantity = int.Parse(match.Groups["quantity"].Value);
                int start = int.Parse(match.Groups["start"].Value);
                int end = int.Parse(match.Groups["end"].Value);
                moves.Add((quantity, start, end));
            }
        }
        return moves;
    }
}
