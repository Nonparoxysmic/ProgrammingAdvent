// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ProgrammingAdvent2016
{
    public class Day21 : Day
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
            if (inputLines.Length == 0)
            {
                solution.WriteSolution(1, "ERROR: No input.", 0);
                return solution;
            }
            stopwatch.Start();

            var swapPosition = new Regex(@"^swap position [0-7] with position [0-7]");
            var swapLetter   = new Regex(@"^swap letter [a-h] with letter [a-h]");
            var rotateLeft   = new Regex(@"^rotate left [0-7] step");
            var rotateRight  = new Regex(@"^rotate right [0-7] step");
            var rotate       = new Regex(@"^rotate based on position of letter [a-h]");
            var reverse      = new Regex(@"^reverse positions [0-7] through [0-7]");
            var move         = new Regex(@"^move position [0-7] to position [0-7]");

            var letters = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
            foreach (string line in inputLines)
            {
                if (swapPosition.IsMatch(line))
                {
                    SwapPositions(ref letters, line[14], line[30]);
                    continue;
                }
                else if (swapLetter.IsMatch(line))
                {
                    int x = 0;
                    int y = 0;
                    for (int i = 0; i < letters.Length; i++)
                    {
                        if (letters[i] == line[12])
                        {
                            x = i;
                        }
                        if (letters[i] == line[26])
                        {
                            y = i;
                        }
                    }
                    SwapPositions(ref letters, x, y);
                    continue;
                }
                else if (rotateLeft.IsMatch(line))
                {
                    Rotate(ref letters, line[12], 'L');
                    continue;
                }
                else if (rotateRight.IsMatch(line))
                {
                    Rotate(ref letters, line[13]);
                    continue;
                }
                else if (rotate.IsMatch(line))
                {
                    int index = 0;
                    for (int i = 0; i < letters.Length; i++)
                    {
                        if (letters[i] == line[35])
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index >= 4) { index++; }
                    Rotate(ref letters, index + 1);
                    continue;
                }
                else if (reverse.IsMatch(line))
                {
                    Reverse(ref letters, line[18], line[28]);
                    continue;
                }
                else if (move.IsMatch(line))
                {
                    Move(ref letters, line[14], line[28]);
                    continue;
                }
                solution.WriteSolution(1, "ERROR: Invalid line in input: \"" + line + "\"", stopwatch.ElapsedMilliseconds);
                return solution;
            }
            solution.WriteSolution(1, new string(letters), stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }

        int DigitToInt(char digit)
        {
            return digit - 48;
        }

        void SwapPositions(ref char[] letters, char a, char b)
        {
            SwapPositions(ref letters, DigitToInt(a), DigitToInt(b));
        }

        void SwapPositions(ref char[] letters, int x, int y)
        {
            if (x == y) return;
            var temp = letters[x];
            letters[x] = letters[y];
            letters[y] = temp;
        }

        void Rotate(ref char[] letters, char steps, char direction = 'R')
        {
            Rotate(ref letters, DigitToInt(steps), direction);
        }

        void Rotate(ref char[] letters, int steps, char direction = 'R')
        {
            steps %= letters.Length;
            if (steps == 0) { return; }

            var original = new char[letters.Length];
            Array.Copy(letters, original, letters.Length);

            int startPos = direction == 'R' ? steps : letters.Length - steps;
            Array.Copy(original, 0, letters, startPos, letters.Length - startPos);
            Array.Copy(original, letters.Length - startPos, letters, 0, startPos);
        }

        void Reverse(ref char[] letters, char a, char b)
        {
            Reverse(ref letters, DigitToInt(a), DigitToInt(b));
        }

        void Reverse(ref char[] letters, int start, int stop)
        {
            if (start == stop) { return; }
            if (start > stop)
            {
                Reverse(ref letters, stop, start);
                return;
            }

            var original = new char[letters.Length];
            Array.Copy(letters, original, letters.Length);

            for (int i = start; i <= stop; i++)
            {
                letters[i] = original[stop + start - i];
            }
        }

        void Move(ref char[] letters, char before, char after)
        {
            Move(ref letters, DigitToInt(before), DigitToInt(after));
        }

        void Move(ref char[] letters, int before, int after)
        {
            if (before == after) { return; }
            Reverse(ref letters, before, after);
            if (Math.Abs(before - after) == 1) { return; }
            else if (before > after)
            {
                Reverse(ref letters, before, after + 1);
            }
            else
            {
                Reverse(ref letters, before, after - 1);
            }
        }
    }
}
