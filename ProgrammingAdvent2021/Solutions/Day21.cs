// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using System.Text.RegularExpressions;
using ProgrammingAdvent2021.Common;

namespace ProgrammingAdvent2021.Solutions;

internal class Day21 : Day
{
    private static readonly ulong[] _advanceQnties = [ 0, 0, 0, 1, 3, 6, 7, 6, 3, 1 ];

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

        ulong[,,,,] scoreUniverses = new ulong[10, 10, 31, 31, 2];
        scoreUniverses[(player1Start + 9) % 10, (player2Start + 9) % 10, 0, 0, 0] = 1;
        while (Update(scoreUniverses)) { }
        ulong partTwoAnswer = MostWinningUniverses(scoreUniverses);

        return ($"{partOneAnswer}", $"{partTwoAnswer}");
    }

    private static bool Update(ulong[,,,,] scoreUniverses)
    {
        bool updated = false;
        for (int player = 0; player < 2; player++)
        {
            for (int pos1 = 0; pos1 < 10; pos1++)
            {
                for (int pos2 = 0; pos2 < 10; pos2++)
                {
                    for (int score1 = 0; score1 < 21; score1++)
                    {
                        for (int score2 = 0; score2 < 21; score2++)
                        {
                            updated |= UpdateGames(scoreUniverses, pos1, pos2, score1, score2, player);
                        }
                    }
                }
            }
            
        }
        return updated;
    }

    private static bool UpdateGames(ulong[,,,,] scoreUniverses, int pos1, int pos2,
        int score1, int score2, int player)
    {
        ulong current = scoreUniverses[pos1, pos2, score1, score2, player];
        if (current == 0)
        {
            return false;
        }
        scoreUniverses[pos1, pos2, score1, score2, player] = 0;
        if (player == 0)
        {
            for (int advance = 3; advance <= 9; advance++)
            {
                int newPos = (pos1 + advance) % 10;
                int newScore = score1 + newPos + 1;
                scoreUniverses[newPos, pos2, newScore, score2, 1] += current * _advanceQnties[advance];
            }
        }
        else
        {
            for (int advance = 3; advance <= 9; advance++)
            {
                int newPos = (pos2 + advance) % 10;
                int newScore = score2 + newPos + 1;
                scoreUniverses[pos1, newPos, score1, newScore, 0] += current * _advanceQnties[advance];
            }
        }
        return true;
    }

    private static ulong MostWinningUniverses(ulong[,,,,] scoreUniverses)
    {
        ulong player1Wins = 0, player2Wins = 0;
        for (int i = 0; i < scoreUniverses.GetLength(0); i++)
        {
            for (int j = 0; j < scoreUniverses.GetLength(1); j++)
            {
                for (int k = 0; k < scoreUniverses.GetLength(2); k++)
                {
                    for (int l = 0; l < scoreUniverses.GetLength(3); l++)
                    {
                        player1Wins += scoreUniverses[i, j, k, l, 1];
                        player2Wins += scoreUniverses[i, j, k, l, 0];
                    }
                }
            }
        }
        return Math.Max(player1Wins, player2Wins);
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
