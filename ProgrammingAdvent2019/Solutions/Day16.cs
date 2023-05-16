// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day16 : Day
{
    private static readonly Regex _validCharacters = new("^[0-9]+$");

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0 || inputLines[0].Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        if (inputLines[0].Length < 8)
        {
            errorMessage = "Input is too short.";
            return false;
        }
        if (!_validCharacters.IsMatch(inputLines[0]))
        {
            errorMessage = $"Input \"{inputLines[0].Left(20, true)}\" contains invalid characters.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();
        byte[] input = new byte[inputLines[0].Length];
        for (int i = 0; i < input.Length; i++)
        {
            input[i] = (byte)(inputLines[0][i] - '0');
        }
        byte[] calculated = BruteForceFFT(input);
        for (int i = 1; i < 100; i++)
        {
            calculated = BruteForceFFT(calculated);
        }
        string partOneAnswer = string.Join(null, calculated)[..8];
        return output.WriteAnswers(partOneAnswer, null);
    }

    private static byte[] BruteForceFFT(byte[] input)
    {
        byte[] output = new byte[input.Length];
        PatternGenerator patternGenerator = new();
        for (int o = 0; o < input.Length; o++)
        {
            int sum = 0;
            patternGenerator.Reset(o);
            for (int i = 0; i < input.Length; i++)
            {
                sum += input[i] * patternGenerator.Next();
            }
            output[o] = (byte)(Math.Abs(sum) % 10);
        }
        return output;
    }

    private class PatternGenerator
    {
        private readonly int[] _values = new int[] { 0, 1, 0, -1 };
        private int _position;
        private int _currentRepeat;
        private int _repeats;

        public PatternGenerator(int repeats = 0)
        {
            Reset(repeats);
        }

        public void Reset(int repeats)
        {
            _position = 0;
            _currentRepeat = 0;
            _repeats = repeats;
            Next();
        }

        public int Next()
        {
            int output = _values[_position];
            if (++_currentRepeat > _repeats)
            {
                _currentRepeat = 0;
                _position = ++_position % 4;
            }
            return output;
        }
    }
}
