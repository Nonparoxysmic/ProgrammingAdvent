// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

namespace ProgrammingAdvent2021.Solutions;

internal class Day06 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        List<int> fish = input[0].Split(',').Select(str => int.Parse(str)).ToList();
        for (int day = 0; day < 80; day++)
        {
            int len = fish.Count;
            for (int i = 0; i < len; i++)
            {
                if (fish[i] == 0)
                {
                    fish[i] = 6;
                    fish.Add(8);
                }
                else
                {
                    fish[i]--;
                }
            }
        }
        return ($"{fish.Count}", "n/a");
    }
}
