// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using System.Text.RegularExpressions;
using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal partial class Day19 : Day
{
    public static readonly Regex ValidInputLine = ValidInputLineRegex();

    [GeneratedRegex("^Blueprint (?<id>[1-9][0-9]?): Each ore robot costs (?<oreOre>[1-9]) ore[.] Each clay robot costs (?<clayOre>[1-9]) ore[.] Each obsidian robot costs (?<obsidianOre>[1-9]) ore and (?<obsidianClay>[1-9][0-9]?) clay[.] Each geode robot costs (?<geodeOre>[1-9]) ore and (?<geodeObsidian>[1-9][0-9]?) obsidian[.]$")]
    private static partial Regex ValidInputLineRegex();

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        Blueprint[] blueprints = ParseInput(input);
        int sumOfQualityLevels = blueprints.Aggregate(0, (sum, current) => sum + QualityLevel(current));

        return result.WriteAnswers(sumOfQualityLevels, null);
    }

    private static Blueprint[] ParseInput(string[] input)
    {
        List<Blueprint> blueprints = [];
        foreach (string line in input)
        {
            Match match = ValidInputLine.Match(line);
            if (match.Success)
            {
                int id = int.Parse(match.Groups["id"].Value);
                int oreOre = int.Parse(match.Groups["oreOre"].Value);
                int clayOre = int.Parse(match.Groups["clayOre"].Value);
                int obsidianOre = int.Parse(match.Groups["obsidianOre"].Value);
                int obsidianClay = int.Parse(match.Groups["obsidianClay"].Value);
                int geodeOre = int.Parse(match.Groups["geodeOre"].Value);
                int geodeObsidian = int.Parse(match.Groups["geodeObsidian"].Value);
                blueprints.Add(new Blueprint(id, oreOre, clayOre, obsidianOre, obsidianClay, geodeOre, geodeObsidian));
            }
        }
        return [.. blueprints];
    }

    private static int QualityLevel(Blueprint blueprint)
    {
        return blueprint.ID * MostGeodesOpenedInMinutes(24, blueprint);
    }

    private static int MostGeodesOpenedInMinutes(int minutes, Blueprint blueprint)
    {
        Search search = new(minutes, blueprint);
        search.TryRobot(0, blueprint.OreRobotOreCost + 1);
        search.TryRobot(1, blueprint.ClayRobotOreCost + 1);
        return search.MostGeodesOpened;
    }

    private static int Divide(int a, int b)
    {
        if (b == 0)
        {
            return 999;
        }
        return a % b == 0 ? a / b : a / b + 1;
    }

    private readonly struct Blueprint(int id, int oror, int clor, int obor, int obcl, int geor, int geob)
    {
        public int ID { get; } = id;
        public int OreRobotOreCost { get; } = oror;
        public int ClayRobotOreCost { get; } = clor;
        public int ObsidianRobotOreCost { get; } = obor;
        public int ObsidianRobotClayCost { get; } = obcl;
        public int GeodeRobotOreCost { get; } = geor;
        public int GeodeRobotObsidianCost { get; } = geob;
        public int MaxOreCost { get; } = MoreMath.Max(oror, clor, obor, geor);
    }

    private enum Robot
    {
        Ore = 0,
        Clay = 1,
        Obsidian = 2,
        Geode = 3,
    }

    private class Search
    {
        public int Minutes { get; }
        public Blueprint Blueprint { get; }
        public int MostGeodesOpened { get; private set; }

        private int MaxOreCost => Blueprint.MaxOreCost;
        private int MaxClayCost => Blueprint.ObsidianRobotClayCost;
        private int MaxObsidianCost => Blueprint.GeodeRobotObsidianCost;

        private int minutesPassed;
        private int geodesOpened;
        private readonly Stack<int> robotBuildOrder;
        private readonly int[] robotsBuilt;
        private readonly int[] currentResources;

        public Search(int minutes, Blueprint blueprint)
        {
            Minutes = minutes;
            Blueprint = blueprint;
            MostGeodesOpened = 0;
            minutesPassed = 0;
            geodesOpened = 0;
            robotBuildOrder = [];
            robotBuildOrder.Push(0);
            robotsBuilt = [1, 0, 0, 0];
            currentResources = new int[4];
        }

        public void TryRobot(int type, int time)
        {
            BuildRobot(type, time);

            MostGeodesOpened = Math.Max(MostGeodesOpened, geodesOpened);

            int oreTime = currentResources[0] >= Blueprint.OreRobotOreCost ?
                0 : Divide(Blueprint.OreRobotOreCost - currentResources[0], robotsBuilt[0]);
            int clayTime = currentResources[0] >= Blueprint.ClayRobotOreCost ?
                0 : Divide(Blueprint.ClayRobotOreCost - currentResources[0], robotsBuilt[0]);
            int obsidianTime = Math.Max(
                currentResources[0] >= Blueprint.ObsidianRobotOreCost ?
                    0 : Divide(Blueprint.ObsidianRobotOreCost - currentResources[0], robotsBuilt[0]),
                currentResources[1] >= Blueprint.ObsidianRobotClayCost ?
                    0 : Divide(Blueprint.ObsidianRobotClayCost - currentResources[1], robotsBuilt[1])
                );
            int geodeTime = Math.Max(
                currentResources[0] >= Blueprint.GeodeRobotOreCost ?
                    0 : Divide(Blueprint.GeodeRobotOreCost - currentResources[0], robotsBuilt[0]),
                currentResources[2] >= Blueprint.GeodeRobotObsidianCost ?
                    0 : Divide(Blueprint.GeodeRobotObsidianCost - currentResources[2], robotsBuilt[2])
                );

            bool buildOre = robotsBuilt[0] < MaxOreCost
                && (minutesPassed + oreTime < Minutes - 1);
            bool buildClay = robotsBuilt[1] < MaxClayCost
                && (minutesPassed + clayTime < Minutes - 2);
            bool buildObsidian = robotsBuilt[2] < MaxObsidianCost && robotsBuilt[1] > 0
                && (minutesPassed + obsidianTime < Minutes - 1);
            bool buildGeode = robotsBuilt[2] > 0
                && (minutesPassed + geodeTime < Minutes);

            if (BestCaseScenario() > MostGeodesOpened)
            {
                if (buildGeode)
                {
                    TryRobot(3, geodeTime + 1);
                }
                if (buildObsidian)
                {
                    TryRobot(2, obsidianTime + 1);
                }
                if (buildClay)
                {
                    TryRobot(1, clayTime + 1);
                }
                if (buildOre)
                {
                    TryRobot(0, oreTime + 1);
                }
            }

            UnbuildRobot(type, time);
        }

        private int BestCaseScenario()
        {
            int timeLeft = Minutes - minutesPassed;
            return geodesOpened + timeLeft * (timeLeft - 1) / 2;
        }

        public void BuildRobot(int type, int time)
        {
            minutesPassed += time;
            for (int i = 0; i < 4; i++)
            {
                currentResources[i] += robotsBuilt[i] * time;
            }
            robotBuildOrder.Push(type);
            robotsBuilt[type]++;
            switch ((Robot)type)
            {
                case Robot.Ore:
                    currentResources[0] -= Blueprint.OreRobotOreCost;
                    break;
                case Robot.Clay:
                    currentResources[0] -= Blueprint.ClayRobotOreCost;
                    break;
                case Robot.Obsidian:
                    currentResources[0] -= Blueprint.ObsidianRobotOreCost;
                    currentResources[1] -= Blueprint.ObsidianRobotClayCost;
                    break;
                case Robot.Geode:
                    currentResources[0] -= Blueprint.GeodeRobotOreCost;
                    currentResources[2] -= Blueprint.GeodeRobotObsidianCost;
                    geodesOpened += Minutes - minutesPassed;
                    break;
            }
        }

        public void UnbuildRobot(int type, int time)
        {
            switch ((Robot)type)
            {
                case Robot.Ore:
                    currentResources[0] += Blueprint.OreRobotOreCost;
                    break;
                case Robot.Clay:
                    currentResources[0] += Blueprint.ClayRobotOreCost;
                    break;
                case Robot.Obsidian:
                    currentResources[0] += Blueprint.ObsidianRobotOreCost;
                    currentResources[1] += Blueprint.ObsidianRobotClayCost;
                    break;
                case Robot.Geode:
                    currentResources[0] += Blueprint.GeodeRobotOreCost;
                    currentResources[2] += Blueprint.GeodeRobotObsidianCost;
                    geodesOpened -= Minutes - minutesPassed;
                    break;
            }
            robotsBuilt[type]--;
            robotBuildOrder.Pop();
            for (int i = 0; i < 4; i++)
            {
                currentResources[i] -= robotsBuilt[i] * time;
            }
            minutesPassed -= time;
        }
    }
}
