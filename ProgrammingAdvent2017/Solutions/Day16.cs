// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day16 : Day
    {
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

            char[] programs = new char[16];
            for (int i = 0; i < programs.Length; i++)
            {
                programs[i] = (char)('a' + i);
            }

            string[] inputMoves = input.Split(',');
            DanceMove[] danceMoves = new DanceMove[inputMoves.Length];
            for (int i = 0; i < inputMoves.Length; i++)
            {
                string move = inputMoves[i];
                if (!Regex.IsMatch(move, @"^(s\d+$|x\d+/\d+$|p[a-p]/[a-p]$)"))
                {
                    output.WriteError($"Not a valid move: \"{move}\".", sw);
                    return output;
                }
                switch (move[0])
                {
                    case 's':
                        int steps = int.Parse(move[1..]);
                        if (steps < 0 || steps > programs.Length - 1)
                        {
                            output.WriteError($"Not a valid move: \"{move}\".", sw);
                            return output;
                        }
                        danceMoves[i] = new SpinMove(steps);
                        break;
                    case 'x':
                        int posA = int.Parse(Regex.Match(move, @"(?<=x)\d+(?=/)").Value);
                        int posB = int.Parse(Regex.Match(move, @"(?<=/)\d+$").Value);
                        if (posA < 0 || posB < 0
                            || posA >= programs.Length || posB >= programs.Length)
                        {
                            output.WriteError($"Not a valid move: \"{move}\".", sw);
                            return output;
                        }
                        danceMoves[i] = new ExchangeMove(posA, posB);
                        break;
                    case 'p':
                        danceMoves[i] = new PartnerMove(move[1], move[3]);
                        break;
                }
            }

            foreach (DanceMove move in danceMoves)
            {
                move.DoMove(ref programs);
            }

            sw.Stop();
            output.WriteAnswers(programs, null, sw);
            return output;
        }

        internal static void SwitchArrayElements<T>(ref T[] array, int positionA, int positionB)
        {
            T temp = array[positionA];
            array[positionA] = array[positionB];
            array[positionB] = temp;
        }

        private abstract class DanceMove
        {
            internal abstract void DoMove(ref char[] programs);
        }

        private class SpinMove : DanceMove
        {
            private readonly int steps;

            public SpinMove(int steps)
            {
                this.steps = steps;
            }

            internal override void DoMove(ref char[] programs)
            {
                int len = programs.Length;
                char[] output = new char[len];
                for (int i = 0; i < len; i++)
                {
                    output[(i + steps) % len] = programs[i];
                }
                programs = output;
            }
        }

        private class ExchangeMove : DanceMove
        {
            private readonly int posA;
            private readonly int posB;

            public ExchangeMove(int positionA, int positionB)
            {
                posA = positionA;
                posB = positionB;
            }

            internal override void DoMove(ref char[] programs)
            {
                SwitchArrayElements(ref programs, posA, posB);
            }
        }

        private class PartnerMove : DanceMove
        {
            private readonly char programA;
            private readonly char programB;

            public PartnerMove(char programA, char programB)
            {
                this.programA = programA;
                this.programB = programB;
            }

            internal override void DoMove(ref char[] programs)
            {
                int posA = System.Array.IndexOf(programs, programA);
                int posB = System.Array.IndexOf(programs, programB);
                SwitchArrayElements(ref programs, posA, posB);
            }
        }
    }
}
