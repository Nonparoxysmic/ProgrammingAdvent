// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day12 : Day
{
    private static readonly Regex _validLine =
        new("^<x=(?<X>-?[0-9]{1,3}), y=(?<Y>-?[0-9]{1,3}), z=(?<Z>-?[0-9]{1,3})>$");

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        if (inputLines.Length != 4)
        {
            errorMessage = "Incorrect number of moons.";
            return false;
        }
        foreach (string line in inputLines)
        {
            if (!_validLine.IsMatch(line))
            {
                errorMessage = $"Invalid input line \"{line.Left(20, true)}\".";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();
        if (!int.TryParse(exampleModifier, out int stepsToSimulate))
        {
            stepsToSimulate = 1000;
        }
        int[] xPositions = new int[inputLines.Length];
        int[] yPositions = new int[inputLines.Length];
        int[] zPositions = new int[inputLines.Length];
        for (int i = 0; i < inputLines.Length; i++)
        {
            Match match = _validLine.Match(inputLines[i]);
            xPositions[i] = int.Parse(match.Groups["X"].Value);
            yPositions[i] = int.Parse(match.Groups["Y"].Value);
            zPositions[i] = int.Parse(match.Groups["Z"].Value);
        }
        Axis[] axes = new Axis[]
        {
            new Axis(xPositions),
            new Axis(yPositions),
            new Axis(zPositions)
        };
        Parallel.ForEach(axes, axis =>
        {
            axis.SimulateTimeSteps(stepsToSimulate);
        });
        int totalEnergy = CalculateTotalEnergy(axes);
        return output.WriteAnswers(totalEnergy, null);
    }

    private static int CalculateTotalEnergy(Axis[] axes)
    {
        if (axes.Length != 3)
        {
            return -1;
        }
        int sum = 0;
        for (int moon = 0; moon < axes[0].Positions.Length; moon++)
        {
            int pot = Math.Abs(axes[0].Positions[moon])
                + Math.Abs(axes[1].Positions[moon])
                + Math.Abs(axes[2].Positions[moon]);
            int kin = Math.Abs(axes[0].Velocities[moon])
                + Math.Abs(axes[1].Velocities[moon])
                + Math.Abs(axes[2].Velocities[moon]);
            sum += pot * kin;
        }
        return sum;
    }

    private class Axis
    {
        public int[] Positions { get; set; }
        public int[] Velocities { get; set; }

        public Axis(int[] positions)
        {
            Positions = positions;
            Velocities = new int[positions.Length];
        }

        public void SimulateTimeSteps(int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                ApplyGravity();
                ApplyVelocities();
            }
        }

        private void ApplyGravity()
        {
            for (int i = 0; i < Positions.Length - 1;i++)
            {
                for (int j = i + 1; j < Positions.Length; j++)
                {
                    if (Positions[i] > Positions[j])
                    {
                        Velocities[i]--;
                        Velocities[j]++;
                    }
                    else if (Positions[i] < Positions[j])
                    {
                        Velocities[i]++;
                        Velocities[j]--;
                    }
                }
            }
        }

        private void ApplyVelocities()
        {
            for (int i = 0; i < Positions.Length; i++)
            {
                Positions[i] += Velocities[i];
            }
        }
    }
}
