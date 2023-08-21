// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text;
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

        return output.WriteAnswers(null, null);
    }

    private class Droid
    {
        public static string[] DoNotTake = new string[] { "photons", "escape pod", "molten lava", "infinite loop", "giant electromagnet" };

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
    }
}
