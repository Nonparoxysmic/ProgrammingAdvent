// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Collections.Generic;
using System.IO;

namespace ProgrammingAdvent2015
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

            foreach (Reindeer r in reindeer)
            {
                r.position = 0;
            }

            for (int seconds = 0; seconds < raceDuration1; seconds++)
            {
                int leaderPos = 0;
                foreach (Reindeer r in reindeer)
                {
                    leaderPos = Math.Max(leaderPos, r.NextPosition());
                }
                foreach (Reindeer r in reindeer)
                {
                    if (r.position == leaderPos) r.score++;
                }
            }

            int highestScore = 0;
            foreach (Reindeer r in reindeer)
            {
                highestScore = Math.Max(highestScore, r.score);
            }

            Console.WriteLine("Day 14 Part Two Answer: " + highestScore);
        }
    }

    class Reindeer
    {
        public string name;
        public int topSpeed;
        public int topSpeedDuration;
        public int restDuration;
        public int position;

        public int score;
        int flyingCountdown;
        int restingCountdown;

        public Reindeer(string name, int topSpeed, int topSpeedDuration, int restDuration)
        {
            this.name = name;
            this.topSpeed = topSpeed;
            this.topSpeedDuration = topSpeedDuration;
            this.restDuration = restDuration;
            position = 0;

            score = 0;
            flyingCountdown = topSpeedDuration;
            restingCountdown = 0;
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

        public int NextPosition()
        {
            if (flyingCountdown > 0)
            {
                position += topSpeed;
                flyingCountdown--;
                if (flyingCountdown == 0) restingCountdown = restDuration;
            }
            else
            {
                restingCountdown--;
                if (restingCountdown == 0) flyingCountdown = topSpeedDuration;
            }

            return position;
        }
    }
}
