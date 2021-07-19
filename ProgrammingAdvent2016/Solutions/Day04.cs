// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ProgrammingAdvent2016
{
    public class Day04 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            string[] inputLines = input.ToLines();
            stopwatch.Start();

            int roomNumberSum = 0;
            foreach (string line in inputLines)
            {
                int delimiterPos = line.LastIndexOf('-');
                if (delimiterPos < 0) continue;
                string roomName = line.Substring(0, delimiterPos);
                string roomData = line.Substring(delimiterPos + 1).TrimEnd(']');
                int checksumPos = roomData.IndexOf('[');
                if (checksumPos < 0) continue;
                if (int.TryParse(roomData.Substring(0, checksumPos), out int roomNumber))
                {
                    string checksum = roomData.Substring(checksumPos + 1);
                    if (checksum == MostCommonLowercaseLetters(roomName))
                    {
                        roomNumberSum += roomNumber;
                    }
                }
            }
            solution.WriteSolution(1, roomNumberSum.ToString(), stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }

        string MostCommonLowercaseLetters(string name)
        {
            int[] letterCount = new int[26];
            foreach (char c in name)
            {
                if (c >= 97 && c <= 122)
                {
                    letterCount[c - 97]++;
                }
            }
            StringBuilder sb = new StringBuilder();
            while (sb.Length < 5)
            {
                int highest = letterCount.Max();
                if (highest == 0) break;
                for (char c = (char)97; c <= 122; c++)
                {
                    if (letterCount[c - 97] == highest)
                    {
                        sb.Append(c);
                        if (sb.Length >= 5) break;
                        letterCount[c - 97] = 0;
                    }
                }
            }
            return sb.ToString();
        }
    }
}
