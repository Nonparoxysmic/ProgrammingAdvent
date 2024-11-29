// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal class Day20 : Day
{
    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        // Part One
        ParseInput(input);
        Number.MixNumbers();
        long sumOfCoordinates = Number.SumOfCoordinates();

        // Part Two
        ParseInput(input, 811_589_153);
        for (int i = 0; i < 10; i++)
        {
            Number.MixNumbers();
        }
        long actualSumOfCoordinates = Number.SumOfCoordinates();

        return result.WriteAnswers(sumOfCoordinates, actualSumOfCoordinates);
    }

    private static void ParseInput(string[] input, long decryptionKey = 1)
    {
        Number[] numbers = input.Select(line => new Number(long.Parse(line) * decryptionKey)).ToArray();
        for (int i = 1; i < input.Length - 1; i++)
        {
            numbers[i].Previous = numbers[i - 1];
            numbers[i].Next = numbers[i + 1];
            numbers[i].NextMove = numbers[i + 1];
        }
        numbers[0].Previous = numbers[^1];
        numbers[0].Next = numbers[1];
        numbers[0].NextMove = numbers[1];
        numbers[^1].Previous = numbers[^2];
        numbers[^1].Next = numbers[0];
        numbers[^1].NextMove = null;
        Number.Count = numbers.Length;
        Number.Start = numbers[0];
        Number.Zero = numbers.Where(n => n.Value == 0).First();
    }

    private class Number
    {
        public static int Count { get; set; }
        public static Number Start { get; set; } = new(long.MinValue);
        public static Number Zero { get; set; } = new(long.MinValue);

        public long Value;
        public Number Previous;
        public Number Next;

        public Number? NextMove;

        public Number(long value)
        {
            Value = value;
            Next = this;
            Previous = this;
        }

        public static void MixNumbers()
        {
            Number currentMove = Start;
            while (true)
            {
                if (currentMove.Value != 0)
                {
                    // Remove current from loop
                    currentMove.Previous.Next = currentMove.Next;
                    currentMove.Next.Previous = currentMove.Previous;
                    Count--;
                    // Find new position for current
                    Number newNext = Advance(currentMove.Value, currentMove.Next);
                    Number newPrevious = newNext.Previous;
                    // Insert current into loop
                    currentMove.Next = newNext;
                    currentMove.Previous = newPrevious;
                    currentMove.Previous.Next = currentMove;
                    currentMove.Next.Previous = currentMove;
                    Count++;
                }
                if (currentMove.NextMove is null)
                {
                    break;
                }
                currentMove = currentMove.NextMove;
            }
        }

        public static long SumOfCoordinates()
        {
            long sum = 0;
            Number current = Advance(1000, Zero);
            sum += current.Value;
            current = Advance(1000, current);
            sum += current.Value;
            current = Advance(1000, current);
            sum += current.Value;
            return sum;
        }

        private static Number Advance(long steps, Number position)
        {
            steps = (steps + Count / 2) % Count - Count / 2;
            Number current = position;
            if (steps < 0)
            {
                for (int i = 0; i < -steps; i++)
                {
                    current = current.Previous;
                }
                return current;
            }
            for (int i = 0; i < steps; i++)
            {
                current = current.Next;
            }
            return current;
        }
    }
}
