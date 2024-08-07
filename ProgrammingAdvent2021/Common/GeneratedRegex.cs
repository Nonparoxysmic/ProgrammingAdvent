// ProgrammingAdvent2021 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using System.Text.RegularExpressions;

namespace ProgrammingAdvent2021.Common;

internal static partial class GeneratedRegex
{
    public static readonly Regex ValidDayName = ValidDayNameRegex();

    [GeneratedRegex("^Day(?<DayNumber>0[1-9]|1[0-9]|2[0-5])$")]
    private static partial Regex ValidDayNameRegex();

    public static readonly Regex ValidDay02InputLine = Day02Regex();

    [GeneratedRegex("^(forward |down |up )(?<Magnitude>[0-9]{1,2})$")]
    private static partial Regex Day02Regex();
}
