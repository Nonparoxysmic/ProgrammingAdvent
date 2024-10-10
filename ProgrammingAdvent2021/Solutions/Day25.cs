// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

namespace ProgrammingAdvent2021.Solutions;

internal class Day25 : Day
{
    private static bool[,] _canMove = new bool[0, 0];  

    protected override (string, string) CalculateAnswers(string[] input)
    {
        int width = input[0].Length;
        int height = input.Length;
        char[,] map = new char[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, y] = input[y][x];
            }
        }
        _canMove = new bool[width, height];

        int partOneAnswer = 1;
        while (Update(map, width, height))
        {
            partOneAnswer++;
        }

        return ($"{partOneAnswer}", "n/a");
    }

    private static bool Update(char[,] map, int width, int height)
    {
        bool movement = false;
        // East-facing
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                _canMove[x, y] = map[x, y] == '>' && map[(x + 1) % width, y] == '.';
            }
        }
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (_canMove[x, y])
                {
                    map[x, y] = '.';
                    map[(x + 1) % width, y] = '>';
                    _canMove[x, y] = false;
                    movement = true;
                }
            }
        }
        // South-facing
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                _canMove[x, y] = map[x, y] == 'v' && map[x, (y + 1) % height] == '.';
            }
        }
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (_canMove[x, y])
                {
                    map[x, y] = '.';
                    map[x, (y + 1) % height] = 'v';
                    _canMove[x, y] = false;
                    movement = true;
                }
            }
        }
        return movement;
    }
}
