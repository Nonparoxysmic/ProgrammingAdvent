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
    private static readonly PatternGenerator _patternGenerator = new();

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
        int[] input = new int[inputLines[0].Length];
        for (int i = 0; i < input.Length; i++)
        {
            input[i] = inputLines[0][i] - '0';
        }
        int[] calculated = FFT(input);
        for (int i = 1; i < 100; i++)
        {
            calculated = FFT(calculated);
        }
        string partOneAnswer = string.Join(null, calculated)[..8];
        return output.WriteAnswers(partOneAnswer, null);
    }

    private static int[] FFT(int[] input)
    {
        int[] sums = new int[input.Length];
        sums[^1] = input[^1];
        for (int i = input.Length - 2; i >= 0; i--)
        {
            sums[i] = input[i] + sums[i + 1];
        }

        int[] output = new int[input.Length];
        for (int i = 0; i < input.Length / 3; i++)
        {
            int sum = 0;
            _patternGenerator.Reset(i);
            for (int j = 0; j < input.Length; j++)
            {
                sum += input[j] * _patternGenerator.Next();
            }
            output[i] = Math.Abs(sum) % 10;
        }
        for (int i = input.Length / 3; i < input.Length / 2; i++)
        {
            output[i] = (sums[i] - sums[i * 2 + 1]) % 10;
        }
        for (int i = input.Length / 2; i < input.Length; i++)
        {
            output[i] = sums[i] % 10;
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
