// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day04 : Day
    {
        private readonly Regex newGuardLine = new Regex(@"^\[(1518-\d\d-\d\d \d\d:\d\d)] Guard #(\d+) begins shift$");
        private readonly Regex sleepToggleLine = new Regex(@"^\[1518-\d\d-\d\d \d\d:(\d\d)] (falls asleep|wakes up)$");

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
            Array.Sort(inputLines);

            List<GuardLog> logs = new List<GuardLog>();
            GuardLog currentLog = null;
            foreach (string line in inputLines)
            {
                Match newGuard = newGuardLine.Match(line);
                if (newGuard.Success)
                {
                    if (currentLog != null) { logs.Add(currentLog); }
                    currentLog = new GuardLog(newGuard.Groups[2].Value, newGuard.Groups[1].Value);
                    continue;
                }
                Match sleepToggle = sleepToggleLine.Match(line);
                if (sleepToggle.Success)
                {
                    if (currentLog != null)
                    {
                        currentLog.AddSleepToggle(sleepToggle.Groups[1].Value);
                    }
                    continue;
                }
                output.WriteError($"Invalid line {line} in input.", sw);
                return output;
            }
            logs.Add(currentLog);

            Dictionary<int, int> totalSleep = new Dictionary<int, int>();
            foreach (GuardLog log in logs)
            {
                if (totalSleep.ContainsKey(log.ID))
                {
                    totalSleep[log.ID] += log.TimeAsleep();
                }
                else { totalSleep.Add(log.ID, log.TimeAsleep()); }
            }
            int mostSleep = -1;
            int mostSleepGuard = -1;
            foreach (var kvp in totalSleep)
            {
                if (kvp.Value > mostSleep)
                {
                    mostSleep = kvp.Value;
                    mostSleepGuard = kvp.Key;
                }
            }

            Dictionary<int, int> mostSleepGuardMinutes = new Dictionary<int, int>();
            for (int i = 0; i < 60; i++)
            {
                mostSleepGuardMinutes.Add(i, 0);
            }
            bool isAwake = true;
            foreach (GuardLog log in logs)
            {
                if (log.ID != mostSleepGuard) { continue; }
                for (int i = 0; i < 60; i++)
                {
                    if (log.IsToggle(i))
                    {
                        isAwake = !isAwake;
                    }
                    if (!isAwake)
                    {
                        mostSleepGuardMinutes[i]++;
                    }
                }
            }
            mostSleep = -1;
            int mostSleepMinute = -1;
            foreach (var kvp in mostSleepGuardMinutes)
            {
                if (kvp.Value > mostSleep)
                {
                    mostSleep = kvp.Value;
                    mostSleepMinute = kvp.Key;
                }
            }

            int partOneAnswer = mostSleepGuard * mostSleepMinute;

            sw.Stop();
            output.WriteAnswers(partOneAnswer, null, sw);
            return output;
        }

        class GuardLog
        {
            public int ID { get; }
            public DateTime ShiftStart { get; }

            readonly List<int> sleepToggles = new List<int>();

            public GuardLog(string id, string shiftStart)
            {
                ID = int.Parse(id);
                ShiftStart = DateTime.Parse(shiftStart);
            }

            public void AddSleepToggle(string minute)
            {
                sleepToggles.Add(int.Parse(minute));
            }

            public int TimeAsleep()
            {
                int sum = 0;
                for (int i = 0; i < sleepToggles.Count; i += 2)
                {
                    if (i < sleepToggles.Count - 1)
                    {
                        sum += sleepToggles[i + 1] - sleepToggles[i];
                    }
                    else
                    {
                        sum += 60 - sleepToggles[i];
                    }
                }
                return sum;
            }

            public bool IsToggle(int number)
            {
                return sleepToggles.Contains(number);
            }
        }
    }
}
