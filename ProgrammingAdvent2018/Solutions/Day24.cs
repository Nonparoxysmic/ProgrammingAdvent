// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day24 : Day
    {
        static readonly Regex groupLine = new Regex(@"([0-9]{1,9}) units each with ([0-9]{1,9}) hit points (\(([a-z ,;]+)\) )?with an attack that does ([0-9]{1,9}) ([a-z]+) damage at initiative ([0-9]{1,9})");

        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            input = input.Trim();
            if (input == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }
            string[] inputLines = input.ToLines();

            List<Group> groups = new List<Group>();
            List<string> damageTypes = new List<string>();
            Army currentArmy = Army.ImmuneSystem;
            foreach (string line in inputLines)
            {
                if (line == "")
                {
                    continue;
                }
                if (line == "Immune System:")
                {
                    currentArmy = Army.ImmuneSystem;
                    continue;
                }
                if (line == "Infection:")
                {
                    currentArmy = Army.Infection;
                    continue;
                }
                Match groupMatch = groupLine.Match(line);
                if (!groupMatch.Success)
                {
                    output.WriteError($"Invalid line: \"{line}\"", sw);
                    return output;
                }
                int numberOfUnits = int.Parse(groupMatch.Groups[1].Value);
                int unitHitPoints = int.Parse(groupMatch.Groups[2].Value);
                string weaknessesAndImmunities = groupMatch.Groups[4].Value;
                int attackDamage = int.Parse(groupMatch.Groups[5].Value);
                string damageTypeName = groupMatch.Groups[6].Value;
                int initiative = int.Parse(groupMatch.Groups[7].Value);

                int damageIndex = damageTypes.IndexOf(damageTypeName);
                if (damageIndex < 0)
                {
                    damageIndex = damageTypes.Count;
                    damageTypes.Add(damageTypeName);
                }

                groups.Add(new Group(numberOfUnits, unitHitPoints, attackDamage,
                    damageIndex, initiative, currentArmy, weaknessesAndImmunities));
            }
            foreach (Group group in groups)
            {
                group.ProcessWeaknessesAndImmunities(damageTypes);
            }

            int partOneAnswer = RemainingUnits(groups);

            int partTwoAnswer = RemainingImmuneUnitsAfterMinimumBoost(groups);

            sw.Stop();
            output.WriteAnswers(partOneAnswer, partTwoAnswer, sw);
            return output;
        }

        private int RemainingUnits(List<Group> groupList, int boost = 0)
        {
            List<Group> groups = Group.CopyList(groupList);
            if (boost > 0)
            {
                for (int i = 0; i < groups.Count; i++)
                {
                    if (groups[i].Army == Army.ImmuneSystem)
                    {
                        groups[i].Boost = boost;
                    }
                }
            }
            int totalUnits = -1;
            while (true)
            {
                // Target selection phase
                groups = groups.OrderByDescending(x => x.EffectivePower << 8 | x.Initiative).ToList();
                for (int i = 0; i < groups.Count; i++)
                {
                    // Group "groups[i]" chooses a target.
                    int mostDamageDealt = -1;
                    for (int j = 0; j < groups.Count; j++)
                    {
                        if (groups[j].IsTargeted || groups[i].Army == groups[j].Army)
                        {
                            continue;
                        }
                        int damageDealt = groups[j].DamageReceived(groups[i].EffectivePower, groups[i].AttackDamageType);
                        if (damageDealt == mostDamageDealt)
                        {
                            if (groups[j].EffectivePower > groups[i].Target.EffectivePower ||
                                (groups[j].EffectivePower == groups[i].Target.EffectivePower &&
                                groups[j].Initiative > groups[i].Target.Initiative))
                            {
                                groups[i].Target = groups[j];
                            }
                        }
                        else if (damageDealt > mostDamageDealt)
                        {
                            groups[i].Target = groups[j];
                            mostDamageDealt = damageDealt;
                        }
                    }
                    if (mostDamageDealt > 0)
                    {
                        groups[i].Target.IsTargeted = true;
                    }
                    else
                    {
                        groups[i].Target = null;
                    }
                }

                // Attacking phase
                groups = groups.OrderByDescending(x => x.Initiative).ToList();
                for (int i = 0; i < groups.Count; i++)
                {
                    // Group "groups[i]" attacks.
                    if (groups[i].Target == null)
                    {
                        continue;
                    }
                    if (groups[i].Units > 0)
                    {
                        groups[i].DealDamageToTarget();
                    }
                }

                // Reset groups for next round.
                List<Group> oldList = groups;
                groups = new List<Group>();
                foreach (Group group in oldList)
                {
                    if (group.Units > 0)
                    {
                        group.Target = null;
                        group.IsTargeted = false;
                        groups.Add(group);
                    }
                }

                // Combat ends once one army has lost all of its units.
                int[] remainingUnits = new int[2];
                foreach (Group group in groups)
                {
                    remainingUnits[(int)group.Army] += group.Units;
                }
                if (remainingUnits[0] == 0 || remainingUnits[1] == 0)
                {
                    break;
                }

                // If no units were killed in a round, it's a stalemate.
                if (remainingUnits[0] + remainingUnits[1] == totalUnits)
                {
                    return -1;
                }
                totalUnits = remainingUnits[0] + remainingUnits[1];
            }
            int sum = 0;
            if (boost > 0)
            {
                foreach (Group group in groups)
                {
                    if (group.Army == Army.ImmuneSystem)
                    {
                        sum += group.Units;
                    }
                }
                return sum;
            }
            foreach (Group group in groups)
            {
                sum += group.Units;
            }
            return sum;
        }

        private int RemainingImmuneUnitsAfterMinimumBoost(List<Group> groups)
        {
            for (int i = 1; i < 2048; i++)
            {
                int unitsRemaining = RemainingUnits(groups, i);
                if (unitsRemaining > 0)
                {
                    return unitsRemaining;
                }
            }
            return -1;
        }

        private class Group
        {
            private int _units;
            public int Units
            {
                get
                {
                    return _units;
                }
                set
                {
                    _units = value;
                    EffectivePower = _units * (AttackDamage + Boost);
                }
            }

            public int UnitHP { get; private set; }
            public int AttackDamage { get; private set; }
            public int AttackDamageType { get; private set; }
            public int Initiative { get; private set; }
            public Army Army { get; private set; }
            public int EffectivePower { get; private set; }
            public int[] Weaknesses { get; private set; }
            public int[] Immunities { get; private set; }

            public Group Target { get; set; }
            public bool IsTargeted { get; set; }

            private int _boost;
            public int Boost
            {
                get
                {
                    return _boost;
                }
                set
                {
                    _boost = value;
                    EffectivePower = Units * (AttackDamage + _boost);
                }
            }

            private readonly string _weaknessesAndImmunities;

            public Group(int unitCount, int unitHP, int attDamage, int attType,
                int initiative, Army army, string weaknessesAndImmunities)
            {
                Units = unitCount;
                UnitHP = unitHP;
                AttackDamage = attDamage;
                AttackDamageType = attType;
                Initiative = initiative;
                Army = army;
                EffectivePower = unitCount * attDamage;
                _weaknessesAndImmunities = weaknessesAndImmunities;
            }

            public Group(Group groupToCopy)
            {
                _units = groupToCopy.Units;
                UnitHP = groupToCopy.UnitHP;
                AttackDamage = groupToCopy.AttackDamage;
                AttackDamageType = groupToCopy.AttackDamageType;
                Initiative = groupToCopy.Initiative;
                Army = groupToCopy.Army;
                EffectivePower = groupToCopy.EffectivePower;
                Weaknesses = groupToCopy.Weaknesses.ToArray();
                Immunities = groupToCopy.Immunities.ToArray();
                Target = groupToCopy.Target;
                IsTargeted = groupToCopy.IsTargeted;
                _boost = groupToCopy.Boost;
            }

            public void ProcessWeaknessesAndImmunities(List<string> damageTypes)
            {
                List<int> weaknesses = new List<int>();
                List<int> immunities = new List<int>();
                string[] split = _weaknessesAndImmunities.Split(';');
                foreach (string input in split)
                {
                    string[] words = input.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length < 3)
                    {
                        continue;
                    }
                    List<int> currentList;
                    if (words[0] == "weak")
                    {
                        currentList = weaknesses;
                    }
                    else if (words[0] == "immune")
                    {
                        currentList = immunities;
                    }
                    else continue;
                    for (int i = 2; i < words.Length; i++)
                    {
                        int damageIndex = damageTypes.IndexOf(words[i]);
                        if (damageIndex >= 0)
                        {
                            currentList.Add(damageIndex);
                        }
                    }
                }
                Weaknesses = weaknesses.ToArray();
                Immunities = immunities.ToArray();
            }

            public int DamageReceived(int damageAmount, int damageType)
            {
                if (Weaknesses.Contains(damageType))
                {
                    return 2 * damageAmount;
                }
                if (Immunities.Contains(damageType))
                {
                    return 0;
                }
                return damageAmount;
            }

            public void DealDamageToTarget()
            {
                int damageDealt = Target.DamageReceived(EffectivePower, AttackDamageType);
                int unitsLost = damageDealt / Target.UnitHP;
                Target.Units -= unitsLost;
            }

            public static List<Group> CopyList(List<Group> groups)
            {
                List<Group> output = new List<Group>();
                foreach (Group group in groups)
                {
                    output.Add(new Group(group));
                }
                return output;
            }
        }

        private enum Army
        {
            ImmuneSystem,
            Infection
        }
    }
}
