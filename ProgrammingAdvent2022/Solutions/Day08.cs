// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal class Day08 : Day
{
    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        int[,] map = InputToMap(input);
        bool[,] visible = VisibleTrees(map);
        int visibleTrees = visible.Cast<bool>().Count(b => b);
        int[,] scenicScores = ScenicScores(map);
        int bestScenicScore = scenicScores.Cast<int>().Max();

        return result.WriteAnswers(visibleTrees, bestScenicScore);
    }

    private static int[,] InputToMap(string[] input)
    {
        int width = input[0].Length;
        int height = input.Length;
        int[,] output = new int[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                output[x, y] = input[y][x] - '0';
            }
        }
        return output;
    }

    private static bool[,] VisibleTrees(int[,] map)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        bool[,] visible = new bool[width, height];
        for (int y = 0; y < height; y++)
        {
            int lastLevel = -1;
            for (int x = 0; x < width && lastLevel < 9; x++)
            {
                if (map[x, y] > lastLevel)
                {
                    visible[x, y] = true;
                    lastLevel = map[x, y];
                }
            }
            lastLevel = -1;
            for (int x = width - 1; x >= 0 && lastLevel < 9; x--)
            {
                if (map[x, y] > lastLevel)
                {
                    visible[x, y] = true;
                    lastLevel = map[x, y];
                }
            }
        }
        for (int x = 0; x < width; x++)
        {
            int lastLevel = -1;
            for (int y = 0; y < height && lastLevel < 9; y++)
            {
                if (map[x, y] > lastLevel)
                {
                    visible[x, y] = true;
                    lastLevel = map[x, y];
                }
            }
            lastLevel = -1;
            for (int y = height - 1; y >= 0 && lastLevel < 9; y--)
            {
                if (map[x, y] > lastLevel)
                {
                    visible[x, y] = true;
                    lastLevel = map[x, y];
                }
            }
        }
        return visible;
    }

    private static int[,] ScenicScores(int[,] map)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        int[,] scores = new int[width, height];
        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                scores[x, y] = ScenicScore(x, y, map, width, height);
            }
        }
        return scores;
    }

    private static int ScenicScore(int xPos, int yPos, int[,] map, int width, int height)
    {
        int score = 1;
        int start = map[xPos, yPos];
        int steps = 0;
        for (int x = xPos - 1; x >= 0; x--)
        {
            if (map[x, yPos] < start)
            {
                steps++;
            }
            else
            {
                steps++;
                break;
            }
        }
        score *= steps;
        steps = 0;
        for (int x = xPos + 1; x < width; x++)
        {
            if (map[x, yPos] < start)
            {
                steps++;
            }
            else
            {
                steps++;
                break;
            }
        }
        score *= steps;
        steps = 0;
        for (int y = yPos - 1; y >= 0; y--)
        {
            if (map[xPos, y] < start)
            {
                steps++;
            }
            else
            {
                steps++;
                break;
            }
        }
        score *= steps;
        steps = 0;
        for (int y = yPos + 1; y < height; y++)
        {
            if (map[xPos, y] < start)
            {
                steps++;
            }
            else
            {
                steps++;
                break;
            }
        }
        score *= steps;
        return score;
    }
}
