// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day04 : Day
    {
        private readonly Regex newGuardLine = new Regex(@"^\[1518-\d\d-\d\d \d\d:\d\d] Guard #(\d+) begins shift$");
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

            // Put input lines in chronological order.
            Array.Sort(inputLines);

            // Create DataTable for input data.
            DataTable guardSleep = new DataTable();
            guardSleep.Columns.Add("ID", typeof(int));
            guardSleep.PrimaryKey = new DataColumn[] { guardSleep.Columns["ID"] };
            guardSleep.Columns.Add("Total", typeof(int));
            for (int i = 0; i < 60; i++)
            {
                guardSleep.Columns.Add(i.ToString(), typeof(int));
            }
            foreach (DataColumn column in guardSleep.Columns)
            {
                column.DefaultValue = 0;
            }

            // Compile the input data and put it in the DataTable.
            GuardLog currentLog = null;
            foreach (string line in inputLines)
            {
                Match newGuard = newGuardLine.Match(line);
                if (newGuard.Success)
                {
                    if (currentLog != null)
                    {
                        AddGuardLog(guardSleep, currentLog);
                    }
                    currentLog = new GuardLog(newGuard.Groups[1].Value);
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
            if (currentLog != null)
            {
                AddGuardLog(guardSleep, currentLog);
            }

            // Find the guard that has the most minutes asleep.
            int mostTotalSleep = -1;
            int mostSleepGuard = -1;
            foreach (DataRow row in guardSleep.Rows)
            {
                int totalSleep = row.Field<int>("Total");
                if (totalSleep > mostTotalSleep)
                {
                    mostTotalSleep = totalSleep;
                    mostSleepGuard = row.Field<int>("ID");
                }
            }

            // Find the minute that guard spends asleep the most.
            DataRow mostSleepGuardRow = guardSleep.Rows.Find(mostSleepGuard);
            int mostMinuteSleep = -1;
            int mostSleepGuardsMostSleepMinute = -1;
            for (int i = 0; i < 60; i++)
            {
                int minuteSleep = mostSleepGuardRow.Field<int>(i.ToString());
                if (minuteSleep > mostMinuteSleep)
                {
                    mostMinuteSleep = minuteSleep;
                    mostSleepGuardsMostSleepMinute = i;
                }
            }

            // Calculate Part One answer.
            int partOneAnswer = mostSleepGuard * mostSleepGuardsMostSleepMinute;

            // Find which guard is most frequently asleep on the same minute.
            int mostSleep = -1;
            int mostSleepMinute = -1;
            int mostSleepMinutesGuard = -1;
            foreach (DataRow row in guardSleep.Rows)
            {
                for (int i = 0; i < 60; i++)
                {
                    int sleep = row.Field<int>(i.ToString());
                    if (sleep > mostSleep)
                    {
                        mostSleep = sleep;
                        mostSleepMinute = i;
                        mostSleepMinutesGuard = row.Field<int>("ID");
                    }
                }
            }

            // Calculate Part Two answer.
            int partTwoAnswer = mostSleepMinute * mostSleepMinutesGuard;

            sw.Stop();
            output.WriteAnswers(partOneAnswer, partTwoAnswer, sw);
            return output;
        }

        private void AddGuardLog(DataTable table, GuardLog guardLog)
        {
            if (!table.Rows.Contains(guardLog.ID))
            {
                table.Rows.Add(guardLog.ID);
            }
            DataRow row = table.Rows.Find(guardLog.ID);
            row["Total"] = row.Field<int>("Total") + guardLog.TimeAsleep();
            bool isAwake = true;
            for (int i = 0; i < 60; i++)
            {
                if (guardLog.IsToggle(i))
                {
                    isAwake = !isAwake;
                }
                if (!isAwake)
                {
                    row[i.ToString()] = row.Field<int>(i.ToString()) + 1;
                }
            }
        }

        class GuardLog
        {
            public int ID { get; }

            readonly List<int> sleepToggles = new List<int>();

            public GuardLog(string id)
            {
                ID = int.Parse(id);
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
