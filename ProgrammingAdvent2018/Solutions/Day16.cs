// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day16 : Day
    {
        private readonly Regex instructionLine = new Regex(@"^([0-9]|1[0-5]) ([0-9]+) ([0-9]+) ([0-3])$");
        private readonly Regex bforeLine = new Regex(@"^Before: \[([0-9]+), ([0-9]+), ([0-9]+), ([0-9]+)]$");
        private readonly Regex afterLine = new Regex(@"^After:  \[([0-9]+), ([0-9]+), ([0-9]+), ([0-9]+)]$");

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
            string[] inputLines = input.ToLines();
            if (inputLines.Length < 60)
            {
                output.WriteError("Input too short.", sw);
                return output;
            }

            var possibleOpcodes = new Dictionary<int, HashSet<Opcode>>();
            for (int i = 0; i < 16; i++)
            {
                possibleOpcodes.Add(i, new HashSet<Opcode>());
                foreach (Opcode opcode in Enum.GetValues(typeof(Opcode)))
                {
                    possibleOpcodes[i].Add(opcode);
                }
            }

            int partOneAnswer = 0;
            int lastAfter = -1;
            for (int i = 0; i <= inputLines.Length - 4; i += 4)
            {
                Match before = bforeLine.Match(inputLines[i]);
                Match instruction = instructionLine.Match(inputLines[i + 1]);
                Match after = afterLine.Match(inputLines[i + 2]);
                if (!before.Success || !instruction.Success || !after.Success)
                {
                    break;
                }
                if (!TryParseRegisters(before, out ulong[] registersBefore))
                {
                    output.WriteError($"Cannot parse register values \"{inputLines[i]}\".", sw);
                    return output;
                }
                if (!int.TryParse(instruction.Groups[1].Value, out int opcodeNumber)
                    || !ulong.TryParse(instruction.Groups[2].Value, out ulong A)
                    || !ulong.TryParse(instruction.Groups[3].Value, out ulong B)
                    || !int.TryParse(instruction.Groups[4].Value, out int C))
                {
                    output.WriteError($"Cannot parse instruction \"{inputLines[i + 1]}\".", sw);
                    return output;
                }
                if (!TryParseRegisters(after, out ulong[] registersAfter))
                {
                    output.WriteError($"Cannot parse register values \"{inputLines[i + 2]}\".", sw);
                    return output;
                }
                List<Opcode> impossibleOpcodes = ImpossibleOpcodes(registersBefore, registersAfter, A, B, C);
                if (impossibleOpcodes.Count <= 13)
                {
                    partOneAnswer++;
                }
                foreach (Opcode impossibleOpcode in impossibleOpcodes)
                {
                    possibleOpcodes[opcodeNumber].Remove(impossibleOpcode);
                }
                lastAfter = i + 2;
            }
            if (lastAfter < 59)
            {
                output.WriteError("Insufficient number of samples.", sw);
                return output;
            }

            if (!TrySolveOpcodes(possibleOpcodes, out Opcode[] opcodes))
            {
                output.WriteError("Unable to solve opcode values.", sw);
                return output;
            }

            sw.Stop();
            output.WriteAnswers(partOneAnswer, null, sw);
            return output;
        }

        private bool TryParseRegisters(Match match, out ulong[] registerValues)
        {
            if (match.Groups.Count < 5)
            {
                registerValues = Array.Empty<ulong>();
                return false;
            }
            if (!ulong.TryParse(match.Groups[1].Value, out ulong first) ||
                !ulong.TryParse(match.Groups[2].Value, out ulong second) ||
                !ulong.TryParse(match.Groups[3].Value, out ulong third) ||
                !ulong.TryParse(match.Groups[4].Value, out ulong fourth))
            {
                registerValues = Array.Empty<ulong>();
                return false;
            }
            registerValues = new ulong[] { first, second, third, fourth };
            return true;
        }

        private List<Opcode> ImpossibleOpcodes(ulong[] before, ulong[] after, ulong A, ulong B, int C)
        {
            List<Opcode> output = new List<Opcode>();
            if (A < 4 && B < 4)
            {
                if (after[C] != before[A] + before[B])
                {
                    output.Add(Opcode.addr);
                }
                if (after[C] != before[A] * before[B])
                {
                    output.Add(Opcode.mulr);
                }
                if (after[C] != (before[A] & before[B]))
                {
                    output.Add(Opcode.banr);
                }
                if (after[C] != (before[A] | before[B]))
                {
                    output.Add(Opcode.borr);
                }
                if (after[C] != (before[A] > before[B] ? 1UL : 0))
                {
                    output.Add(Opcode.gtrr);
                }
                if (after[C] != (before[A] == before[B] ? 1UL : 0))
                {
                    output.Add(Opcode.eqrr);
                }
            }
            else
            {
                output.Add(Opcode.addr);
                output.Add(Opcode.mulr);
                output.Add(Opcode.banr);
                output.Add(Opcode.borr);
                output.Add(Opcode.gtrr);
                output.Add(Opcode.eqrr);
            }

            if (A < 4)
            {
                if (after[C] != before[A])
                {
                    output.Add(Opcode.setr);
                }
                if (after[C] != before[A] + B)
                {
                    output.Add(Opcode.addi);
                }
                if (after[C] != before[A] * B)
                {
                    output.Add(Opcode.muli);
                }
                if (after[C] != (before[A] & B))
                {
                    output.Add(Opcode.bani);
                }
                if (after[C] != (before[A] | B))
                {
                    output.Add(Opcode.bori);
                }
                if (after[C] != (before[A] > B ? 1UL : 0))
                {
                    output.Add(Opcode.gtri);
                }
                if (after[C] != (before[A] == B ? 1UL : 0))
                {
                    output.Add(Opcode.eqri);
                }
            }
            else
            {
                output.Add(Opcode.setr);
                output.Add(Opcode.addi);
                output.Add(Opcode.muli);
                output.Add(Opcode.bani);
                output.Add(Opcode.bori);
                output.Add(Opcode.gtri);
                output.Add(Opcode.eqri);
            }

            if (B < 4)
            {
                if (after[C] != (A > before[B] ? 1UL : 0))
                {
                    output.Add(Opcode.gtir);
                }
                if (after[C] != (A == before[B] ? 1UL : 0))
                {
                    output.Add(Opcode.eqir);
                }
            }
            else
            {
                output.Add(Opcode.gtir);
                output.Add(Opcode.eqir);
            }

            if (after[C] != A)
            {
                output.Add(Opcode.seti);
            }
            return output;
        }

        private bool TrySolveOpcodes(Dictionary<int, HashSet<Opcode>> possibleOpcodes, out Opcode[] opcodes)
        {
            SolveState[,] solution = new SolveState[16, 16];
            bool[] solvedNumbers = new bool[16];
            bool[] solvedOpcodes = new bool[16];
            foreach (var kvp in possibleOpcodes)
            {
                int opcodeValue = kvp.Key;
                foreach (Opcode possibleOpcode in kvp.Value)
                {
                    solution[opcodeValue, (int)possibleOpcode] = SolveState.Maybe;
                }
            }
            bool changesMade = true;
            while (changesMade)
            {
                changesMade = false;
                for (int x = 0; x < 16; x++)
                {
                    if (solvedNumbers[x])
                    {
                        continue;
                    }
                    int maybes = 0;
                    int lastMaybeY = -1;
                    for (int y = 0; y < 16; y++)
                    {
                        if (solution[x, y] == SolveState.Maybe)
                        {
                            maybes++;
                            lastMaybeY = y;
                        }
                    }
                    if (maybes == 0)
                    {
                        opcodes = Array.Empty<Opcode>();
                        return false;
                    }
                    if (maybes > 1)
                    {
                        continue;
                    }
                    SolveOpcodeX(solution, possibleOpcodes, x);
                    solvedNumbers[x] = true;
                    solvedOpcodes[lastMaybeY] = true;
                    changesMade = true;
                }
                for (int y = 0; y < 16; y++)
                {
                    if (solvedOpcodes[y])
                    {
                        continue;
                    }
                    int maybes = 0;
                    int lastMaybeX = -1;
                    for (int x = 0; x < 16; x++)
                    {
                        if (solution[x, y] == SolveState.Maybe)
                        {
                            maybes++;
                            lastMaybeX = x;
                        }
                    }
                    if (maybes == 0)
                    {
                        opcodes = Array.Empty<Opcode>();
                        return false;
                    }
                    if (maybes > 1)
                    {
                        continue;
                    }
                    SolveOpcodeY(solution, possibleOpcodes, y);
                    solvedNumbers[lastMaybeX] = true;
                    solvedOpcodes[y] = true;
                    changesMade = true;
                }
            }
            foreach (bool solved in solvedNumbers)
            {
                if (!solved)
                {
                    opcodes = Array.Empty<Opcode>();
                    return false;
                }
            }
            foreach (bool solved in solvedOpcodes)
            {
                if (!solved)
                {
                    opcodes = Array.Empty<Opcode>();
                    return false;
                }
            }
            opcodes = new Opcode[16];
            for (int i = 0; i < 16; i++)
            {
                if (possibleOpcodes[i].Count != 1)
                {
                    opcodes = Array.Empty<Opcode>();
                    return false;
                }
                opcodes[i] = possibleOpcodes[i].ToArray()[0];
            }
            return true;
        }

        private void SolveOpcodeX(SolveState[,] solution, Dictionary<int, HashSet<Opcode>> possibleOpcodes, int x)
        {
            int y = -1;
            for (int i = 0; i < 16; i++)
            {
                if (solution[x, i] == SolveState.Maybe)
                {
                    y = i;
                    solution[x, i] = SolveState.True;
                    break;
                }
            }
            for (int i = 0; i < 16; i++)
            {
                if (solution[i, y] == SolveState.Maybe)
                {
                    solution[i, y] = SolveState.False;
                }
                if (i != x)
                {
                    possibleOpcodes[i].Remove((Opcode)y);
                }
            }
        }

        private void SolveOpcodeY(SolveState[,] solution, Dictionary<int, HashSet<Opcode>> possibleOpcodes, int y)
        {
            int x = -1;
            for (int i = 0; i < 16; i++)
            {
                if (solution[i, y] == SolveState.Maybe)
                {
                    x = i;
                    solution[i, y] = SolveState.True;
                    break;
                }
            }
            for (int i = 0; i < 16; i++)
            {
                if (solution[x, i] == SolveState.Maybe)
                {
                    solution[x, i] = SolveState.False;
                }
                if (i != y)
                {
                    possibleOpcodes[x].Remove((Opcode)i);
                }
            }
        }

        private enum Opcode
        {
            addi,
            addr,
            bani,
            banr,
            bori,
            borr,
            eqir,
            eqri,
            eqrr,
            gtir,
            gtri,
            gtrr,
            muli,
            mulr,
            seti,
            setr
        }

        private enum SolveState
        {
            False,
            Maybe,
            True
        }
    }
}
