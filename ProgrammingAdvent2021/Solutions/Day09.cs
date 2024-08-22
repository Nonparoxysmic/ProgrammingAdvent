// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

namespace ProgrammingAdvent2021.Solutions;

internal class Day09 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        int[,] map = InputToMap(input);
        int riskSum = 0;
        for (int y = 1; y < map.GetLength(1) - 1; y++)
        {
            for (int x = 1; x < map.GetLength(0) - 1; x++)
            {
                if (map[x, y] < map[x + 1, y] && map[x, y] < map[x, y + 1] &&
                    map[x, y] < map[x - 1, y] && map[x, y] < map[x, y - 1])
                {
                    riskSum += map[x, y] + 1;
                }
            }
        }
        return ($"{riskSum}", "n/a");
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
                    output[x, y] = 10;
                }
                else
                {
                    output[x, y] = input[y - 1][x - 1] - '0';
                }
            }
        }
        return output;
    }
}
