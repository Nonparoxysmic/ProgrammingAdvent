// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day08 : Day
{
    private static readonly Regex _validCharacters = new("^[012]+$");

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0 || inputLines[0].Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        if (!_validCharacters.IsMatch(inputLines[0]))
        {
            errorMessage = $"Input \"{inputLines[0].Left(20, true)}\" contains invalid characters.";
            return false;
        }
        if (inputLines[0].Length < 25 * 6)
        {
            errorMessage = $"Input \"{inputLines[0].Left(20, true)}\" contains insufficient characters.";
            return false;
        }
        if (inputLines[0].Length % (25 * 6) != 0)
        {
            errorMessage = $"Input \"{inputLines[0].Left(20, true)}\" contains the wrong number of characters.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines)
    {
        PuzzleAnswers output = new();
        char[,,] pixelData = new char[inputLines[0].Length / 150, 25, 6];
        int fewestZeroes = int.MaxValue;
        int fewestZeroesLayer = 0;
        for (int layer = 0; layer < inputLines[0].Length / 150; layer++)
        {
            int zeroCount = 0;
            for (int i = 0; i < 150; i++)
            {
                char c = inputLines[0][i + layer * 150];
                if (c == '0')
                {
                    zeroCount++;
                }
                int x = i % 25;
                int y = i / 25;
                pixelData[layer, x, y] = c;
            }
            if (zeroCount < fewestZeroes)
            {
                fewestZeroes = zeroCount;
                fewestZeroesLayer = layer;
            }
        }
        int oneCount = 0;
        int twoCount = 0;
        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 25; x++)
            {
                char pixel = pixelData[fewestZeroesLayer, x, y];
                if (pixel == '1')
                {
                    oneCount++;
                }
                else if (pixel == '2')
                {
                    twoCount++;
                }
            }
        }
        char[,] mergedLayers = MergeLayers(pixelData);
        char[] password = new char[mergedLayers.GetLength(0) / 5];
        bool foundUnknownLetters = false;
        for (int letter = 0; letter < password.Length; letter++)
        {
            char[] pixels = new char[5 * mergedLayers.GetLength(1)];
            int i = 0;
            for (int y = 0; y < mergedLayers.GetLength(1); y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    pixels[i++] = mergedLayers[x + 5 * letter, y];
                }
            }
            if (!TryGetLetter(string.Join(null, pixels), out char c))
            {
                foundUnknownLetters = true;
            }
            password[letter] = c;
        }
        if (foundUnknownLetters)
        {
            string message = "Unrecognized letter(s) in answer: " + string.Join(null, password);
            return output.WriteAnswers(oneCount * twoCount, message);
        }
        return output.WriteAnswers(oneCount * twoCount, password);
    }

    private static char[,] MergeLayers(char[,,] pixels)
    {
        char[,] output = new char[pixels.GetLength(1), pixels.GetLength(2)];
        for (int y = 0; y < pixels.GetLength(2); y++)
        {
            for (int x = 0; x < pixels.GetLength(1); x++)
            {
                char pixel = '2';
                for (int layer = 0; layer < pixels.GetLength(0); layer++)
                {
                    if (pixels[layer, x, y] == '0' || pixels[layer, x, y] == '1')
                    {
                        pixel = pixels[layer, x, y];
                        break;
                    }
                }
                output[x, y] = pixel;
            }
        }
        return output;
    }

    public static bool TryGetLetter(string pixels, out char letter)
    {
        letter = pixels switch
        {
            "011001001010010111101001010010" => 'A',
            "111001001011100100101001011100" => 'B',
            "011001001010000100001001001100" => 'C',
            "111101000011100100001000011110" => 'E',
            "111101000011100100001000010000" => 'F',
            "011001001010000101101001001110" => 'G',
            "100101001011110100101001010010" => 'H',
            "011100010000100001000010001110" => 'I',
            "001100001000010000101001001100" => 'J',
            "100101010011000101001010010010" => 'K',
            "100001000010000100001000011110" => 'L',
            "011001001010010100101001001100" => 'O',
            "111001001010010111001000010000" => 'P',
            "111001001010010111001010010010" => 'R',
            "011101000010000011000001011100" => 'S',
            "100101001010010100101001001100" => 'U',
            "100011000101010001000010000100" => 'Y',
            "111100001000100010001000011110" => 'Z',
            _ => '?'
        };
        return letter != '?';
    }
}
