// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text;
using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day25 : Day
{
    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0 || inputLines[0].Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        if (!ValidateIntcodeInput(inputLines, out errorMessage))
        {
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        Droid droid = new(inputLines[0]);
        if (!droid.FindItemsAndGoToSecurityCheckpoint())
        {
            return output.WriteError("Unable to find all items and reach security checkpoint.");
        }
        if (!droid.PassThroughSecurityCheckpoint() || droid.Password is null)
        {
            return output.WriteError("No combination of items allowed passing through security sensor.");
        }

        return output.WriteAnswers(droid.Password, null);
    }

    private class Droid
    {
        public static string[] DoNotTake = new string[] { "photons", "escape pod", "molten lava", "infinite loop", "giant electromagnet" };

        private static readonly Regex _sensorFailure = new("A loud, robotic voice says \"Alert! Droids on this ship are (?<targetWeight>heavier|lighter) than the detected value!\" and you are ejected back to the checkpoint.");
        private static readonly Regex _sensorSuccess = new("\"Oh, hello! You should be able to get in by typing (?<password>[0-9]+) on the keypad at the main airlock.\"");

        public string? Password { get; private set; } = null;
        public string CurrentLocation { get; private set; } = string.Empty;
        public List<string> DoorsHere { get; private set; } = new();
        public List<string> ItemsHere { get; private set; } = new();
        public List<string> Inventory { get; private set; } = new();

        private readonly Day09.Day09Program _program;
        private readonly string _code;
        private List<string> _securityCheckpointPath = new();
        private readonly Stack<string> _currentPath = new();
        private string? _sensorDirection = null;

        public Droid(string code)
        {
            _code = code;
            _program = new(_code);
            while (_program.Tick()) { }
            UpdateLocation();
        }

        public bool FindItemsAndGoToSecurityCheckpoint()
        {
            List<string> firstDoors = new(DoorsHere);
            foreach (string door in firstDoors)
            {
                FindItems(door);
            }
            return GoToSecurityCheckpoint();
        }

        public bool PassThroughSecurityCheckpoint()
        {
            // Make a fixed list of all items and drop inventory.
            List<string> allItems = new(Inventory);
            foreach (string item in allItems)
            {
                DropItem(item);
            }
            // Try passing with no items.
            if (TryPassSecuritySensor(out string _))
            {
                return true;
            }
            // Try passing with one item.
            List<string> tooHeavy = new();
            foreach (string item in allItems)
            {
                TakeItem(item);
                if (TryPassSecuritySensor(out string targetWeight))
                {
                    return true;
                }
                else if (targetWeight == "lighter")
                {
                    tooHeavy.Add(item);
                }
                DropItem(item);
            }
            // Remove items that are too heavy on their own.
            foreach (string item in tooHeavy)
            {
                allItems.Remove(item);
            }
            // Try all combinations of items.
            for (int count = 2; count <= allItems.Count; count++)
            {
                Combinations combo = new(allItems.Count, count);
                while (combo.Next())
                {
                    foreach (int i in combo.Combination)
                    {
                        TakeItem(allItems[i]);
                    }
                    if (TryPassSecuritySensor(out string _))
                    {
                        return true;
                    }
                    foreach (int i in combo.Combination)
                    {
                        DropItem(allItems[i]);
                    }
                }
            }
            // No combination of items succeeded.
            return false;
        }

        private void UpdateLocation()
        {
            string[] output = ReadOutput();
            CurrentLocation = output[3][3..^3];
            DoorsHere.Clear();
            int itemIndex = 10;
            for (int i = 7; i < output.Length - 2; i++)
            {
                if (output[i].Length == 0)
                {
                    break;
                }
                DoorsHere.Add(output[i][2..]);
                itemIndex = i + 3;
            }
            ItemsHere.Clear();
            for (int i = itemIndex; i < output.Length - 2; i++)
            {
                if (output[i].Length == 0)
                {
                    break;
                }
                ItemsHere.Add(output[i][2..]);
            }
        }

        private string[] ReadOutput()
        {
            StringBuilder sb = new();
            while (_program.OutputCount > 0)
            {
                sb.Append((char)_program.DequeueOutput());
            }
            return sb.ToString().ToLines();
        }

        private void InputCommand(string command)
        {
            foreach (char c in command)
            {
                _program.EnqueueInput(c);
            }
            _program.EnqueueInput(10);
        }

        private void FindItems(string door)
        {
            Move(door);
            _currentPath.Push(door);
            string reverse = ReverseDoor(door);
            if (CurrentLocation == "Security Checkpoint")
            {
                _securityCheckpointPath = new(_currentPath);
                _securityCheckpointPath.Reverse();
                _sensorDirection = DoorsHere.Where(dir => dir != reverse).FirstOrDefault();
            }
            else
            {
                foreach (string item in ItemsHere)
                {
                    if (!DoNotTake.Contains(item))
                    {
                        TakeItem(item);
                    }
                }
                List<string> currentDoors = new(DoorsHere);
                foreach (string nextDoor in currentDoors)
                {
                    if (nextDoor == reverse)
                    {
                        continue;
                    }
                    FindItems(nextDoor);
                }
            }
            Move(reverse);
            _currentPath.Pop();
        }

        private static string ReverseDoor(string door)
        {
            return door switch
            {
                "north" => "south",
                "south" => "north",
                "east" => "west",
                "west" => "east",
                _ => string.Empty
            };
        }

        private bool GoToSecurityCheckpoint()
        {
            if (_securityCheckpointPath.Count == 0 || _sensorDirection is null)
            {
                return false;
            }
            foreach (string dir in _securityCheckpointPath)
            {
                Move(dir);
                _currentPath.Push(dir);
            }
            return CurrentLocation == "Security Checkpoint";
        }

        private void Move(string door)
        {
            InputCommand(door);
            while (_program.Tick()) { }
            UpdateLocation();
        }

        private void TakeItem(string item)
        {
            InputCommand($"take {item}");
            Inventory.Add(item);
            while (_program.Tick()) { }
            _program.ClearOutput();
        }

        private void DropItem(string item)
        {
            InputCommand($"drop {item}");
            Inventory.Remove(item);
            while (_program.Tick()) { }
            _program.ClearOutput();
        }

        private bool TryPassSecuritySensor(out string target)
        {
            target = string.Empty;
            if (_sensorDirection is null)
            {
                return false;
            }
            InputCommand(_sensorDirection);
            while (_program.Tick()) { }
            string[] output = ReadOutput();
            if (output.Length == 0)
            {
                throw new InvalidOperationException();
            }
            foreach (string line in output)
            {
                Match failureMatch = _sensorFailure.Match(line);
                if (failureMatch.Success)
                {
                    target = failureMatch.Groups["targetWeight"].Value;
                    return false;
                }
            }
            foreach (string line in output)
            {
                Match successMatch = _sensorSuccess.Match(line);
                if (successMatch.Success)
                {
                    CurrentLocation = output[3][3..^3];
                    DoorsHere.Clear();
                    DoorsHere.Add(ReverseDoor(_sensorDirection));
                    Password = successMatch.Groups["password"].Value;
                    return true;
                }
            }
            return false;
        }
    }

    private class Combinations
    {
        public int Range { get; private set; }
        public int[] Combination { get; private set; }

        private bool _first = false;

        public Combinations(int range, int size = -1)
        {
            Range = range;
            if (size > 0)
            {
                Combination = new int[size];
                for (int i = 0; i < size; i++)
                {
                    Combination[i] = i;
                }
            }
            else
            {
                Combination = Array.Empty<int>();
            }
        }

        public void Reset(int size)
        {
            Combination = new int[size];
            for (int i = 0; i < size; i++)
            {
                Combination[i] = i;
            }
            _first = false;
        }

        public bool Next()
        {
            if (!_first)
            {
                _first = true;
                return true;
            }
            if (Combination[0] + Combination.Length >= Range)
            {
                return false;
            }
            for (int i = Combination.Length - 1; i >= 0; i--)
            {
                if (Combination[i] < Range - 1)
                {
                    if (i < Combination.Length - 1 && Combination[i] + 1 >= Combination[i + 1])
                    {
                        continue;
                    }
                    Combination[i]++;
                    for (int j = i + 1; j < Combination.Length; j++)
                    {
                        Combination[j] = Combination[i] + j - i;
                    }
                    break;
                }
            }
            return true;
        }
    }
}
