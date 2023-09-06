// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day04 : Day
{
    private static readonly string KEY = "(byr|iyr|eyr|hgt|hcl|ecl|pid|cid)";
    private static readonly string VALUE = "([#0-9a-z]{1,12})";

    private static readonly Regex _validLine = new($"^{KEY}:{VALUE}( {KEY}:{VALUE}){{0,7}}$");

    private static readonly Regex _validHeight = new("^(?<quantity>[0-9]{2,3})(?<unit>cm|in)$");
    private static readonly Regex _validHairColor = new("^#[0-9a-f]{6}$");
    private static readonly Regex _validEyeColor = new("^(amb|blu|brn|gry|grn|hzl|oth)$");
    private static readonly Regex _validPassportID = new("^[0-9]{9}$");

    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        foreach (string line in input)
        {
            if (line.Length == 0)
            {
                continue;
            }
            if (!_validLine.IsMatch(line))
            {
                errorMessage = $"Invalid line \"{line.Left(20, true)}\" in input.";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        List<Passport> passports = ReadInput(input);
        int partOneAnswer = passports.Count(p => p.HasRequiredFields);
        int partTwoAnswer = passports.Count(p => p.IsValid);

        return output.WriteAnswers(partOneAnswer, partTwoAnswer);
    }

    private static List<Passport> ReadInput(string[] input)
    {
        List<Passport> output = new();
        Passport? current = null;
        foreach (string line in input)
        {
            if (line.Length == 0)
            {
                if (current is not null)
                {
                    output.Add(current);
                    current = null;
                }
                continue;
            }
            current ??= new();
            string[] terms = line.Split();
            foreach (string kvp in terms)
            {
                string key = kvp[0..3];
                string value = kvp[4..^0];
                current.SetField(key, value);
            }
        }
        if (current is not null)
        {
            output.Add(current);
        }
        return output;
    }

    private class Passport
    {
        public string? BirthYear { get; set; }
        public string? IssueYear { get; set; }
        public string? ExpirationYear { get; set; }
        public string? Height { get; set; }
        public string? HairColor { get; set; }
        public string? EyeColor { get; set; }
        public string? PassportID { get; set; }
        public string? CountryID { get; set; }

        public bool HasRequiredFields
        {
            get
            {
                return BirthYear is not null
                    && IssueYear is not null
                    && ExpirationYear is not null
                    && Height is not null
                    && HairColor is not null
                    && EyeColor is not null
                    && PassportID is not null;
            }
        }
        public bool IsValid => FieldsAreValid();

        public void SetField(string key, string value)
        {
            switch (key)
            {
                case "byr":
                    BirthYear = value;
                    break;
                case "iyr":
                    IssueYear = value;
                    break;
                case "eyr":
                    ExpirationYear = value;
                    break;
                case "hgt":
                    Height = value;
                    break;
                case "hcl":
                    HairColor = value;
                    break;
                case "ecl":
                    EyeColor = value;
                    break;
                case "pid":
                    PassportID = value;
                    break;
                case "cid":
                    CountryID = value;
                    break;
            }
        }

        private bool FieldsAreValid()
        {
            if (!int.TryParse(BirthYear, out int byr) || byr < 1920 || byr > 2002)
            {
                return false;
            }
            if (!int.TryParse(IssueYear, out int iyr) || iyr < 2010 || iyr > 2020)
            {
                return false;
            }
            if (!int.TryParse(ExpirationYear, out int eyr) || eyr < 2020 || eyr > 2030)
            {
                return false;
            }
            if (Height is null || HairColor is null
                || EyeColor is null || PassportID is null)
            {
                return false;
            }
            Match heightMatch = _validHeight.Match(Height);
            if (!heightMatch.Success)
            {
                return false;
            }
            int quantity = int.Parse(heightMatch.Groups["quantity"].Value);
            if (heightMatch.Groups["unit"].Value == "cm")
            {
                if (quantity < 150 || quantity > 193)
                {
                    return false;
                }
            }
            else
            {
                if (quantity < 59 || quantity > 76)
                {
                    return false;
                }
            }
            return _validHairColor.IsMatch(HairColor)
                && _validEyeColor.IsMatch(EyeColor)
                && _validPassportID.IsMatch(PassportID);
        }
    }
}
