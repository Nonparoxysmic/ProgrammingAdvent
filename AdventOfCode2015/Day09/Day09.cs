// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace AdventOfCode2015
{
    static class Day09
    {
        public static void Solve()
        {
            string input1Path = @"Day09\Puzzle\Input1.txt";
            string[] input1 = new string[0];
            try
            {
                input1 = File.ReadAllLines(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }

            List<string> locations = new List<string>();

            DataTable distanceTable = new DataTable("Distances");
            distanceTable.Columns.Add(new DataColumn() { DataType = typeof(string), ColumnName = "Location" });
            distanceTable.PrimaryKey = new DataColumn[1] { distanceTable.Columns["Location"] };

            foreach (string line in input1)
            {
                string[] data = line.Split();
                if ((data.Length != 5) || (data[1] != "to") || (data[3] != "="))
                {
                    PrintErrorInInput("\"" + line + "\"");
                    continue;
                }
                if (!int.TryParse(data[4], out int distance))
                {
                    PrintErrorInInput("as an integer \"" + data[4] + "\" in \"" + line + "\"");
                    continue;
                }

                if (!locations.Contains(data[0]))
                {
                    AddRowAndColumn(data[0], distanceTable);
                    locations.Add(data[0]);
                }
                if (!locations.Contains(data[2]))
                {
                    AddRowAndColumn(data[2], distanceTable);
                    locations.Add(data[2]);
                }

                SetValue(data[0], data[2], distance, distanceTable);
            }
            foreach (string location in locations)
            {
                SetValue(location, location, 0, distanceTable);
            }

            int numOfPermutations = Factorial(locations.Count);



            Console.WriteLine("Day 9 Part One Answer: " + "TO BE IMPLEMENTED");
        }

        static void AddRowAndColumn(string name, DataTable table)
        {
            table.Columns.Add(new DataColumn() { DataType = typeof(int), ColumnName = name });
            DataRow row = table.NewRow();
            row["Location"] = name;
            table.Rows.Add(row);
        }

        static void SetValue(string name1, string name2, int value, DataTable table)
        {
            DataRow row = table.Rows.Find(name1);
            row[name2] = value;
            row = table.Rows.Find(name2);
            row[name1] = value;
        }

        static int GetValue(string name1, string name2, DataTable table)
        {
            DataRow row = table.Rows.Find(name1);
            return (int)row[name2];
        }

        static int Factorial(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException();
            int output = 1;
            while (n > 1)
            {
                output *= n;
                n--;
            }
            return output;
        }

        static void PrintErrorInInput(string text)
        {
            Console.WriteLine("Day 9: Cannot process {0} in Input1.txt", text);
        }
    }
}
