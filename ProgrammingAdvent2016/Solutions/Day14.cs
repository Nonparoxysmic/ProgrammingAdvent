// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ProgrammingAdvent2016
{
    public class Day14 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        readonly Dictionary<byte, BytePair> byteConversions = new Dictionary<byte, BytePair>();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            input = input.Trim();
            if (input.Length == 0)
            {
                solution.WriteSolution(1, "ERROR: No input.", 0);
                return solution;
            }
            stopwatch.Start();

            InitializeByteConversions();

            int partOneSolution = FinalIndexOfSearch(input, 0);
            solution.WriteSolution(1, partOneSolution, stopwatch.ElapsedMilliseconds);

            int partTwoSolution = FinalIndexOfSearch(input, 2016);
            solution.WriteSolution(2, partTwoSolution, stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }

        int FinalIndexOfSearch(string input, int stretchingIterations)
        {
            var hashDataQueue = new Queue<HashData>();
            int keysFound = 0;
            int result = -1;
            for (int i = 0; i < 1000; i++)
            {
                hashDataQueue.Enqueue(ComputeHashData(input + i, stretchingIterations));
            }
            for (int i = 1000; i < int.MaxValue; i++)
            {
                hashDataQueue.Enqueue(ComputeHashData(input + i, stretchingIterations));

                var currentHashData = hashDataQueue.Dequeue();

                foreach (var hd in hashDataQueue)
                {
                    foreach (char c in hd.repeatsOfFive)
                    {
                        if (c == currentHashData.firstRepeatOfThree)
                        {
                            keysFound++;
                            goto DoneSearchingQueue;
                        }
                    }
                }
                DoneSearchingQueue:
                if (keysFound >= 64)
                {
                    result = i - 1000;
                    break;
                }
            }
            return result;
        }

        HashData ComputeHashData(string inputData, int stretchingIterations)
        {
            if (stretchingIterations < 1)
            {
                return new HashData(AdventMD5.ComputeHash(Encoding.UTF8.GetBytes(inputData)));
            }

            byte[] hash = AdventMD5.ComputeHash(Encoding.UTF8.GetBytes(inputData));
            for (int count = 0; count < stretchingIterations; count++)
            {
                byte[] hashChars = new byte[hash.Length * 2];
                for (int i = 0; i < hash.Length; i++)
                {
                    var bytePair = byteConversions[hash[i]];
                    hashChars[i * 2] = bytePair.a;
                    hashChars[i * 2 + 1] = bytePair.b;
                }
                hash = AdventMD5.ComputeHash(hashChars);
            }
            return new HashData(hash);
        }

        void InitializeByteConversions()
        {
            for (int i = 0; i < 256; i++)
            {
                string hex = ((byte)i).ToString("x2");
                byteConversions.Add((byte)i, new BytePair((byte)hex[0], (byte)hex[1]));
            }
        }
    }

    class HashData
    {
        public char firstRepeatOfThree;
        public char[] repeatsOfFive;

        public HashData(byte[] hash)
        {
            bool firstTripleFound = false;
            var fives = new List<char>();
            // For each byte in the hash...
            for (int i = 0; i < hash.Length; i++)
            {
                // If the two hex digits that represent the byte are not the same, skip it.
                if ((hash[i] & 0xf) != (hash[i] >> 4)) continue;
                // If the first three in a row in this hash hasn't been found yet...
                if (!firstTripleFound)
                {
                    // Check the hex digits before and after this byte.
                    if ((i > 0 && ((hash[i - 1] & 0xf) == (hash[i] & 0xf)))
                        || (i < hash.Length - 1 && ((hash[i + 1] >> 4) == (hash[i] & 0xf))))
                    {
                        firstRepeatOfThree = ExtractHexChar(hash[i]);
                        firstTripleFound = true;
                    }
                }
                // If the next byte is the same (i.e. four matching hex digits in a row)...
                if (i < hash.Length - 1 && hash[i] == hash[i + 1])
                {
                    // Check the hex digits before the first byte and after the second byte.
                    if ((i > 0 && ((hash[i - 1] & 0xf) == (hash[i] & 0xf)))
                        || (i < hash.Length - 2 && ((hash[i + 2] >> 4) == (hash[i] & 0xf))))
                    {
                        var five = ExtractHexChar(hash[i]);
                        if (!fives.Contains(five))
                        {
                            fives.Add(five);
                        }
                    }
                }
            }
            repeatsOfFive = fives.ToArray();
        }

        char ExtractHexChar(byte input)
        {
            input &= 0xf;
            if (input < 10) return (char)(input + 48);
            return (char)(input + 87);
        }
    }

    class BytePair
    {
        public byte a;
        public byte b;

        public BytePair(byte one, byte two)
        {
            a = one;
            b = two;
        }
    }
}
