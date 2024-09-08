// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using System.Text;
using System.Text.RegularExpressions;
using ProgrammingAdvent2021.Common;

namespace ProgrammingAdvent2021.Solutions;

internal class Day13 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        int i = 0;
        List<(int, int)> dots = [];
        Queue<(char, int)> folds = [];
        int xMax = 0, yMax = 0;
        for (; i < input.Length; i++)
        {
            Match match = GeneratedRegex.ValidDay13Coordinates.Match(input[i]);
            if (!match.Success)
            {
                break;
            }
            int x = int.Parse(match.Groups["x"].Value);
            int y = int.Parse(match.Groups["y"].Value);
            dots.Add((x, y));
            xMax = Math.Max(xMax, x);
            yMax = Math.Max(yMax, y);
        }
        for (; i < input.Length; i++)
        {
            Match match = GeneratedRegex.ValidDay13Fold.Match(input[i]);
            if (match.Success)
            {
                string axis = match.Groups["axis"].Value;
                int coordinate = int.Parse(match.Groups["coordinate"].Value);
                if (coordinate == 0)
                {
                    continue;
                }
                folds.Enqueue((axis[0], coordinate));
                if (axis[0] == 'x')
                {
                    xMax = Math.Max(xMax, coordinate * 2);
                }
                else
                {
                    yMax = Math.Max(yMax, coordinate * 2);
                }
            }
        }

        Paper paper = new(dots, folds, xMax + 1, yMax + 1);
        paper.NextFold();
        int partOneAnswer = paper.DotCount();

        while (paper.NextFold()) { }
        if (!paper.TryRead(out string partTwoAnswer))
        {
            return ($"{partOneAnswer}", "Failed to solve");
        }

        return ($"{partOneAnswer}", $"{partTwoAnswer}");
    }

    public static bool TryGetLetter(string pixels, out char letter)
    {
        letter = pixels switch
        {
            ".##..#..#.#..#.####.#..#.#..#." => 'A',
            "###..#..#.###..#..#.#..#.###.." => 'B',
            ".##..#..#.#....#....#..#..##.." => 'C',
            "####.#....###..#....#....####." => 'E',
            "####.#....###..#....#....#...." => 'F',
            ".##..#..#.#....#.##.#..#..###." => 'G',
            "#..#.#..#.####.#..#.#..#.#..#." => 'H',
            ".###...#....#....#....#...###." => 'I',
            "..##....#....#....#.#..#..##.." => 'J',
            "#..#.#.#..##...#.#..#.#..#..#." => 'K',
            "#....#....#....#....#....####." => 'L',
            ".##..#..#.#..#.#..#.#..#..##.." => 'O',
            "###..#..#.#..#.###..#....#...." => 'P',
            "###..#..#.#..#.###..#.#..#..#." => 'R',
            ".###.#....#.....##.....#.###.." => 'S',
            "#..#.#..#.#..#.#..#.#..#..##.." => 'U',
            "#...##...#.#.#...#....#....#.." => 'Y',
            "####....#...#...#...#....####." => 'Z',
            _ => '?'
        };
        return letter != '?';
    }

    private class Paper
    {
        private readonly bool[,] _dots;
        private int _width;
        private int _height;
        private readonly Queue<(char, int)> _folds;

        public Paper(List<(int, int)> dots, Queue<(char, int)> folds, int width, int height)
        {
            _dots = new bool[width, height];
            foreach ((int x, int y) in dots)
            {
                _dots[x, y] = true;
            }
            _folds = folds;
            _width = width;
            _height = height;
        }

        public bool NextFold()
        {
            if (_folds.Count == 0)
            {
                return false;
            }
            (char axis, int coordinate) = _folds.Dequeue();
            if (axis == 'x')
            {
                FoldX(coordinate);
            }
            else
            {
                FoldY(coordinate);
            }
            return true;
        }

        public int DotCount()
        {
            int sum = 0;
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    sum += _dots[x, y] ? 1 : 0;
                }
            }
            return sum;
        }

        public void Print()
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    char c = _dots[x, y] ? '#' : '.';
                    Console.Write(c);
                }
                Console.WriteLine();
            }
        }

        public bool TryRead(out string result)
        {
            result = string.Empty;
            if (_height != 6 || _width != 40)
            {
                return false;
            }
            char[] letters = new char[8];
            for (int i = 0; i < 8; i++)
            {
                StringBuilder sb = new();
                for (int y = 0; y < _height; y++)
                {
                    for (int x = i * 5; x < i * 5 + 5; x++)
                    {
                        sb.Append(_dots[x, y] ? '#' : '.');
                    }
                }
                if (!TryGetLetter(sb.ToString(), out char letter))
                {
                    return false;
                }
                letters[i] = letter;
            }
            result = string.Join(null, letters);
            return true;
        }

        private void FoldX(int coordinate)
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < coordinate; x++)
                {
                    _dots[x, y] |= _dots[2 * coordinate - x, y];
                }
            }
            _width = coordinate;
        }

        private void FoldY(int coordinate)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < coordinate; y++)
                {
                    _dots[x, y] |= _dots[x, 2 * coordinate - y];
                }
            }
            _height = coordinate;
        }
    }
}
