// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day10 : Day
{
    private static readonly Regex _validCharacters = new("^[.#]+$");

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        int lineLength = inputLines[0].Length;
        foreach (string line in inputLines)
        {
            if (line.Length == 0)
            {
                errorMessage = "Input contains an empty line.";
                return false;
            }
            if (line.Length != lineLength)
            {
                errorMessage = "All lines of input are not the same length.";
                return false;
            }
            if (!_validCharacters.IsMatch(line))
            {
                errorMessage = $"Input \"{line.Left(20, true)}\" contains invalid characters.";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines)
    {
        PuzzleAnswers output = new();
        List<Vector2Int> asteroids = new();
        for (int y = 0; y < inputLines.Length; y++)
        {
            for (int x = 0; x < inputLines[0].Length; x++)
            {
                if (inputLines[y][x] == '#')
                {
                    asteroids.Add(new Vector2Int(x, y));
                }
            }
        }
        int bestDetectableAsteroids = 0;
        Parallel.ForEach(asteroids, position =>
        {
            int detectableAsteroids = DetectableAsteroids(position.X, position.Y, inputLines, asteroids);
            bestDetectableAsteroids = Math.Max(bestDetectableAsteroids, detectableAsteroids);
        });
        return output.WriteAnswers(bestDetectableAsteroids, null);
    }

    private static int DetectableAsteroids(int xPos, int yPos, string[] input, List<Vector2Int> asteroids)
    {
        char[,] map = new char[input[0].Length, input.Length];
        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[0].Length; x++)
            {
                map[x, y] = input[y][x];
            }
        }
        map[xPos, yPos] = 'X';
        foreach (var asteroid in asteroids)
        {
            if (asteroid.X == xPos && asteroid.Y == yPos)
            {
                continue;
            }
            Vector2Int step = StepVector(asteroid.X - xPos, asteroid.Y - yPos);
            Vector2Int position = asteroid + step;
            while (position.X >= 0 && position.Y >= 0
                && position.X < input[0].Length && position.Y < input.Length)
            {
                map[position.X, position.Y] = '*';
                position += step;
            }
        }
        int detectable = 0;
        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[0].Length; x++)
            {
                if (map[x, y] == '#')
                {
                    detectable++;
                }
            }
        }
        return detectable;
    }

    private static Vector2Int StepVector(int x, int y)
    {
        int a = Math.Abs(x), b = Math.Abs(y);
        while (b != 0)
        {
            (a, b) = (b, a % b);
        }
        return new Vector2Int(x / a, y / a);
    }
}
