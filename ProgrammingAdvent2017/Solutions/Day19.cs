// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System.Diagnostics;
using System.Text;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day19 : Day
    {
        private readonly Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int( 0, -1),  // [0] north
            new Vector2Int( 1,  0),  // [1] east
            new Vector2Int( 0,  1),  // [2] south
            new Vector2Int(-1,  0)   // [3] west
        };

        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (input.Trim() == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }
            string[] inputLines = input.ToLines();
            if (inputLines.Length < 3 || inputLines[0].Length < 3)
            {
                output.WriteError("Insufficient input.", sw);
                return output;
            }
            for (int i = 0; i < inputLines.Length; i++)
            {
                if (inputLines[i].Length != inputLines[0].Length
                    || inputLines[i][0] != ' ' || inputLines[i][^1] != ' ')
                {
                    output.WriteError($"Line {i} is not valid.", sw);
                    return output;
                }
            }
            if (inputLines[0].Trim() != "|")
            {
                output.WriteError($"First line is not valid.", sw);
                return output;
            }
            if (inputLines[^1].Trim() != "")
            {
                output.WriteError($"Final line is not valid.", sw);
                return output;
            }

            Vector2Int position = Vector2Int.Zero;
            int direction = 2;
            for (int i = 0; i < inputLines[0].Length; i++)
            {
                if (inputLines[0][i] == '|')
                {
                    position.X = i;
                    break;
                }
            }
            StringBuilder partOneAnswer = new StringBuilder();
            bool followingPath = true;
            while (followingPath)
            {
                Vector2Int lookahead = position + directions[direction];
                char lookaheadChar = inputLines[lookahead.Y][lookahead.X];
                switch (lookaheadChar)
                {
                    case ' ':
                        int left = (direction + 3) % 4;
                        Vector2Int lookLeft = position + directions[left];
                        char leftChar = inputLines[lookLeft.Y][lookLeft.X];
                        if (leftChar != ' ')
                        {
                            direction = left;
                            break;
                        }
                        int right = (direction + 5) % 4;
                        Vector2Int lookRight = position + directions[right];
                        char rightChar = inputLines[lookRight.Y][lookRight.X];
                        if (rightChar != ' ')
                        {
                            direction = right;
                            break;
                        }
                        followingPath = false;
                        break;
                    case '|':
                    case '-':
                    case '+':
                        position = lookahead;
                        break;
                    default:
                        partOneAnswer.Append(lookaheadChar);
                        position = lookahead;
                        break;
                }
            }

            sw.Stop();
            output.WriteAnswers(partOneAnswer, null, sw);
            return output;
        }
    }
}
