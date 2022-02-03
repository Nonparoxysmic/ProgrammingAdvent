// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day07 : Day
    {
        private DataTable dataTable;
        private string[] names;

        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (input == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }
            string[] inputLines = input.ToLines();
            foreach (string line in inputLines)
            {
                if (!Regex.IsMatch(line, @"^[a-z]+ \(\d+\)( -> [a-z, ]+$)*$"))
                {
                    output.WriteError($"Invalid line: \"{line}\"", sw);
                    return output;
                }
            }
            List<string> nameList = new List<string>();
            foreach (string line in inputLines)
            {
                nameList.Add(Regex.Match(line, @"^[a-z]+(?= )").Value);
            }
            names = nameList.ToArray();

            CreateDataTable();
            for (int i = 0; i < inputLines.Length; i++)
            {
                try
                {
                    DataRow row = dataTable.NewRow();
                    row["ID"] = i;
                    row["Name"] = names[i];
                    row["Weight"] = int.Parse(Regex.Match(inputLines[i], @"(?<=\()\d+(?=\))").Value);
                    dataTable.Rows.Add(row);
                }
                catch
                {
                    output.WriteError($"Invalid data: \"{inputLines[i]}\"", sw);
                    return output;
                }
            }
            for (int i = 0; i < inputLines.Length; i++)
            {
                if (Regex.IsMatch(inputLines[i], @"(?<=( -> ))[a-z, ]+$"))
                {
                    string[] heldPrograms =
                        Regex.Match(inputLines[i], @"(?<=( -> ))[a-z, ]+$").Value
                        .Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string p in heldPrograms)
                    {
                        int j = Array.IndexOf(names, p);
                        dataTable.Rows[j]["Parent"] = dataTable.Rows[i]["ID"];
                    }
                }
            }

            int count = 0;
            string bottomProgram = "";
            foreach (DataRow row in dataTable.Rows)
            {
                if (row["Parent"] == DBNull.Value)
                {
                    count++;
                    bottomProgram = row["Name"].ToString();
                }
            }
            if (count != 1)
            {
                output.WriteError("Invalid data.", sw);
                return output;
            }

            sw.Stop();
            output.WriteAnswers(bottomProgram, null, sw);
            return output;
        }

        private void CreateDataTable()
        {
            dataTable = new DataTable();

            DataColumn column = new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "ID",
                ReadOnly = true,
                Unique = true
            };
            dataTable.Columns.Add(column);

            column = new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "Name",
                ReadOnly = true,
                Unique = true
            };
            dataTable.Columns.Add(column);

            column = new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "Weight",
                ReadOnly = true,
                Unique = false
            };
            dataTable.Columns.Add(column);

            column = new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "Parent",
                ReadOnly = false,
                Unique = false,
                AllowDBNull = true
            };
            dataTable.Columns.Add(column);

            DataColumn[] primaryKeyColumns = new DataColumn[1];
            primaryKeyColumns[0] = dataTable.Columns["ID"];
            dataTable.PrimaryKey = primaryKeyColumns;
        }
    }
}
