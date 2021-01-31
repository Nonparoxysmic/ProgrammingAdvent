// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.Data;
using System.IO;

namespace AdventOfCode2015
{
    static class Day21
    {
        public static void Solve()
        {
            string input1Path = @"Day21\Puzzle\Input1.txt";
            string[] input1 = null;
            try
            {
                input1 = File.ReadAllLines(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }
            if (input1.Length < 3 || !input1[0].StartsWith("Hit Points: ") || !input1[1].StartsWith("Damage: ") || !input1[2].StartsWith("Armor: "))
            {
                Print.PrintErrorAndExit("Day 21: Insufficent data in " + input1Path);
            }
            if (!int.TryParse(input1[0].Substring(12), out int bossHP))
            {
                Print.PrintErrorAndExit("Day 21: Cannot parse as an integer \"" + input1[0].Substring(12) + "\" in " + input1Path);
            }
            if (!int.TryParse(input1[1].Substring(8), out int bossDamage))
            {
                Print.PrintErrorAndExit("Day 21: Cannot parse as an integer \"" + input1[1].Substring(8) + "\" in " + input1Path);
            }
            if (!int.TryParse(input1[2].Substring(7), out int bossArmor))
            {
                Print.PrintErrorAndExit("Day 21: Cannot parse as an integer \"" + input1[2].Substring(7) + "\" in " + input1Path);
            }

            DataTable weapons = new DataTable();
            weapons.Columns.Add(new DataColumn() { DataType = typeof(string), ColumnName = "Name" });
            weapons.PrimaryKey = new DataColumn[1] { weapons.Columns["Name"] };
            weapons.Columns.Add(new DataColumn() { DataType = typeof(int), ColumnName = "Cost" });
            weapons.Columns.Add(new DataColumn() { DataType = typeof(int), ColumnName = "Damage" });
            weapons.Columns.Add(new DataColumn() { DataType = typeof(int), ColumnName = "Armor" });
            DataTable armor = weapons.Clone();
            DataTable rings = weapons.Clone();
            DataTable ringOptions = weapons.Clone();
            AddItemTableRow(weapons, "Dagger", 8, 4, 0);
            AddItemTableRow(weapons, "Shortsword", 10, 5, 0);
            AddItemTableRow(weapons, "Warhammer", 25, 6, 0);
            AddItemTableRow(weapons, "Longsword", 40, 7, 0);
            AddItemTableRow(weapons, "Greataxe", 74, 8, 0);
            AddItemTableRow(armor, "Unarmored", 0, 0, 0);
            AddItemTableRow(armor, "Leather", 13, 0, 1);
            AddItemTableRow(armor, "Chainmail", 31, 0, 2);
            AddItemTableRow(armor, "Splintmail", 53, 0, 3);
            AddItemTableRow(armor, "Bandedmail", 75, 0, 4);
            AddItemTableRow(armor, "Platemail", 102, 0, 5);
            AddItemTableRow(rings, "Damage +1", 25, 1, 0);
            AddItemTableRow(rings, "Damage +2", 50, 2, 0);
            AddItemTableRow(rings, "Damage +3", 100, 3, 0);
            AddItemTableRow(rings, "Defense +1", 20, 0, 1);
            AddItemTableRow(rings, "Defense +2", 40, 0, 2);
            AddItemTableRow(rings, "Defense +3", 80, 0, 3);
            AddItemTableRow(ringOptions, "Ringless", 0, 0, 0);
            for (int i = 0; i < rings.Rows.Count; i++)
            {
                ringOptions.Rows.Add(rings.Rows[i].ItemArray);
                for (int j = i + 1; j < rings.Rows.Count; j++)
                {
                    DataRow row = ringOptions.NewRow();
                    row["Name"] = rings.Rows[i]["Name"] + "," + rings.Rows[j]["Name"];
                    row["Cost"] = (int)rings.Rows[i]["Cost"] + (int)rings.Rows[j]["Cost"];
                    row["Damage"] = (int)rings.Rows[i]["Damage"] + (int)rings.Rows[j]["Damage"];
                    row["Armor"] = (int)rings.Rows[i]["Armor"] + (int)rings.Rows[j]["Armor"];
                    ringOptions.Rows.Add(row);
                }
            }

            int minimumWinCost = int.MaxValue;
            int maximumLossCost = -1;
            for (int weaponChoice = 0; weaponChoice < weapons.Rows.Count; weaponChoice++)
            {
                for (int armorChoice = 0; armorChoice < armor.Rows.Count; armorChoice++)
                {
                    for (int ringChoice = 0; ringChoice < ringOptions.Rows.Count; ringChoice++)
                    {
                        int playerDamage = (int)weapons.Rows[weaponChoice]["Damage"] + (int)ringOptions.Rows[ringChoice]["Damage"];
                        int playerArmor = (int)armor.Rows[armorChoice]["Armor"] + (int)ringOptions.Rows[ringChoice]["Armor"];
                        int cost = (int)weapons.Rows[weaponChoice]["Cost"] + (int)armor.Rows[armorChoice]["Cost"] + (int)ringOptions.Rows[ringChoice]["Cost"];
                        if (PlayerWins(100, playerDamage, playerArmor, bossHP, bossDamage, bossArmor))
                        {
                            if (cost < minimumWinCost) minimumWinCost = cost;
                        }
                        else
                        {
                            if (cost > maximumLossCost) maximumLossCost = cost;
                        }
                    }
                }
            }

            Console.WriteLine("Day 21 Part One Answer: " + minimumWinCost);
            Console.WriteLine("Day 21 Part Two Answer: " + maximumLossCost);
        }

        static bool PlayerWins(int playerHP, int playerDamage, int playerArmor, int bossHP, int bossDamage, int bossArmor)
        {
            float playerDuration = (float)playerHP / Math.Max(bossDamage - playerArmor, 1);
            float bossDuration = (float)bossHP / Math.Max(playerDamage - bossArmor, 1);
            return bossDuration < playerDuration + 1;
        }

        static void AddItemTableRow(DataTable table, string name, int cost, int damage, int armor)
        {
            DataRow row = table.NewRow();
            row["Name"] = name;
            row["Cost"] = cost;
            row["Damage"] = damage;
            row["Armor"] = armor;
            table.Rows.Add(row);
        }
    }
}
