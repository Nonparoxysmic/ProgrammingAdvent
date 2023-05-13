// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day14 : Day
{
    private static readonly Regex _validLine =
        new("^([0-9]{1,4} [A-Z]{1,6})(, [0-9]{1,4} [A-Z]{1,6}){0,12} => ([0-9]{1,4} [A-Z]{1,6})$");

    private readonly Dictionary<string, Reaction> reactions = new();

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        // Verify that the input is a set of valid reactions.
        reactions.Clear();
        HashSet<string> inputChemicals = new();
        HashSet<string> outputChemicals = new();
        foreach (string line in inputLines)
        {
            Match match = _validLine.Match(line);
            if (!match.Success)
            {
                errorMessage = $"Invalid input line \"{line.Left(20, true)}\".";
                return false;
            }
            Reaction reaction = new(line);
            reactions.Add(reaction.Output.Chemical, reaction);
            foreach ((int quantity, string Chemical) in reaction.Inputs)
            {
                if (quantity == 0)
                {
                    errorMessage = $"Reaction in line \"{line.Left(20, true)}\" contains a zero quantity input.";
                    return false;
                }
                inputChemicals.Add(Chemical);
            }
            if (reaction.Output.Quantity == 0)
            {
                errorMessage = $"Reaction in line \"{line.Left(20, true)}\" produces zero output.";
                return false;
            }
            outputChemicals.Add(reaction.Output.Chemical);
        }
        if (!outputChemicals.Contains("FUEL"))
        {
            errorMessage = "No reaction produces FUEL.";
            return false;
        }
        if (!inputChemicals.Contains("ORE"))
        {
            errorMessage = "No reaction consumes ORE.";
            return false;
        }
        if (outputChemicals.Contains("ORE"))
        {
            errorMessage = "A reaction produces ORE.";
            return false;
        }
        if (inputChemicals.Contains("FUEL"))
        {
            errorMessage = "A reaction consumes FUEL.";
            return false;
        }
        // Verify that the input chemicals are output chemicals of other reactions.
        outputChemicals.Remove("FUEL");
        inputChemicals.Remove("ORE");
        if (!outputChemicals.SetEquals(inputChemicals))
        {
            errorMessage = "Mismatched input chemicals and output chemicals.";
            return false;
        }
        // Verify that FUEL can be produced with ORE alone.
        HashSet<string> targetChemicals = new() { "FUEL" };
        HashSet<string> ingredients = new();
        int timeout = 0;
        while (timeout++ < 1000)
        {
            ingredients.Clear();
            foreach (string chemical in targetChemicals)
            {
                if (chemical == "ORE")
                {
                    continue;
                }
                targetChemicals.Remove(chemical);
                foreach ((int _, string ingredient) in reactions[chemical].Inputs)
                {
                    ingredients.Add(ingredient);
                }
            }
            targetChemicals.UnionWith(ingredients);
            if (targetChemicals.Count == 1 && targetChemicals.Single() == "ORE")
            {
                break;
            }
        }
        if (targetChemicals.Count != 1 || targetChemicals.Single() != "ORE")
        {
            errorMessage = "FUEL cannot be produced by ORE.";
            return true;
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();
        reactions.Clear();
        foreach (string line in inputLines)
        {
            Reaction reaction = new(line);
            reactions.Add(reaction.Output.Chemical, reaction);
        }
        long oreForOneFuel = ReduceToOre(1, "FUEL");
        long fuelForATrillionOre = FuelForATrillionOre(oreForOneFuel);
        return output.WriteAnswers(oreForOneFuel, fuelForATrillionOre);
    }

    private long ReduceFuelToOre(long amount) => ReduceToOre(amount, "FUEL");

    private long ReduceToOre(long amount, string chemical)
    {
        if (chemical == "ORE")
        {
            return amount;
        }
        if (!reactions.ContainsKey(chemical))
        {
            return -404;
        }
        Dictionary<string, long> requiredQuantities = new()
        {
            { chemical, amount }
        };
        Dictionary<string, long> extraQuantities = new();
        int timeout = 0;
        while (timeout++ < 10_000)
        {
            if (requiredQuantities.Count == 1 && requiredQuantities.Single().Key == "ORE")
            {
                return requiredQuantities["ORE"];
            }
            string product = requiredQuantities.First(q => q.Key != "ORE").Key;
            Reaction reaction = reactions[product];
            long amountNeeded = requiredQuantities[product]
                - (extraQuantities.ContainsKey(product) ? extraQuantities[product] : 0);
            if (amountNeeded == 0)
            {
                extraQuantities.Remove(product);
                requiredQuantities.Remove(product);
                continue;
            }
            if (amountNeeded < 0)
            {
                extraQuantities[product] -= requiredQuantities[product];
                requiredQuantities.Remove(product);
                continue;
            }
            long numberOfReactions = amountNeeded / reaction.Output.Quantity
                + (amountNeeded % reaction.Output.Quantity > 0 ? 1 : 0);
            long extraProductProduced = reaction.Output.Quantity * numberOfReactions - requiredQuantities[product];
            if (extraQuantities.ContainsKey(product))
            {
                extraQuantities[product] += extraProductProduced;
            }
            else
            {
                extraQuantities[product] = extraProductProduced;
            }
            requiredQuantities.Remove(product);
            foreach ((int Quantity, string Chemical) in reaction.Inputs)
            {
                if (requiredQuantities.ContainsKey(Chemical))
                {
                    requiredQuantities[Chemical] += Quantity * numberOfReactions;
                }
                else
                {
                    requiredQuantities.Add(Chemical, Quantity * numberOfReactions);
                }
            }
        }
        return -1;
    }

    private long FuelForATrillionOre(long oreForOneFuel)
    {
        long start = 1_000_000_000_000 / oreForOneFuel;
        long increment = start / 10;
        long oreConsumedLowerBound = ReduceFuelToOre(start);
        if (oreConsumedLowerBound == 1_000_000_000_000)
        {
            return start;
        }
        long oreConsumedUpperBound = oreConsumedLowerBound;
        for (long fuelGuess = start; fuelGuess < 2 * start; fuelGuess += increment)
        {
            oreConsumedLowerBound = oreConsumedUpperBound;
            oreConsumedUpperBound = ReduceFuelToOre(fuelGuess + increment);
            if (oreConsumedUpperBound == 1_000_000_000_000)
            {
                return fuelGuess + increment;
            }
            if (oreConsumedLowerBound < 1_000_000_000_000 && oreConsumedUpperBound > 1_000_000_000_000)
            {
                return BinarySearch(fuelGuess, fuelGuess + increment);
            }
        }
        return -1;
    }

    private long BinarySearch(long lowerBound, long upperBound)
    {
        if (upperBound - lowerBound <= 1)
        {
            return lowerBound;
        }
        long middle = (upperBound + lowerBound) / 2;
        long middleFuel = ReduceFuelToOre(middle);
        if (middleFuel == 1_000_000_000_000)
        {
            return middle;
        }
        if (middleFuel > 1_000_000_000_000)
        {
            return BinarySearch(lowerBound, middle);
        }
        return BinarySearch(middle, upperBound);
    }

    private class Reaction
    {
        public (int Quantity, string Chemical) Output { get; private set; }
        public (int Quantity, string Chemical)[] Inputs { get; private set; }

        public Reaction(string text)
        {
            Output = ParseTerm(text[(text.IndexOf(" => ") + 4)..]);
            string[] terms = text[..text.IndexOf(" => ")].Split(", ");
            List<(int, string)> inputs = new();
            foreach (string term in terms)
            {
                inputs.Add(ParseTerm(term));
            }
            Inputs = inputs.ToArray();
        }

        private static (int, string) ParseTerm(string term)
        {
            int quantity = int.Parse(term[..term.IndexOf(' ')]);
            return (quantity, term[(term.IndexOf(' ') + 1)..]);
        }
    }
}
