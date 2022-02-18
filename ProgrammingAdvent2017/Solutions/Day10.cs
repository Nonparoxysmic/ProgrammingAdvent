// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System;
using System.Diagnostics;
using System.Text;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day10 : Day
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

            int partOneAnswer = ProcessInput(inputValues);

            string partTwoAnswer = KnotHash(input);

            sw.Stop();
            output.WriteAnswers(partOneAnswer, partTwoAnswer, sw);
            return output;
        }

        private int ProcessInput(int[] inputValues)
        {
            int[] elements = new int[256];
            for (int i = 0; i < 256; i++)
            {
                elements[i] = i;
            }

            int currentPos = 0;
            int skipSize = 0;
            foreach (int n in inputValues)
            {
                ReverseElements(elements, n, currentPos);
                currentPos += n + skipSize;
                skipSize++;
            }
            return elements[0] * elements[1];
        }

        internal static void ReverseElements(int[] elements, int length, int currentPos = 0)
        {
            if (length < 2) { return; }
            int startPos = currentPos;
            int endPos = currentPos + length - 1;
            while (startPos < endPos)
            {
                int temp = elements[startPos % elements.Length];
                elements[startPos % elements.Length] = elements[endPos % elements.Length];
                elements[endPos % elements.Length] = temp;
                startPos++;
                endPos--;
            }
        }

        internal static string KnotHash(string input)
        {
            byte[] output = KnotHashBytes(input);
            return BitConverter.ToString(output).Replace("-", "").ToLower();
        }

        internal static byte[] KnotHashBytes(string input)
        {
            byte[] suffix = new byte[] { 17, 31, 73, 47, 23 };
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] lengths = new byte[inputBytes.Length + 5];
            for (int i = 0; i < inputBytes.Length; i++)
            {
                lengths[i] = inputBytes[i];
            }
            for (int i = inputBytes.Length; i < lengths.Length; i++)
            {
                lengths[i] = suffix[i - inputBytes.Length];
            }
            int[] elements = new int[256];
            for (int i = 0; i < 256; i++)
            {
                elements[i] = i;
            }

            int currentPos = 0;
            int skipSize = 0;
            for (int i = 0; i < 64; i++)
            {
                foreach (int n in lengths)
                {
                    ReverseElements(elements, n, currentPos);
                    currentPos += n + skipSize;
                    skipSize++;
                }
            }

            byte[] output = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                int xor = elements[i * 16];
                for (int j = 1; j < 16; j++)
                {
                    xor ^= elements[(i * 16) + j];
                }
                output[i] = (byte)xor;
            }
            return output;
        }
    }
}
