// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

namespace ProgrammingAdvent2021.Solutions;

internal class Day11 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        int[,] map = InputToMap(input);
        int totalFlashes = 0;
        for (int i = 0; i < 100; i++)
        {
            Step(map, out int flashes);
            totalFlashes += flashes;
        }
        int allFlashStep = -1;
        for (int i = 100; i < 4096; i++)
        {
            Step(map, out int flashes);
            if (flashes == 100)
            {
                allFlashStep = i + 1;
                break;
            }
        }
        if (allFlashStep < 0)
        {
            return ($"{totalFlashes}", "Error: didn't find answer.");
        }
        return ($"{totalFlashes}", $"{allFlashStep}");
    }
    private static int[,] InputToMap(string[] input)
    {
        int width = input[0].Length;
        int height = input.Length;
        int[,] output = new int[width + 2, height + 2];
        for (int y = 0; y < height + 2; y++)
        {
            for (int x = 0; x < width + 2; x++)
            {
                if (x < 1 || y < 1 || x > width || y > height)
                {
                    output[x, y] = int.MinValue;
                }
                else
                {
                    output[x, y] = input[y - 1][x - 1] - '0';
                }
            }
        }
        return output;
    }

    private static void Step(int[,] map, out int flashCount)
    {
        // The energy level of each octopus increases by 1.
        for (int y = 1; y < map.GetLength(1) - 1; y++)
        {
            for (int x = 1; x < map.GetLength(0) - 1; x++)
            {
                map[x, y]++;
            }
        }
        // Any octopus with an energy level greater than 9 flashes.
        flashCount = 0;
        for (int y = 1; y < map.GetLength(1) - 1; y++)
        {
            for (int x = 1; x < map.GetLength(0) - 1; x++)
            {
                if (map[x, y] > 9)
                {
                    map[x - 1, y - 1]++;
                    map[x - 1, y]++;
                    map[x - 1, y + 1]++;
                    map[x, y - 1]++;
                    map[x, y] = int.MinValue;
                    map[x, y + 1]++;
                    map[x + 1, y - 1]++;
                    map[x + 1, y]++;
                    map[x + 1, y + 1]++;
                    x = x > 1 ? x - 2 : 0;
                    y = y > 1 ? y - 2 : 0;
                    flashCount++;
                }
            }
        }
        // Any octopus that flashed during this step has its energy level set to 0.
        for (int y = 1; y < map.GetLength(1) - 1; y++)
        {
            for (int x = 1; x < map.GetLength(0) - 1; x++)
            {
                if (map[x, y] < 0)
                {
                    map[x, y] = 0;
                }
            }
        }
    }
}
