// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day21 : Day
{
    private static readonly Regex _validLine = new("^(?<ingredient>[a-z]+)(?: (?<ingredient>[a-z]+))*"
        + " [(]contains (?<allergen>[a-z]+)(?:, (?<allergen>[a-z]+))*[)]$");

    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        foreach (string line in input)
        {
            Match match = _validLine.Match(line);
            if (!match.Success)
            {
                errorMessage = $"Invalid line \"{line.Left(20, true)}\" in input.";
                return false;
            }
            IEnumerable<string> ingredients = match.Groups["ingredient"].Captures.Select(c => c.Value);
            if (ingredients.Except(ingredients.Distinct()).Any())
            {
                errorMessage = $"Line \"{line.Left(20, true)}\" contains duplicate ingredients.";
                return false;
            }
            IEnumerable<string> allergens = match.Groups["allergen"].Captures.Select(c => c.Value);
            if (allergens.Except(allergens.Distinct()).Any())
            {
                errorMessage = $"Line \"{line.Left(20, true)}\" contains duplicate allergens.";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        List<Food> foods = new();
        HashSet<string> ingredients = new();
        HashSet<string> allergens = new();
        foreach (string line in input)
        {
            Match match = _validLine.Match(line);
            if (match.Success)
            {
                IEnumerable<string> thisIngredients = match.Groups["ingredient"].Captures.Select(c => c.Value);
                IEnumerable<string> thisAllergens = match.Groups["allergen"].Captures.Select(c => c.Value);
                ingredients.UnionWith(thisIngredients);
                allergens.UnionWith(thisAllergens);
                foods.Add(new Food(thisIngredients, thisAllergens));
            }
        }

        PossibleAllergens possibleAllergens = new(allergens, foods);
        int partOneAnswer = possibleAllergens.NonallergenCount();
        string? partTwoAnswer = possibleAllergens.CanonicalDangerousIngredientList();
        if (partTwoAnswer is null)
        {
            return output.WriteAnswers(partOneAnswer, "Unable to solve Part Two.");
        }

        return output.WriteAnswers(partOneAnswer, partTwoAnswer);
    }

    private class Food
    {
        public IEnumerable<string> Ingredients { get; private set; }
        public IEnumerable<string> Allergens { get; private set; }

        public Food(IEnumerable<string> ingredients, IEnumerable<string> allergens)
        {
            Ingredients = ingredients;
            Allergens = allergens;
        }
    }

    private class PossibleAllergens
    {
        private readonly List<Food> _foods;
        private readonly List<string> _allergens;
        private readonly Dictionary<string, HashSet<string>> _possibilities;

        public PossibleAllergens(HashSet<string> allergens, List<Food> foods)
        {
            _foods = foods;
            // Sort the allergens alphabetically for Part Two.
            _allergens = allergens.ToList();
            _allergens.Sort();
            // Initialize the dictionary of which ingredients could contain an allergen.
            _possibilities = new();
            // For each allergen...
            foreach (string allergen in allergens)
            {
                // Get the collection of foods that are known to contain that allergen.
                IEnumerable<Food> allergenFoods = foods.Where(f => f.Allergens.Contains(allergen));
                // Initialize the possible ingredients that could contain that allergen.
                _possibilities.Add(allergen, new HashSet<string>(allergenFoods.First().Ingredients));
                // Narrow down the possibilities to those ingredients that 
                // are in every food that is known to contain that allergen.
                foreach (Food food in allergenFoods)
                {
                    _possibilities[allergen].IntersectWith(food.Ingredients);
                }
            }
        }

        public int NonallergenCount()
        {
            // Get the set of all ingredients that could contain an allergen.
            HashSet<string> possibleAllergenIngredients = new(_possibilities.Values.First());
            foreach (string allergen in _allergens)
            {
                possibleAllergenIngredients.UnionWith(_possibilities[allergen]);
            }
            // Count the total appearances of ingredients that are not possible allergens.
            int count = 0;
            foreach (Food food in _foods)
            {
                count += food.Ingredients.Except(possibleAllergenIngredients).Count();
            }
            return count;
        }

        public string? CanonicalDangerousIngredientList()
        {
            bool[] resolved = new bool[_allergens.Count];
            int timeout = _allergens.Count;
            while (timeout-- >= 0)
            {
                for (int i = 0; i < _allergens.Count; i++)
                {
                    if (resolved[i]) { continue; }
                    switch (_possibilities[_allergens[i]].Count)
                    {
                        case 0:
                            return null;
                        case 1:
                            string identified = _possibilities[_allergens[i]].First();
                            for (int j = 0; j < _allergens.Count; j++)
                            {
                                if (j == i) { continue; }
                                _possibilities[_allergens[j]].Remove(identified);
                            }
                            resolved[i] = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            if (!resolved.All(b => b))
            {
                return null;
            }
            IEnumerable<string> ingredients = _allergens.Select(a => _possibilities[a].First());
            return string.Join(',', ingredients);
        }
    }
}
