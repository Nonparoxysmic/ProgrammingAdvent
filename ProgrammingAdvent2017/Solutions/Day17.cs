// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System.Diagnostics;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day17 : Day
    {
        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            input = input.Trim();
            if (input == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }
            if (!int.TryParse(input, out int steps) || steps < 1)
            {
                output.WriteError("Input must be a positive integer.", sw);
                return output;
            }

            BufferElement currentElement = new BufferElement(0);
            for (int i = 1; i <= 2017; i++)
            {
                // Stepping forward.
                currentElement = currentElement.TakeSteps(steps);

                // Inserting a new value.
                BufferElement newElement = new BufferElement(i, currentElement.NextElement);
                currentElement.NextElement = newElement;

                // Using the location of the inserted value as the new current position.
                currentElement = newElement;
            }

            sw.Stop();
            output.WriteAnswers(currentElement.NextElement.Value, null, sw);
            return output;
        }

        private class BufferElement
        {
            internal BufferElement NextElement { get; set; }
            internal int Value { get; set; }

            public BufferElement(int value)
            {
                Value = value;
                NextElement = this;
            }

            public BufferElement(int value, BufferElement nextElement)
            {
                Value = value;
                NextElement = nextElement;
            }

            internal BufferElement TakeSteps(int steps)
            {
                if (steps == 1) { return NextElement; }
                else
                {
                    return NextElement.TakeSteps(steps - 1);
                }
            }
        }
    }
}
