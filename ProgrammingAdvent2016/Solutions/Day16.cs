// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace ProgrammingAdvent2016
{
    public class Day16 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            if (input.Length == 0)
            {
                solution.WriteSolution(1, "ERROR: No input.", 0);
                return solution;
            }
            stopwatch.Start();

            input = input.Trim();
            if (!Regex.IsMatch(input, @"^[01]+$"))
            {
                solution.WriteSolution(1, "ERROR: Invalid input.", 0);
                return solution;
            }

            string partOneSolution = GenerateChecksum(input, 272);
            solution.WriteSolution(1, partOneSolution, stopwatch.ElapsedMilliseconds);

            string partTwoSolution = GenerateChecksum(input, 35651584);
            solution.WriteSolution(2, partTwoSolution, stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }

        string GenerateChecksum(string generatorInput, int sequenceLength)
        {
            var sequenceGenerator = new SequenceGenerator(generatorInput);

            int checksumLength = sequenceLength;
            int iterations = 0;
            while (checksumLength % 2 == 0)
            {
                checksumLength /= 2;
                iterations++;
            }

            Queue<char>[] queues = new Queue<char>[iterations + 1];
            for (int i = 0; i < queues.Length; i++)
            {
                queues[i] = new Queue<char>();
            }

            while (true)
            {
                queues[0].Enqueue(sequenceGenerator.Next());
                queues[0].Enqueue(sequenceGenerator.Next());
                for (int i = 1; i < queues.Length; i++)
                {
                    if (queues[i - 1].Count > 1)
                    {
                        char a = queues[i - 1].Dequeue();
                        char b = queues[i - 1].Dequeue();
                        if (a == b) { queues[i].Enqueue('1'); }
                        else { queues[i].Enqueue('0'); }
                    }
                }
                if (queues[queues.Length - 1].Count >= checksumLength)
                {
                    break;
                }
            }

            StringBuilder sb = new StringBuilder();
            while (queues[queues.Length - 1].Count > 0)
            {
                sb.Append(queues[queues.Length - 1].Dequeue());
            }
            return sb.ToString();
        }
    }
}

class SequenceGenerator
{
    readonly static string zeroSequence = "0010011000110110001001110011011"
        + "000100110001101110010011100110110001001100011011"
        + "000100111001101110010011000110111001001110011011";

    readonly string seed;

    int direction = 1;
    int position = -1;
    SequenceGenerator subsequenceGenerator = null;

    public SequenceGenerator(string seed)
    {
        this.seed = seed;
    }

    public char Next()
    {
        position += direction;
        if (position < 0 || position >= seed.Length)
        {
            direction *= -1;
            if (subsequenceGenerator == null)
            {
                subsequenceGenerator = new SequenceGenerator(zeroSequence);
            }
            return subsequenceGenerator.Next();
        }
        char output = seed[position];
        if (direction < 0)
        {
            if (output == '0') { return '1'; }
            else if (output == '1') { return '0'; }
        }
        return output;
    }
}
