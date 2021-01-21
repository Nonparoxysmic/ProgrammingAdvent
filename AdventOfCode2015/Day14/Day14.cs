// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2015
{
    static class Day14
    {
        public static void Solve()
        {
            string input1Path = @"Day14\Puzzle\Input1.txt";
            string[] input1 = null;
            try
            {
                input1 = File.ReadAllLines(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }
            if (input1.Length < 2)
            {
                Print.PrintErrorAndExit("Day 14: Input too short.");
            }

            List<Reindeer> reindeer = new List<Reindeer>();

            foreach (string line in input1)
            {
                string[] words = line.Split();
                if (words.Length != 15) Print.PrintErrorAndExit("Day 14: Cannot process \"" + line + "\" in Input1.txt");
                if (int.TryParse(words[3], out int topSpeed) && int.TryParse(words[6], out int topSpeedDuration) && int.TryParse(words[13], out int restDuration))
                {
                    reindeer.Add(new Reindeer(words[0], topSpeed, topSpeedDuration, restDuration));
                }
                else Print.PrintErrorAndExit("Day 14: Cannot process \"" + line + "\" in Input1.txt");
            }

            int raceDuration1 = 2503;

            int longestDistance = 0;
            foreach (Reindeer r in reindeer)
            {
                r.Race(raceDuration1);
                longestDistance = Math.Max(longestDistance, r.position);
            }

            Console.WriteLine("Day 14 Part One Answer: " + longestDistance);
        }
    }

    class Reindeer
    {
        public string name;
        public int topSpeed;
        public int topSpeedDuration;
        public int restDuration;
        public int position;

        public Reindeer(string name, int topSpeed, int topSpeedDuration, int restDuration)
        {
            this.name = name;
            this.topSpeed = topSpeed;
            this.topSpeedDuration = topSpeedDuration;
            this.restDuration = restDuration;
            position = 0;
        }

        public void Race(int duration)
        {
            int timeRemaining = duration;
            while (timeRemaining > 0)
            {
                int flyingTime = Math.Min(timeRemaining, topSpeedDuration);
                position += flyingTime * topSpeed;
                timeRemaining -= flyingTime;
                timeRemaining -= Math.Min(timeRemaining, restDuration);
            }
        }
    }
}
