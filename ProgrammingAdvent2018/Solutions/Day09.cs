// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day09 : Day
    {
        private readonly Regex validInput = new Regex(@"^([0-9]+) players; last marble is worth ([0-9]+) points$");

        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            input = input.Trim();
            if (input == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }
            Match validMatch = validInput.Match(input);
            if (!validMatch.Success)
            {
                output.WriteError("Invalid input.", sw);
                return output;
            }
            int numberOfPlayers = int.Parse(validMatch.Groups[1].Value);
            int lastMarbleValue = int.Parse(validMatch.Groups[2].Value);

            int winningScore = WinningScore(numberOfPlayers, lastMarbleValue);

            sw.Stop();
            output.WriteAnswers(winningScore, null, sw);
            return output;
        }

        private int WinningScore(int numberOfPlayers, int lastMarbleValue)
        {
            if (numberOfPlayers < 1) { return -1; }
            if (lastMarbleValue < 23) { return 0; }
            SortedDictionary<int, int> scores = new SortedDictionary<int, int>();
            for (int i = 1; i <= numberOfPlayers; i++)
            {
                scores.Add(i, 0);
            }
            Marble zeroMarble = new Marble(0);
            zeroMarble.NextMarble = zeroMarble;
            zeroMarble.PreviousMarble = zeroMarble;
            Marble currentMarble = zeroMarble;
            for (int i = 1; i <= lastMarbleValue; i++)
            {
                if (i % 23 == 0)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        currentMarble = currentMarble.PreviousMarble;
                    }
                    int currentPlayer = (i - 1) % numberOfPlayers + 1;
                    scores[currentPlayer] += i + currentMarble.NextMarble.Value;
                    currentMarble.NextMarble = currentMarble.NextMarble.NextMarble;
                    currentMarble.NextMarble.PreviousMarble = currentMarble;
                    currentMarble = currentMarble.NextMarble;
                }
                else
                {
                    Marble newMarble = new Marble(i)
                    {
                        NextMarble = currentMarble.NextMarble.NextMarble,
                        PreviousMarble = currentMarble.NextMarble
                    };
                    newMarble.NextMarble.PreviousMarble = newMarble;
                    newMarble.PreviousMarble.NextMarble = newMarble;
                    currentMarble = newMarble;
                }
            }
            return scores.Values.Max();
        }

        private class Marble
        {
            public int Value { get; private set; }
            public Marble NextMarble { get; set; }
            public Marble PreviousMarble { get; set; }

            public Marble(int value)
            {
                Value = value;
            }
        }
    }
}
