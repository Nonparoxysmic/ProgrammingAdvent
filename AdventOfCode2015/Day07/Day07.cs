// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2015
{
    static class Day07
    {
        public static void Solve()
        {
            string input1Path = @"Day07\Puzzle\Input1.txt";
            string[] input1 = new string[0];
            try
            {
                input1 = File.ReadAllLines(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }

            List<string> wires = new List<string>();
            List<Connection> connections = new List<Connection>();

            foreach (string line in input1)
            {
                string[] io = line.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries);
                if (io.Length != 2)
                {
                    PrintErrorInInput(line);
                    continue;
                }
                string inputA = io[0];
                string inputB = inputA;
                Gate gate = Gate.None;
                int operationPos = io[0].IndexOf("NOT ");
                if (operationPos == 0)
                {
                    inputA = io[0].Substring(4);
                    inputB = inputA;
                    gate = Gate.Not;
                }
                operationPos = io[0].IndexOf(" AND ");
                if (operationPos > 0)
                {
                    inputA = io[0].Substring(0, operationPos);
                    inputB = io[0].Substring(operationPos + 5);
                    gate = Gate.And;
                }
                operationPos = io[0].IndexOf(" OR ");
                if (operationPos > 0)
                {
                    inputA = io[0].Substring(0, operationPos);
                    inputB = io[0].Substring(operationPos + 4);
                    gate = Gate.Or;
                }
                operationPos = io[0].IndexOf(" LSHIFT ");
                if (operationPos > 0)
                {
                    inputA = io[0].Substring(0, operationPos);
                    inputB = io[0].Substring(operationPos + 8);
                    gate = Gate.LShift;
                }
                operationPos = io[0].IndexOf(" RSHIFT ");
                if (operationPos > 0)
                {
                    inputA = io[0].Substring(0, operationPos);
                    inputB = io[0].Substring(operationPos + 8);
                    gate = Gate.RShift;
                }

                if (!wires.Contains(inputA) && !int.TryParse(inputA, out _)) wires.Add(inputA);
                if (!wires.Contains(inputB) && !int.TryParse(inputB, out _)) wires.Add(inputB);
                if (!wires.Contains(io[1]) && !int.TryParse(io[1], out _)) wires.Add(io[1]);

                Connection connection = new Connection { inputA = inputA, inputB = inputB, gate = gate, output = io[1] };
                connections.Add(connection);
            }

            Dictionary<string, ushort> wireValues = new Dictionary<string, ushort>();

            int connectionsMade = 1;
            int loopCount = 0;
            while (connectionsMade > 0)
            {
                connectionsMade = 0;
                foreach (Connection connection in connections)
                {
                    if (wireValues.ContainsKey(connection.output)) continue;
                    switch (connection.gate)
                    {
                        case Gate.None:
                            if (ushort.TryParse(connection.inputA, out ushort value))
                            {
                                wireValues.Add(connection.output, value);
                                connectionsMade++;
                            }
                            else if (wireValues.TryGetValue(connection.inputA, out ushort value2))
                            {
                                wireValues.Add(connection.output, value2);
                                connectionsMade++;
                            }
                            break;
                        case Gate.Not:
                            if (wireValues.TryGetValue(connection.inputA, out ushort value3))
                            {
                                wireValues.Add(connection.output, (ushort)~value3);
                                connectionsMade++;
                            }
                            break;
                        case Gate.And:
                            if (wireValues.TryGetValue(connection.inputA, out ushort value4) && wireValues.TryGetValue(connection.inputB, out ushort value5))
                            {
                                wireValues.Add(connection.output, (ushort)(value4 & value5));
                                connectionsMade++;
                            }
                            break;
                        case Gate.Or:
                            if (wireValues.TryGetValue(connection.inputA, out ushort value6) && wireValues.TryGetValue(connection.inputB, out ushort value7))
                            {
                                wireValues.Add(connection.output, (ushort)(value6 | value7));
                                connectionsMade++;
                            }
                            break;
                        case Gate.LShift:
                            if (wireValues.TryGetValue(connection.inputA, out ushort value8) && ushort.TryParse(connection.inputB, out ushort value9))
                            {
                                wireValues.Add(connection.output, (ushort)(value8 << value9));
                                connectionsMade++;
                            }
                            break;
                        case Gate.RShift:
                            if (wireValues.TryGetValue(connection.inputA, out ushort value10) && ushort.TryParse(connection.inputB, out ushort value11))
                            {
                                wireValues.Add(connection.output, (ushort)(value10 >> value11));
                                connectionsMade++;
                            }
                            break;
                    }
                }
                loopCount++;
                Console.WriteLine("wireValues.Count = {0}", wireValues.Count);
            }

            Console.WriteLine("wires.Count = {0}", wires.Count);
            Console.WriteLine("loopcount = {0}", loopCount);


            foreach (KeyValuePair<string, ushort> kvp in wireValues)
            {
                Console.WriteLine("Wire {0} = {1}", kvp.Key, kvp.Value);
            }

            Console.WriteLine("NOT COMPLETE - WORK IN PROGRESS");

        }

        static void PrintErrorInInput(string text)
        {
            Console.WriteLine("Day 7: Cannot process \"{0}\" in Input1.txt", text);
        }
    }

    public enum Gate
    {
        None,
        Not,
        And,
        Or,
        LShift,
        RShift
    }

    class Connection
    {
        public string inputA;
        public string inputB;
        public Gate gate;
        public string output;
    }
}
