// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

namespace ProgrammingAdvent2021.Solutions;

internal class Day03 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        string partOneAnswer = PartOneAnswer(input);
        string partTwoAnswer = PartTwoAnswer(input);
        return (partOneAnswer, partTwoAnswer);
    }

    private static string PartOneAnswer(string[] input)
    {
        int[] gammaBits = Enumerable.Repeat((int)'0', input[0].Length).ToArray();
        foreach (string line in input)
        {
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '1')
                {
                    gammaBits[i] += 1;
                }
                else
                {
                    gammaBits[i] -= 1;
                }
            }
        }
        char[] epsilonBits = new char[gammaBits.Length];
        for (int i = 0; i < gammaBits.Length; i++)
        {
            if (gammaBits[i] > '1')
            {
                gammaBits[i] = '1';
            }
            if (gammaBits[i] < '0')
            {
                gammaBits[i] = '0';
            }
            if (gammaBits[i] == '1')
            {
                epsilonBits[i] = '0';
            }
            else
            {
                epsilonBits[i] = '1';
            }
        }
        int gamma = Convert.ToInt32(new string(gammaBits.Select(n => (char)n).ToArray()), 2);
        int epsilon = Convert.ToInt32(new string(epsilonBits), 2);
        return $"{gamma * epsilon}";
    }

    private static string PartTwoAnswer(string[] input)
    {
        BitNode rootNode = new(input[0].Length);
        foreach (string line in input)
        {
            rootNode.PopulateNodes(line, line.Length);
        }
        int o2 = Convert.ToInt32(rootNode.O2GeneratorRating(), 2);
        int co2 = Convert.ToInt32(rootNode.CO2ScrubberRating(), 2);
        return $"{o2 * co2}";
    }

    private class BitNode
    {
        private readonly BitNode? _onesNode;
        private readonly BitNode? _zerosNode;
        private int _net;
        private string? _leaf;

        public BitNode(int depth)
        {
            if (depth > 0)
            {
                _onesNode = new BitNode(depth - 1);
                _zerosNode = new BitNode(depth - 1);
            }
        }

        public void PopulateNodes(string bits, int depth)
        {
            if (depth > 0)
            {
                char bit = bits[^depth];
                if (bit == '1')
                {
                    _net++;
                    _onesNode?.PopulateNodes(bits, depth - 1);
                }
                else
                {
                    _net--;
                    _zerosNode?.PopulateNodes(bits, depth - 1);
                }
            }
            else
            {
                _leaf = bits;
            }
        }

        public string O2GeneratorRating()
        {
            if (_leaf is null)
            {
                if (_net >= 0)
                {
                    return _onesNode?.O2GeneratorRating() ?? "Answer not found";
                }
                return _zerosNode?.O2GeneratorRating() ?? "Answer not found";
            }
            return _leaf;
        }

        public string? CO2ScrubberRating()
        {
            if (_leaf is null)
            {
                if (_net >= 0)
                {
                    return _zerosNode?.CO2ScrubberRating()
                        ?? _onesNode?.CO2ScrubberRating();
                }
                return _onesNode?.CO2ScrubberRating()
                    ?? _zerosNode?.CO2ScrubberRating();
            }
            return _leaf;
        }
    }
}
