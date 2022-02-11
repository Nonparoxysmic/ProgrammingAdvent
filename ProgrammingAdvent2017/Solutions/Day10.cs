// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System.Diagnostics;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day10 : Day
    {
        private int[] elements;

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
            string[] inputTerms = input.Split(',');
            int[] inputValues = new int[inputTerms.Length];
            for (int i = 0; i < inputTerms.Length; i++)
            {
                if (!int.TryParse(inputTerms[i], out int n) || n < 0)
                {
                    output.WriteError($"Invalid input \"{inputTerms[i]}\".", sw);
                    return output;
                }
                inputValues[i] = n;
            }

            elements = new int[256];
            for (int i = 0; i < 256; i++)
            {
                elements[i] = i;
            }

            int partOneAnswer = ProcessInput(inputValues);

            sw.Stop();
            output.WriteAnswers(partOneAnswer, null, sw);
            return output;
        }

        private int ProcessInput(int[] inputValues)
        {
            int currentPos = 0;
            int skipSize = 0;
            foreach (int n in inputValues)
            {
                ReverseElements(n, currentPos);
                currentPos += n + skipSize;
                skipSize++;
            }
            return elements[0] * elements[1];
        }

        private void ReverseElements(int length, int currentPos)
        {
            if (length < 2) { return; }
            int startPos = currentPos;
            int endPos = currentPos + length - 1;
            while (startPos < endPos)
            {
                int temp = elements[startPos % 256];
                elements[startPos % 256] = elements[endPos % 256];
                elements[endPos % 256] = temp;
                startPos++;
                endPos--;
            }
        }
    }
}
