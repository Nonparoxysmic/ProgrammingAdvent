// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System.Collections.Generic;
using System.Diagnostics;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day08 : Day
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
            string[] inputTerms = input.Split();
            Queue<uint> numbers = new Queue<uint>(inputTerms.Length);
            for (uint i = 0; i < inputTerms.Length; i++)
            {
                if (!uint.TryParse(inputTerms[i], out uint result))
                {
                    output.WriteError($"Invalid term \"{inputTerms[i]}\" in input.", sw);
                    return output;
                }
                numbers.Enqueue(result);
            }

            Node rootNode;
            try
            {
                rootNode = new Node(numbers);
            }
            catch
            {
                output.WriteError("Invalid input data.", sw);
                return output;
            }
            if (numbers.Count > 0)
            {
                output.WriteError("Extra data in input.", sw);
                return output;
            }

            uint metadataSum = rootNode.MetadataSum();

            uint rootNodeValue = rootNode.NodeValue();

            sw.Stop();
            output.WriteAnswers(metadataSum, rootNodeValue, sw);
            return output;
        }

        private class Node
        {
            private readonly Node[] childNodes;
            private readonly uint[] metadata;

            public Node(Queue<uint> numbers)
            {
                childNodes = new Node[numbers.Dequeue()];
                metadata = new uint[numbers.Dequeue()];

                for (uint i = 0; i < childNodes.Length; i++)
                {
                    childNodes[i] = new Node(numbers);
                }
                for (uint i = 0; i < metadata.Length; i++)
                {
                    metadata[i] = numbers.Dequeue();
                }
            }

            public uint MetadataSum()
            {
                uint sum = 0;
                for (uint i = 0; i < childNodes.Length; i++)
                {
                    sum += childNodes[i].MetadataSum();
                }
                for (uint i = 0; i < metadata.Length; i++)
                {
                    sum += metadata[i];
                }
                return sum;
            }

            public uint NodeValue()
            {
                if (childNodes.Length == 0)
                {
                    return MetadataSum();
                }

                uint sum = 0;
                for (uint i = 0; i < metadata.Length; i++)
                {
                    if (metadata[i] < 1 || metadata[i] > childNodes.Length)
                    {
                        continue;
                    }
                    sum += childNodes[metadata[i] - 1].NodeValue();
                }
                return sum;
            }
        }
    }
}
