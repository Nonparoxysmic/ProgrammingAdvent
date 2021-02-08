// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Collections.Generic;
using System.IO;

namespace ProgrammingAdvent2015
{
    static class Day15
    {
        public static int bestScore;
        public static int best500CalScore;

        public static void Solve()
        {
            string input1Path = @"Day15\Puzzle\Input1.txt";
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
                Print.PrintErrorAndExit("Day 15: Input too short.");
            }

            List<Ingredient> ingredients = new List<Ingredient>();

            foreach (string line in input1)
            {
                string[] words = line.Split();
                if (words.Length != 11) Print.PrintErrorAndExit("Day 15: Cannot process \"" + line + "\" in Input1.txt");

                if (int.TryParse(words[2].Trim(','), out int capacity) && 
                    int.TryParse(words[4].Trim(','), out int durability) &&
                    int.TryParse(words[6].Trim(','), out int flavor) &&
                    int.TryParse(words[8].Trim(','), out int texture) &&
                    int.TryParse(words[10], out int calories))
                {
                    ingredients.Add(new Ingredient(words[0].Trim(':'), capacity, durability, flavor, texture, calories));
                }
                else Print.PrintErrorAndExit("Day 15: Cannot process \"" + line + "\" in Input1.txt");
            }
            if (ingredients.Count < 2) Print.PrintErrorAndExit("Day 15: Input too short.");

            int[] recipe = new int[ingredients.Count];

            NextQuantity(ingredients, recipe, 0, 0);

            Console.WriteLine("Day 15 Part One Answer: " + bestScore);
            Console.WriteLine("Day 15 Part Two Answer: " + best500CalScore);
        }

        static void NextQuantity(List<Ingredient> ingredients, int[] recipe, int pos, int quantitySoFar)
        {
            if ((quantitySoFar == 100) || (pos == recipe.Length - 1))
            {
                recipe[pos] = 100 - quantitySoFar;
                Cookie cookie = new Cookie(ingredients, recipe);
                bestScore = Math.Max(bestScore, cookie.CalculateScore());
                if (cookie.calories == 500)
                {
                    best500CalScore = Math.Max(best500CalScore, cookie.score);
                }
                return;
            }
            else
            {
                for (int i = 0; i <= 100 - quantitySoFar; i++)
                {
                    recipe[pos] = i;
                    NextQuantity(ingredients, recipe, pos + 1, quantitySoFar + i);
                }
            }
        }
    }

    class Ingredient
    {
        public string name;
        public int capacity;
        public int durability;
        public int flavor;
        public int texture;
        public int calories;

        public Ingredient(string name, int capacity, int durability, int flavor, int texture, int calories)
        {
            this.name = name;
            this.capacity = capacity;
            this.durability = durability;
            this.flavor = flavor;
            this.texture = texture;
            this.calories = calories;
        }
    }

    class Cookie
    {
        readonly Dictionary<Ingredient, int> ingredients;
        public int score = -1;
        public int calories = 0;

        public Cookie()
        {
            ingredients = new Dictionary<Ingredient, int>();
        }

        public Cookie(List<Ingredient> availableIngredients, int[] recipe)
        {
            if (availableIngredients.Count != recipe.Length) throw new ArgumentException();
            ingredients = new Dictionary<Ingredient, int>();
            for (int i = 0; i < recipe.Length; i++)
            {
                if (recipe[i] > 0) ingredients.Add(availableIngredients[i], recipe[i]);
            }
        }

        public void AddIngredient(Ingredient i, int quantity)
        {
            ingredients.Add(i, quantity);
        }

        public int CalculateScore()
        {
            int capacity = 0;
            int durability = 0;
            int flavor = 0;
            int texture = 0;
            foreach (KeyValuePair<Ingredient, int> i in ingredients)
            {
                capacity += i.Key.capacity * i.Value;
                durability += i.Key.durability * i.Value;
                flavor += i.Key.flavor * i.Value;
                texture += i.Key.texture * i.Value;

                calories += i.Key.calories * i.Value;
            }
            if (capacity < 1 || durability < 1 || flavor < 1 || texture < 1)
            {
                score = 0;
                return 0;
            }
            else
            {
                score = capacity * durability * flavor * texture;
                return score;
            }
        }
    }
}
