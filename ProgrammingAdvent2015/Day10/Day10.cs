// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.IO;
using System.Text;

namespace ProgrammingAdvent2015
{
    static class Day10
    {
        public static void Solve()
        {
            string input1Path = @"Day10\Puzzle\Input1.txt";
            string input1 = "";
            try
            {
                input1 = File.ReadAllText(input1Path).Trim();
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }
            if (input1.Length == 0) Print.PrintErrorAndExit("Day 10: Empty input in Input1.txt");

            StringBuilder sb = new StringBuilder();

            string result = input1;
            for (int i = 0; i < 40; i++)
            {
                result = LookAndSay(result, sb);
            }

            Console.WriteLine("Day 10 Part One Answer: " + result.Length);

            for (int i = 0; i < 10; i++)
            {
                result = LookAndSay(result, sb);
            }

            Console.WriteLine("Day 10 Part Two Answer: " + result.Length);
        }

        static string LookAndSay(string input, StringBuilder sb)
        {
            sb.Clear();
            char currentLetter = input[0];
            int currentStreak = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == currentLetter)
                {
                    currentStreak++;
                }
                else
                {
                    sb.Append(currentStreak.ToString() + currentLetter);
                    currentLetter = input[i];
                    currentStreak = 1;
                }
            }
            sb.Append(currentStreak.ToString() + currentLetter);

            return sb.ToString();
        }
    }
}
