// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day25 : Day
    {
        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (input.Trim() == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }
            string[] inputLines = input.ToLines();
            if (inputLines.Length < 12)
            {
                output.WriteError("Input is too short.", sw);
                return output;
            }

            Match firstLine = Regex.Match(inputLines[0], @"^Begin in state ([A-Z])\.$");
            if (!firstLine.Success)
            {
                output.WriteError($"Invalid line in input: \"{inputLines[0]}\"", sw);
                return output;
            }
            Match secondLine = Regex.Match(inputLines[1],
                @"^Perform a diagnostic checksum after (\d+) steps.$");
            if (!secondLine.Success)
            {
                output.WriteError($"Invalid line in input: \"{inputLines[1]}\"", sw);
                return output;
            }
            char initialState = firstLine.Groups[1].Value[0];
            int stepsToDiagnostic = int.Parse(secondLine.Groups[1].Value);

            Dictionary<char, State> states = new Dictionary<char, State>();
            for (int i = 3; i + 8 < inputLines.Length; i += 10)
            {
                Match stateMatch = Regex.Match(inputLines[i], @"^In state ([A-Z]):$");
                if (!stateMatch.Success)
                {
                    output.WriteError($"Invalid line in input: \"{inputLines[i]}\"", sw);
                    return output;
                }
                char state = stateMatch.Groups[1].Value[0];
                int[] writeValues = new int[2];
                Direction[] moveDirections = new Direction[2];
                char[] nextStates = new char[2];
                for (int c = 0; c <= 1; c++)
                {
                    int index = i + 2 + 4 * c;
                    Match writeMatch = Regex.Match(inputLines[index],
                        @"^    - Write the value ([01])\.$");
                    if (!writeMatch.Success)
                    {
                        output.WriteError($"Invalid line in input: \"{inputLines[index]}\"", sw);
                        return output;
                    }
                    index++;
                    Match moveMatch = Regex.Match(inputLines[index],
                        @"^    - Move one slot to the (left|right)\.$");
                    if (!moveMatch.Success)
                    {
                        output.WriteError($"Invalid line in input: \"{inputLines[index]}\"", sw);
                        return output;
                    }
                    index++;
                    Match continueMatch = Regex.Match(inputLines[index],
                        @"^    - Continue with state ([A-Z])\.$");
                    if (!continueMatch.Success)
                    {
                        output.WriteError($"Invalid line in input: \"{inputLines[index]}\"", sw);
                        return output;
                    }
                    writeValues[c] = int.Parse(writeMatch.Groups[1].Value);
                    moveDirections[c] = moveMatch.Groups[1].Value == "left"
                        ? Direction.left : Direction.right;
                    nextStates[c] = continueMatch.Groups[1].Value[0];
                }
                states.Add(state, new State(state, writeValues, moveDirections, nextStates));
            }
            if (!states.ContainsKey(initialState))
            {
                output.WriteError("List of states does not contain the initial state.", sw);
                return output;
            }
            foreach (KeyValuePair<char, State> kvp in states)
            {
                if (!states.ContainsKey(kvp.Value.ContinueStates[0])
                    || !states.ContainsKey(kvp.Value.ContinueStates[1]))
                {
                    output.WriteError($"Unknown state reference in state {kvp.Value.StateID}.", sw);
                    return output;
                }
            }
            
            TuringMachine turingMachine = new TuringMachine(states, initialState);
            while (stepsToDiagnostic > 0)
            {
                turingMachine.Tick();
                stepsToDiagnostic--;
            }
            int partOneAnswer = turingMachine.DiagnosticChecksum();

            sw.Stop();
            output.WriteAnswers(partOneAnswer, "Reboot the Printer", sw);
            return output;
        }

        private enum Direction
        {
            left,
            right
        }

        private class State
        {
            internal char StateID { get; }
            internal int[] WriteValues { get; }
            internal Direction[] MoveDirections { get; }
            internal char[] ContinueStates { get; }

            public State(char state, int[] write, Direction[] move, char[] next)
            {
                StateID = state;
                WriteValues = write;
                MoveDirections = move;
                ContinueStates = next;
            }

            public override string ToString()
            {
                string NL = Environment.NewLine;
                StringBuilder output = new StringBuilder();
                output.Append($"In state {StateID}:" + NL);
                output.Append($"  If the current value is 0:" + NL);
                output.Append($"    - Write the value {WriteValues[0]}." + NL);
                output.Append($"    - Move one slot to the {MoveDirections[0]}." + NL);
                output.Append($"    - Continue with state {ContinueStates[0]}." + NL);
                output.Append($"  If the current value is 1:" + NL);
                output.Append($"    - Write the value {WriteValues[1]}." + NL);
                output.Append($"    - Move one slot to the {MoveDirections[1]}." + NL);
                output.Append($"    - Continue with state {ContinueStates[1]}.");
                return output.ToString();
            }
        }

        private class TuringMachine
        {
            internal int CurrentPosition { get; set; }
            internal char CurrentStateID { get; set; }

            private readonly Dictionary<char, State> states;
            private readonly HashSet<int> tapeOnes = new HashSet<int>();

            public TuringMachine(Dictionary<char, State> states, char initialState)
            {
                this.states = states;
                CurrentStateID = initialState;
            }

            internal void Tick()
            {
                State currentState = states[CurrentStateID];
                int currentValue = tapeOnes.Contains(CurrentPosition) ? 1 : 0;
                if (currentState.WriteValues[currentValue] == 0)
                {
                    tapeOnes.Remove(CurrentPosition);
                }
                else tapeOnes.Add(CurrentPosition);
                if (currentState.MoveDirections[currentValue] == Direction.left)
                {
                    CurrentPosition--;
                }
                else CurrentPosition++;
                CurrentStateID = currentState.ContinueStates[currentValue];
            }

            internal int DiagnosticChecksum()
            {
                return tapeOnes.Count;
            }
        }
    }
}
