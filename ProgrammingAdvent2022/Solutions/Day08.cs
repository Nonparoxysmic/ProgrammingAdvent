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

        return result.WriteAnswers(visibleTrees, null);
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
}
