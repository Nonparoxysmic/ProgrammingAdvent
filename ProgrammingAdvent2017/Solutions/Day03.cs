// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System;
using System.Diagnostics;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day03 : Day
    {
        // OEIS Sequence A141481
        // https://oeis.org/A141481
        internal static int[] A141481 = new int[] { 1, 1, 2, 4, 5, 10, 11, 23, 25, 26, 54, 57, 59,
            122, 133, 142, 147, 304, 330, 351, 362, 747, 806, 880, 931, 957, 1968, 2105, 2275, 2391,
            2450, 5022, 5336, 5733, 6155, 6444, 6591, 13486, 14267, 15252, 16295, 17008, 17370,
            35487, 37402, 39835, 42452, 45220, 47108, 48065, 98098, 103128, 109476, 116247, 123363,
            128204, 130654, 266330, 279138, 295229, 312453, 330785, 349975, 363010, 369601, 752688,
            787032, 830037, 875851, 924406, 975079, 1009457, 1026827, 2089141, 2179400, 2292124,
            2411813, 2539320, 2674100, 2814493, 2909666, 2957731, 6013560, 6262851, 6573553,
            6902404, 7251490, 7619304, 8001525, 8260383, 8391037, 17048404, 17724526, 18565223,
            19452043, 20390510, 21383723, 22427493, 23510079, 24242690, 24612291, 49977270,
            51886591, 54256348, 56749268, 59379562, 62154898, 65063840, 68075203, 70111487,
            71138314, 144365769, 149661137, 156221802, 163105139, 170348396, 177973629, 186001542,
            194399801, 203081691, 208949088, 211906819, 429827198, 445061340, 463911304, 483650112,
            504377559, 526150757, 549023076, 572904288, 597557233, 614208653, 622599690, 1262247784,
            1305411751, 1358749904, 1414491696, 1472899472, 1534125748, 1598327474, 1665648769,
            1735829031, 1808194091, 1857049072, 1881661363 };

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
            if (!int.TryParse(input, out int inputValue))
            {
                output.WriteError("Input is not an integer.", sw);
                return output;
            }
            if (inputValue < 1)
            {
                output.WriteError("Input is less than one.", sw);
                return output;
            }

            (int, int) inputValueCoords = FindCoordinates(inputValue);
            int steps = Math.Abs(inputValueCoords.Item1) + Math.Abs(inputValueCoords.Item2);

            int partTwoValue = NextInSequence(inputValue);
            
            sw.Stop();
            output.WriteAnswers(steps, partTwoValue, sw);
            return output;
        }

        private (int, int) FindCoordinates(int number)
        {
            if (number < 1)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (number == 1)
            {
                return (0, 0);
            }
            int layerSize = SquareSize(number);
            int x = layerSize / 2;
            int y = 1 - x;
            int squareValue = (layerSize - 2) * (layerSize - 2) + 1;
            int[] corners = new int[4];
            corners[3] = layerSize * layerSize;
            corners[2] = corners[3] - layerSize + 1;
            corners[1] = corners[2] - layerSize + 1;
            corners[0] = corners[1] - layerSize + 1;
            while (squareValue < corners[0])
            {
                if (squareValue == number)
                {
                    return (x, y);
                }
                squareValue++;
                y++;
            }
            while (squareValue < corners[1])
            {
                if (squareValue == number)
                {
                    return (x, y);
                }
                squareValue++;
                x--;
            }
            while (squareValue < corners[2])
            {
                if (squareValue == number)
                {
                    return (x, y);
                }
                squareValue++;
                y--;
            }
            while (squareValue < corners[3])
            {
                if (squareValue == number)
                {
                    return (x, y);
                }
                squareValue++;
                x++;
            }
            return (x, y);
        }

        private int SquareSize(int number)
        {
            for (int size = 1; size < 46_340; size += 2)
            {
                if ((size * size) >= number) { return size; }
            }
            throw new ArgumentOutOfRangeException();
        }

        private int NextInSequence(int number)
        {
            for (int i = 0; i < A141481.Length; i++)
            {
                if (A141481[i] > number)
                {
                    return A141481[i];
                }
            }
            throw new ArgumentOutOfRangeException();
        }
    }
}
