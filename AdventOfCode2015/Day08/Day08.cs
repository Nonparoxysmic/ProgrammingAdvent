// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.IO;
using System.Text;

namespace AdventOfCode2015
{
    static class Day08
    {
        public static void Solve()
        {
            string input1Path = @"Day08\Puzzle\Input1.txt";
            string[] input1 = new string[0];
            try
            {
                input1 = File.ReadAllLines(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }

            string[] strings = new string[input1.Length];
            StringBuilder sb = new StringBuilder();

            for (int ln = 0; ln < input1.Length; ln++)
            {
                if (input1[ln].Length == 0)
                {
                    PrintErrorInInput("empty line");
                    continue;
                }
                if ((input1[ln].Length < 2) || (input1[ln][0] != '"') || (input1[ln][input1[ln].Length - 1] != '"'))
                {
                    PrintErrorInInput("«" + input1[ln] + "»");
                    continue;
                }

                for (int ch = 1; ch < input1[ln].Length - 1; ch++)
                {
                    if (input1[ln][ch] == '\\')
                    {
                        if (input1[ln][ch + 1] == '\\')
                        {
                            sb.Append('\\');
                        }
                        else if (input1[ln][ch + 1] == '\"')
                        {
                            sb.Append('\"');
                        }
                        else if (input1[ln][ch + 1] == 'x')
                        {
                            string code = input1[ln][ch + 2].ToString() + input1[ln][ch + 3];
                            if (int.TryParse(code, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out int ascii))
                            {
                                sb.Append((char)ascii);
                            }
                            else
                            {
                                PrintErrorInInput("«\\x" + code + "» in «" + input1[ln] + "»");
                            }
                            ch += 2;
                        }
                        else PrintErrorInInput("«" + input1[ln][ch] + input1[ln][ch + 1] + "» in «" + input1[ln] + "»");

                        ch++;
                    }
                    else sb.Append(input1[ln][ch]);
                }

                strings[ln] = sb.ToString();
                sb.Clear();
            }

            int partOneAnswer = 0;
            for (int ln = 0; ln < input1.Length; ln++)
            {
                partOneAnswer += input1[ln].Length - strings[ln].Length;
            }
            Console.WriteLine("Day 8 Part One Answer: " + partOneAnswer);
        }

        static void PrintErrorInInput(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Day 8: Cannot process {0} in Input1.txt", text);
            Console.WriteLine("This may result in an incorrect answer.");
            Console.ResetColor();
        }
    }
}
