// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Collections.Concurrent;
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
        int asteroidCount = 0;
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
            foreach (char c in line)
            {
                if (c == '#')
                {
                    asteroidCount++;
                }
            }
        }
        if (asteroidCount == 0)
        {
            errorMessage = "No asteroids in the map.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
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
        if (asteroids.Count == 1 || asteroids.Count == 2)
        {
            return output.WriteAnswers(asteroids.Count - 1, "There are not 200 other asteroids.");
        }
        int bestDetectableAsteroids = 0;
        Vector2Int bestPosition = Vector2Int.Zero;
        Parallel.ForEach(asteroids, position =>
        {
            int detectableAsteroids = DetectableAsteroids(position.X, position.Y, inputLines, asteroids);
            if (detectableAsteroids > bestDetectableAsteroids)
            {
                bestDetectableAsteroids = detectableAsteroids;
                bestPosition = position;
            }
        });
        if (asteroids.Count < 201)
        {
            return output.WriteAnswers(bestDetectableAsteroids, "There are not 200 other asteroids.");
        }
        ConcurrentDictionary<int, (double, int)> asteroidData = new();
        Parallel.ForEach(asteroids, position =>
        {
            if (position == bestPosition)
            {
                return;
            }
            int key = 100 * position.X + position.Y;
            asteroidData.TryAdd(key, CalculateAsteroidData(position, bestPosition, inputLines));
        });
        List<KeyValuePair<int, (double, int)>> asteroidDataList = asteroidData.ToList();
        asteroidDataList.Sort(CompareVaporizationOrder);
        return output.WriteAnswers(bestDetectableAsteroids, asteroidDataList[199].Key);
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
            Vector2Int step = StepVector(asteroid.X - xPos, asteroid.Y - yPos, out int _);
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

    private static Vector2Int StepVector(int x, int y, out int factor)
    {
        factor = MathS.GCD(Math.Abs(x), Math.Abs(y));
        return new Vector2Int(x / factor, y / factor);
    }

    private static (double, int) CalculateAsteroidData(Vector2Int position, Vector2Int bestPosition, string[] inputLines)
    {
        double angle = -Math.Atan2(position.X - bestPosition.X, position.Y - bestPosition.Y);
        int depth = 0;
        Vector2Int pos = position;
        Vector2Int step = StepVector(bestPosition.X - position.X, bestPosition.Y - position.Y, out int factor);
        for (int i = 0; i < factor - 1; i++)
        {
            pos += step;
            if (inputLines[pos.Y][pos.X] == '#')
            {
                depth++;
            }
        }
        return (angle, depth);
    }

    private int CompareVaporizationOrder(KeyValuePair<int, (double, int)> A, KeyValuePair<int, (double, int)> B)
    {
        if (A.Value.Item2 < B.Value.Item2)
        {
            return -1;
        }
        if (A.Value.Item2 > B.Value.Item2)
        {
            return 1;
        }
        if (A.Value.Item1 < B.Value.Item1)
        {
            return -1;
        }
        if (A.Value.Item1 > B.Value.Item1)
        {
            return 1;
        }
        return 0;
    }
}
