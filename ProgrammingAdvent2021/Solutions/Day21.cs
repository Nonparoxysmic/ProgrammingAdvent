// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using System.Text.RegularExpressions;
using ProgrammingAdvent2021.Common;

namespace ProgrammingAdvent2021.Solutions;

internal class Day21 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        Match player1Match = GeneratedRegex.ValidDay21InputLine.Match(input[0]);
        Match player2Match = GeneratedRegex.ValidDay21InputLine.Match(input[1]);
        if (!player1Match.Success || !player2Match.Success)
        {
            return ("Invalid input", "n/a");
        }
        int player1Start = int.Parse(player1Match.Groups["start"].Value);
        int player2Start = int.Parse(player2Match.Groups["start"].Value);

        int[] positions = [(player1Start + 9) % 10, (player2Start + 9) % 10];
        int[] scores = [0, 0];
        int currentPlayer = 0, rolls = 0;
        DeterministicD100 die = new();
        while (true)
        {
            positions[currentPlayer] += die.Roll() + die.Roll() + die.Roll();
            positions[currentPlayer] %= 10;
            scores[currentPlayer] += positions[currentPlayer] + 1;
            rolls += 3;
            if (scores[currentPlayer] >= 1000)
            {
                break;
            }
            currentPlayer = (currentPlayer + 1) % 2;
        }
        int partOneAnswer = rolls * scores[(currentPlayer + 1) % 2];

        return ($"{partOneAnswer}", "n/a");
    }

    private class DeterministicD100
    {
        int _current = 1;

        public int Roll()
        {
            if (_current > 100)
            {
                _current = 1;
            }
            return _current++;
        }
    }
}
