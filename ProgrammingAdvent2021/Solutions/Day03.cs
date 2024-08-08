// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

namespace ProgrammingAdvent2021.Solutions;

internal class Day03 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        int[] gammaBits = Enumerable.Repeat((int)'0', input[0].Length).ToArray();
        foreach (string line in input)
        {
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '1')
                {
                    gammaBits[i] += 1;
                }
                else
                {
                    gammaBits[i] -= 1;
                }
            }
        }
        char[] epsilonBits = new char[gammaBits.Length];
        for (int i = 0; i < gammaBits.Length; i++)
        {
            if (gammaBits[i] > '1')
            {
                gammaBits[i] = '1';
            }
            if (gammaBits[i] < '0')
            {
                gammaBits[i] = '0';
            }
            if (gammaBits[i] == '1')
            {
                epsilonBits[i] = '0';
            }
            else
            {
                epsilonBits[i] = '1';
            }
        }
        int gamma = Convert.ToInt32(new string(gammaBits.Select(n => (char)n).ToArray()), 2);
        int epsilon = Convert.ToInt32(new string(epsilonBits), 2);
        return ($"{gamma * epsilon}", "n/a");
    }
}
