// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace AdventOfCode2015
{
    static class Day04
    {
        public static void Solve()
        {
            string input1Path = @"Day04\Puzzle\Input1.txt";
            byte[] input1 = new byte[0];
            try
            {
                input1 = File.ReadAllBytes(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }
            // Remove the Unicode byte order mark if there is one at the beginning of the file
            if (input1[0] == 239 && input1[1] == 187 && input1[2] == 191)
            {
                byte[] input1Raw = input1;
                input1 = new byte[input1Raw.Length - 3];
                for (int i = 0; i < input1.Length; i++)
                {
                    input1[i] = input1Raw[i + 3];
                }
            }

            int answer1 = 0;
            using (MD5 algorithm = MD5.Create())
            {
                bool coinHashFound = false;
                while (!coinHashFound)
                {
                    answer1++;
                    byte[] testValue = Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(input1) + answer1);
                    byte[] hash1 = algorithm.ComputeHash(testValue);
                    string firstFive = BitConverter.ToString(hash1).Replace("-", "").Substring(0, 5);
                    if (firstFive == "00000")
                    {
                        coinHashFound = true;
                    }
                }
            }

            Console.WriteLine("Day 4 Part One Answer: " + answer1);
        }
    }
}
