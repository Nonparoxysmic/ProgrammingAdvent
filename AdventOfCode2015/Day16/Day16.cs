// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace AdventOfCode2015
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

            Console.WriteLine("Number of rows in input = {0}", sueTable.Rows.Count);


            List<string> potentialMatches = new List<string>();

            foreach (string line in input1)
            {
                if (line.Contains("children: ") && !line.Contains("children: 3"))
                {
                    continue;
                }
                if (line.Contains("cats: ") && !line.Contains("cats: 7"))
                {
                    continue;
                }
                if (line.Contains("samoyeds: ") && !line.Contains("samoyeds: 2"))
                {
                    continue;
                }
                if (line.Contains("pomeranians: ") && !line.Contains("pomeranians: 3"))
                {
                    continue;
                }
                if (line.Contains("akitas: ") && !line.Contains("akitas: 0"))
                {
                    continue;
                }
                if (line.Contains("vizslas: ") && !line.Contains("vizslas: 0"))
                {
                    continue;
                }
                if (line.Contains("goldfish: ") && !line.Contains("goldfish: 5"))
                {
                    continue;
                }
                if (line.Contains("trees: ") && !line.Contains("trees: 3"))
                {
                    continue;
                }
                if (line.Contains("cars: ") && !line.Contains("cars: 2"))
                {
                    continue;
                }
                if (line.Contains("perfumes: ") && !line.Contains("perfumes: 1"))
                {
                    continue;
                }
                potentialMatches.Add(line);
            }

            if (potentialMatches.Count == 0)
            {
                Console.WriteLine("Day 16 Part One: No Valid Answer");
            }
            else if (potentialMatches.Count == 1)
            {
                string sueNumber = potentialMatches[0].Split()[1].Trim(':');
                Console.WriteLine("Day 16 Part One Answer: " + sueNumber);
            }
            else
            {
                // Manual check by user. Didn't need this for my solution.
                Console.WriteLine("Day 16 Part One: Compare the following potential matches manually.");
                foreach (string sue in potentialMatches) Console.WriteLine(" >> " + sue);
            }


            Console.WriteLine("Day 16 Part One Answer: " + "To Be Implemented Properly");
        }
    }
}
