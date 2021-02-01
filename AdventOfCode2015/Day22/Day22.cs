// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.IO;

namespace AdventOfCode2015
{
    static class Day22
    {
        static int leastManaRequired = int.MaxValue;
        static int bossDamage;

        public static void Solve()
        {
            string input1Path = @"Day22\Puzzle\Input1.txt";
            string[] input1 = null;
            try
            {
                input1 = File.ReadAllLines(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }
            if (input1.Length < 2 || !input1[0].StartsWith("Hit Points: ") || !input1[1].StartsWith("Damage: "))
            {
                Print.PrintErrorAndExit("Day 22: Insufficent data in " + input1Path);
            }
            if (!int.TryParse(input1[0].Substring(12), out int bossHP))
            {
                Print.PrintErrorAndExit("Day 22: Cannot parse as an integer \"" + input1[0].Substring(12) + "\" in " + input1Path);
            }
            if (!int.TryParse(input1[1].Substring(8), out bossDamage))
            {
                Print.PrintErrorAndExit("Day 22: Cannot parse as an integer \"" + input1[1].Substring(8) + "\" in " + input1Path);
            }

            DoPlayerTurn(50, 500, 0, bossHP, 0, 0, 0);

            Console.WriteLine("Day 22 Part One Answer: " + leastManaRequired);
        }

        static void DoPlayerTurn(int playerHP, int playerMana, int manaSpent, int bossHP, int shieldTimer, int poisonTimer, int rechargeTimer)
        {
            // Process effects at start of player turn
            if (shieldTimer > 0) shieldTimer--;
            if (poisonTimer > 0)
            {
                bossHP -= 3;
                poisonTimer--;
            }
            if (rechargeTimer > 0)
            {
                playerMana += 101;
                rechargeTimer--;
            }
            // Player wins if boss died
            if (bossHP < 1)
            {
                if (manaSpent < leastManaRequired) leastManaRequired = manaSpent;
                return;
            }
            // Player casts a spell
            for (int i = 0; i < 5; i++)
            {
                switch (i)
                {
                    case 0:
                        // Magic Missile
                        if (playerMana < 53) break;
                        DoBossTurn(playerHP, playerMana - 53, manaSpent + 53, bossHP - 4, shieldTimer, poisonTimer, rechargeTimer);
                        break;
                    case 1:
                        // Drain
                        if (playerMana < 73) break;
                        DoBossTurn(playerHP + 2, playerMana - 73, manaSpent + 73, bossHP - 2, shieldTimer, poisonTimer, rechargeTimer);
                        break;
                    case 2:
                        // Shield
                        if (playerMana < 113 || shieldTimer > 0) break;
                        DoBossTurn(playerHP, playerMana - 113, manaSpent + 113, bossHP, 6, poisonTimer, rechargeTimer);
                        break;
                    case 3:
                        // Poison
                        if (playerMana < 173 || poisonTimer > 0) break;
                        DoBossTurn(playerHP, playerMana - 173, manaSpent + 173, bossHP, shieldTimer, 6, rechargeTimer);
                        break;
                    case 4:
                        // Recharge
                        if (playerMana < 229 || rechargeTimer > 0) break;
                        DoBossTurn(playerHP, playerMana - 229, manaSpent + 229, bossHP, shieldTimer, poisonTimer, 5);
                        break;
                }
            }
        }

        static void DoBossTurn(int playerHP, int playerMana, int manaSpent, int bossHP, int shieldTimer, int poisonTimer, int rechargeTimer)
        {
            // Process effects at start of boss turn
            if (shieldTimer > 0) shieldTimer--;
            if (poisonTimer > 0)
            {
                bossHP -= 3;
                poisonTimer--;
            }
            if (rechargeTimer > 0)
            {
                playerMana += 101;
                rechargeTimer--;
            }
            // Player wins if boss died
            if (bossHP < 1)
            {
                if (manaSpent < leastManaRequired) leastManaRequired = manaSpent;
                return;
            }
            // Boss deals damage
            if (shieldTimer > 0)
            {
                playerHP -= Math.Max(bossDamage - 7, 1);
            }
            else
            {
                playerHP -= bossDamage;
            }
            // Player loses if player died
            if (playerHP < 1) return;
            // Boss turn ends
            DoPlayerTurn(playerHP, playerMana, manaSpent, bossHP, shieldTimer, poisonTimer, rechargeTimer);
        }
    }
}
