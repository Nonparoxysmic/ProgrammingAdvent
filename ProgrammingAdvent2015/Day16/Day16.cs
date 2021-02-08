// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace ProgrammingAdvent2015
{
    static class Day16
    {
        public static void Solve()
        {
            string input1Path = @"Day16\Puzzle\Input1.txt";
            string[] input1 = null;
            try
            {
                input1 = File.ReadAllLines(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }

            DataTable sueTable = new DataTable();
            sueTable.Columns.Add(new DataColumn() { DataType = typeof(string), ColumnName = "id", Unique = true });
            sueTable.PrimaryKey = new DataColumn[1] { sueTable.Columns["id"] };
            string[] columnNames = new string[] { "children", "cats", "samoyeds", "pomeranians", "akitas", "vizslas", "goldfish", "trees", "cars", "perfumes" };
            foreach (string name in columnNames)
            {
                sueTable.Columns.Add(new DataColumn() { DataType = typeof(int), ColumnName = name });
            }
            Dictionary<string, int> trueSue = new Dictionary<string, int>();
            int[] trueValues = new int[] { 3, 7, 2, 3, 0, 0, 5, 3, 2, 1 };
            for (int i = 0; i < trueValues.Length; i++)
            {
                trueSue.Add(columnNames[i], trueValues[i]);
            }

            foreach (string line in input1)
            {
                string[] words = line.Split();
                if (words.Length < 4 || words[0] != "Sue") continue;
                string key = words[1].Trim(':');
                if (sueTable.Rows.Contains(key))
                {
                    Print.PrintErrorAndExit("Day 16: Duplicate ID \"" + key + "\" in Input1.txt");
                }
                DataRow row = sueTable.NewRow();
                row["id"] = key;
                for (int i = 2; i < words.Length - 1; i += 2)
                {
                    if (int.TryParse(words[i + 1].Trim(','), out int value))
                    {
                        string column = words[i].Trim(':');
                        if (!columnNames.Contains(column)) Print.PrintErrorAndExit("Day 16: Invalid data \"" + words[i] + " " + words[i + 1] + "\" in Input1.txt");
                        row[column] = value;
                    }
                    else Print.PrintErrorAndExit("Day 16: Invalid data \"" + words[i] + " " + words[i + 1] + "\" in Input1.txt");
                }
                sueTable.Rows.Add(row);
            }

            List<string> potentialMatches = new List<string>();
            foreach (DataRow row in sueTable.Rows)
            {
                bool hasConflicts = false;
                foreach (string column in columnNames)
                {
                    if (row[column] == DBNull.Value) continue;
                    if ((int)row[column] != trueSue[column]) hasConflicts = true;
                }
                if (!hasConflicts) potentialMatches.Add(row["id"].ToString());
            }

            if (potentialMatches.Count == 0)
            {
                Print.PrintErrorAndExit("Invalid input for Day 16 Part One: No valid answer");
            }
            else if (potentialMatches.Count == 1)
            {
                Console.WriteLine("Day 16 Part One Answer: " + potentialMatches[0]);
            }
            else
            {
                Print.PrintErrorAndExit("Invalid input for Day 16 Part One: Multiple possible answers: " + String.Join(", ", potentialMatches.ToArray()));
            }

            potentialMatches = new List<string>();
            Dictionary<string, int> comparisons = new Dictionary<string, int>();
            int[] comparisonValues = new int[] { 0, 1, 0, -1, 0, 0, -1, 1, 0, 0 };
            for (int i = 0; i < comparisonValues.Length; i++)
            {
                comparisons.Add(columnNames[i], comparisonValues[i]);
            }
            foreach (DataRow row in sueTable.Rows)
            {
                bool hasConflicts = false;
                foreach (string column in columnNames)
                {
                    if (row[column] == DBNull.Value) continue;
                    if (comparisons[column] == 0)
                    {
                        if ((int)row[column] != trueSue[column]) hasConflicts = true;
                    }
                    else if (comparisons[column] > 0)
                    {
                        if (!((int)row[column] > trueSue[column])) hasConflicts = true;
                    }
                    else
                    {
                        if (!((int)row[column] < trueSue[column])) hasConflicts = true;
                    }
                }
                if (!hasConflicts) potentialMatches.Add(row["id"].ToString());
            }

            if (potentialMatches.Count == 0)
            {
                Print.PrintErrorAndExit("Invalid input for Day 16 Part Two: No valid answer");
            }
            else if (potentialMatches.Count == 1)
            {
                Console.WriteLine("Day 16 Part Two Answer: " + potentialMatches[0]);
            }
            else
            {
                Print.PrintErrorAndExit("Invalid input for Day 16 Part Two: Multiple possible answers: " + String.Join(", ", potentialMatches.ToArray()));
            }
        }
    }
}
