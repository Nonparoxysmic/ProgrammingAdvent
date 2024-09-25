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

    public static readonly Regex ValidHexadecimal = ValidHexadecimalRegex();

    [GeneratedRegex("^[0-9A-F]+$")]
    private static partial Regex ValidHexadecimalRegex();

    public static readonly Regex ValidDay02InputLine = Day02Regex();

    [GeneratedRegex("^(forward |down |up )(?<Magnitude>[0-9]{1,2})$")]
    private static partial Regex Day02Regex();

    public static readonly Regex ValidDay05InputLine = Day05Regex();

    [GeneratedRegex("^(?<x1>[0-9]{1,3}),(?<y1>[0-9]{1,3}) -> (?<x2>[0-9]{1,3}),(?<y2>[0-9]{1,3})$")]
    private static partial Regex Day05Regex();

    public static readonly Regex ValidDay12InputLine = Day12Regex();

    [GeneratedRegex("^(?<cave0>([a-z]{1,2}|[A-Z]{1,2}|start|end))-(?<cave1>([a-z]{1,2}|[A-Z]{1,2}|start|end))$")]
    private static partial Regex Day12Regex();

    public static readonly Regex ValidDay13Coordinates = Day13CoordinateRegex();

    [GeneratedRegex("^(?<x>[0-9]{1,4}),(?<y>[0-9]{1,4})$")]
    private static partial Regex Day13CoordinateRegex();

    public static readonly Regex ValidDay13Fold = Day13FoldRegex();

    [GeneratedRegex("^fold along (?<axis>[xy])=(?<coordinate>[0-9]{1,4})$")]
    private static partial Regex Day13FoldRegex();

    public static readonly Regex ValidDay14Template = Day14TemplateRegex();

    [GeneratedRegex("^[A-Z]{2,32}$")]
    private static partial Regex Day14TemplateRegex();

    public static readonly Regex ValidDay14Rule = Day14RuleRegex();

    [GeneratedRegex("^(?<pair>[A-Z]{2}) -> (?<insertion>[A-Z])$")]
    private static partial Regex Day14RuleRegex();

    public static readonly Regex ValidDay17Input = Day17Regex();

    [GeneratedRegex("^target area: x=(?<xMin>[0-9]{1,3})[.]{2}(?<xMax>[0-9]{1,3}), y=(?<yMin>-?[0-9]{1,3})[.]{2}(?<yMax>-?[0-9]{1,3})$")]
    private static partial Regex Day17Regex();

    public static readonly Regex ValidDay19Scanner = Day19ScannerRegex();

    [GeneratedRegex("^--- scanner (?<scanner>[0-9]{1,2}) ---$")]
    private static partial Regex Day19ScannerRegex();

    public static readonly Regex ValidDay19Coordinates = Day19CoordinatesRegex();

    [GeneratedRegex("^(?<x>-?[0-9]{1,3}),(?<y>-?[0-9]{1,3}),(?<z>-?[0-9]{1,3})$")]
    private static partial Regex Day19CoordinatesRegex();

    public static readonly Regex ValidDay21InputLine = Day21Regex();

    [GeneratedRegex("^Player (?<player>[12]) starting position: (?<start>[1-9]|10)$")]
    private static partial Regex Day21Regex();
}
