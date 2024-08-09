// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

namespace ProgrammingAdvent2021.Solutions;

internal class Day04 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        IEnumerable<int> numbers = input[0].Split(',').Select(str => int.Parse(str));
        List<BingoBoard> boards = [];
        for (int i = 6; i < input.Length; i += 6)
        {
            boards.Add(new BingoBoard(input[i-4], input[i-3], input[i-2], input[i-1], input[i]));
        }
        BingoBoard? partOneWinner = PartOneWinner(numbers, boards);
        if (partOneWinner is null)
        {
            return ("No winning board found.", "n/a");
        }
        int partOneAnswer = partOneWinner.Score();
        return ($"{partOneAnswer}", "n/a");
    }

    private static BingoBoard? PartOneWinner(IEnumerable<int> numbers, List<BingoBoard> boards)
    {
        foreach (int i in numbers)
        {
            foreach (BingoBoard board in boards)
            {
                if (board.MarkBoard(i))
                {
                    return board;
                }
            }
        }
        return null;
    }

    private class BingoBoard
    {
        // 40 41 42 43 44
        // 30 31 32 33 34
        // 20 21 22 23 24
        // 10 11 12 13 14
        //  0  1  2  3  4
        private ulong _marked;
        private int _previous;
        private readonly int[] _positions;

        public BingoBoard(string line0, string line1, string line2, string line3, string line4)
        {
            _positions = Enumerable.Repeat(-1, 100).ToArray();
            SetPositions(_positions, line0, 40);
            SetPositions(_positions, line1, 30);
            SetPositions(_positions, line2, 20);
            SetPositions(_positions, line3, 10);
            SetPositions(_positions, line4, 0);
        }

        public bool MarkBoard(int number)
        {
            int position = _positions[number];
            if (position >= 0)
            {
                _marked |= 1UL << position;
            }
            _previous = number;
            return HasWon();
        }

        public int Score()
        {
            int sum = 0;
            for (int n = 0; n < 100; n++)
            {
                int position = _positions[n];
                if (position < 0)
                {
                    continue;
                }
                if (((_marked >> position) & 1) == 0)
                {
                    sum += n;
                }
            }
            return sum * _previous;
        }

        private bool HasWon()
        {
            return
                (
                    (_marked & (_marked >> 1) & (_marked >> 2)
                    & (_marked >> 3) & (_marked >> 4)) != 0
                ) || (
                    (_marked & (_marked >> 10) & (_marked >> 20)
                     & (_marked >> 30) & (_marked >> 40)) != 0
                );
        }

        private static void SetPositions(int[] _positions, string line, int start)
        {
            int[] numbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(n => int.Parse(n)).ToArray();
            for (int i = 0; i < numbers.Length; i++)
            {
                _positions[numbers[i]] = start + i;
            }
        }
    }
}
