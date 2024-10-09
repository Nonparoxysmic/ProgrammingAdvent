// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using ProgrammingAdvent2021.Utilities;

namespace ProgrammingAdvent2021.Solutions;

internal class Day23 : Day
{
    private static readonly int[] _energyCosts = [0, 1, 10, 100, 1000];

    protected override (string, string) CalculateAnswers(string[] input)
    {
        if (input.Length < 5 || input[2].Length < 13 || input[3].Length < 11)
        {
            return ("Invalid input", "n/a");
        }
        State.ClearStates();
        State initialState = new(input);
        ulong goal = initialState.GoalID();
        int partOneAnswer = LeastEnergyToGoal(initialState, goal);
        State.ClearStates();
        State fullInitialState = new(input, true);
        ulong fullGoal = fullInitialState.GoalID();
        int partTwoAnswer = LeastEnergyToGoal(fullInitialState, fullGoal);
        return ($"{partOneAnswer}", $"{partTwoAnswer}");
    }

    private static int LeastEnergyToGoal(State initialState, ulong goal)
    {
        MinHeap openStates = new();
        openStates.Insert(initialState);
        while (openStates.Count > 0)
        {
            State current = openStates.FindMin();
            if (current.ID == goal)
            {
                return current.EnergyToReach;
            }
            openStates.DeleteMin();
            foreach ((ulong ID, int cost) in current.PossibleNextStates())
            {
                State neighbor = State.GetState(ID);
                int costToReachNeighbor = current.EnergyToReach + cost;
                if (costToReachNeighbor < neighbor.EnergyToReach)
                {
                    neighbor.EnergyToReach = costToReachNeighbor;
                    if (neighbor.Open)
                    {
                        openStates.KeyDecreased(neighbor);
                    }
                    else
                    {
                        openStates.Insert(neighbor);
                    }
                }
            }
        }
        return -1;
    }

    //public static void PrintState(ulong id)
    //{
    //    int part = (int)(id % 5);
    //    id /= 5;
    //    int[] state = part == 1 ? new int[15] : new int[23];
    //    for (int i = state.Length - 1; i >= 0; i--)
    //    {
    //        state[i] = (int)(id % 5);
    //        id /= 5;
    //    }
    //    Console.WriteLine("#############");
    //    Console.WriteLine($"#{state[0]}{state[1]}.{state[2]}.{state[3]}.{state[4]}.{state[5]}{state[6]}#".Replace('0', '.'));
    //    Console.WriteLine($"###{state[7]}#{state[8]}#{state[9]}#{state[10]}###".Replace('0', '.'));
    //    for (int i = 11; i < state.Length - 3; i += 4)
    //    {
    //        Console.WriteLine($"  #{state[i]}#{state[i + 1]}#{state[i + 2]}#{state[i + 3]}#".Replace('0', '.'));
    //    }
    //    Console.WriteLine("  #########");
    //}

    private class State
    {
        private static readonly Dictionary<ulong, State> _allStates = [];

        public ulong ID { get; }
        public bool Open { get; set; }
        public int EnergyToReach { get; set; } = int.MaxValue;
        public int EnergyToGoal { get; }
        public int Score => EnergyToReach + EnergyToGoal;

        private readonly int[] _state;
        private readonly bool[] _canMove;
        private readonly bool[] _canEnterRoom;

        public State(string[] input, bool partTwo = false) : this(InputToState(input, partTwo))
        {
            EnergyToReach = 0;
        }

        public State(int[] state)
        {
            _state = new int[state.Length];
            Array.Copy(state, _state, state.Length);

            ID = Encode(_state);

            if (_allStates.TryAdd(ID, this))
            {
                _canMove = new bool[_state.Length];
                for (int i = 0; i < 7; i++)
                {
                    _canMove[i] = _state[i] > 0;
                }
                bool lockA = true, lockB = true, lockC = true, lockD = true;
                for (int i = _state.Length - 4; i >= 7; i -= 4)
                {
                    _canMove[i + 0] = !lockA || (_state[i + 0] != 1);
                    _canMove[i + 0] &= _state[i + 0] != 0;
                    lockA &= _state[i + 0] == 1;
                    _canMove[i + 1] = !lockB || (_state[i + 1] != 2);
                    _canMove[i + 1] &= _state[i + 1] != 0;
                    lockB &= _state[i + 1] == 2;
                    _canMove[i + 2] = !lockC || (_state[i + 2] != 3);
                    _canMove[i + 2] &= _state[i + 2] != 0;
                    lockC &= _state[i + 2] == 3;
                    _canMove[i + 3] = !lockD || (_state[i + 3] != 4);
                    _canMove[i + 3] &= _state[i + 3] != 0;
                    lockD &= _state[i + 3] == 4;
                }

                _canEnterRoom = Enumerable.Repeat(true, 5).ToArray();
                for (int i = _state.Length - 4; i >= 7; i -= 4)
                {
                    _canEnterRoom[1] &= (_state[i + 0] == 1 || _state[i + 0] == 0);
                    _canEnterRoom[2] &= (_state[i + 1] == 2 || _state[i + 1] == 0);
                    _canEnterRoom[3] &= (_state[i + 2] == 3 || _state[i + 2] == 0);
                    _canEnterRoom[4] &= (_state[i + 3] == 4 || _state[i + 3] == 0);
                }

                EnergyToGoal = 0;
                int[] indices = [0, _state.Length - 4, _state.Length - 3, _state.Length - 2, _state.Length - 1];
                for (int room = 1; room <= 4; room++)
                {
                    for (int i = _state.Length - 5 + room; i >= 7; i -= 4)
                    {
                        if (_state[i] == room)
                        {
                            indices[room] -= 4;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                for (int i = _state.Length - 4; i >= 7; i -= 4)
                {
                    // Room 1
                    if (_canMove[i])
                    {
                        switch (_state[i])
                        {
                            case 2:
                            case 3:
                            case 4:
                                EnergyToGoal += _energyCosts[_state[i]] *
                                    (2 * (_state[i] - 1) + ((i - 3) / 4) + ((indices[_state[i]] - 3) / 4));
                                indices[_state[i]] -= 4;
                                break;
                            default:
                                break;
                        }
                    }
                }
                for (int i = _state.Length - 3; i >= 7; i -= 4)
                {
                    // Room 2
                    if (_canMove[i])
                    {
                        switch (_state[i])
                        {
                            case 1:
                            case 3:
                            case 4:
                                EnergyToGoal += _energyCosts[_state[i]] *
                                    (2 * Math.Abs(_state[i] - 2) + ((i - 3) / 4) + ((indices[_state[i]] - 3) / 4));
                                indices[_state[i]] -= 4;
                                break;
                            default:
                                break;
                        }
                    }
                }
                for (int i = _state.Length - 2; i >= 7; i -= 4)
                {
                    // Room 3
                    if (_canMove[i])
                    {
                        switch (_state[i])
                        {
                            case 1:
                            case 2:
                            case 4:
                                EnergyToGoal += _energyCosts[_state[i]] *
                                    (2 * Math.Abs(_state[i] - 3) + ((i - 3) / 4) + ((indices[_state[i]] - 3) / 4));
                                indices[_state[i]] -= 4;
                                break;
                            default:
                                break;
                        }
                    }
                }
                for (int i = _state.Length - 1; i >= 7; i -= 4)
                {
                    // Room 4
                    if (_canMove[i])
                    {
                        switch (_state[i])
                        {
                            case 1:
                            case 2:
                            case 3:
                                EnergyToGoal += _energyCosts[_state[i]] *
                                    (2 * (4 - _state[i]) + ((i - 3) / 4) + ((indices[_state[i]] - 3) / 4));
                                indices[_state[i]] -= 4;
                                break;
                            default:
                                break;
                        }
                    }
                }
                for (int i = 0; i < 7; i++)
                {
                    if (_canMove[i])
                    {
                        EnergyToGoal += Steps(i, indices[_state[i]]) * _energyCosts[_state[i]];
                        indices[_state[i]] -= 4;
                    }
                }
            }
            else
            {
                _canMove = [];
                _canEnterRoom = [];
            }
        }

        private static int[] InputToState(string[] input, bool partTwo)
        {
            int[] state;
            if (partTwo)
            {
                state = new int[23];
                state[7] = input[2][3] - 64;
                state[8] = input[2][5] - 64;
                state[9] = input[2][7] - 64;
                state[10] = input[2][9] - 64;
                
                state[11] = 'D' - 64;
                state[12] = 'C' - 64;
                state[13] = 'B' - 64;
                state[14] = 'A' - 64;
                state[15] = 'D' - 64;
                state[16] = 'B' - 64;
                state[17] = 'A' - 64;
                state[18] = 'C' - 64;

                state[19] = input[3][3] - 64;
                state[20] = input[3][5] - 64;
                state[21] = input[3][7] - 64;
                state[22] = input[3][9] - 64;
            }
            else
            {
                state = new int[15];
                state[7] = input[2][3] - 64;
                state[8] = input[2][5] - 64;
                state[9] = input[2][7] - 64;
                state[10] = input[2][9] - 64;
                state[11] = input[3][3] - 64;
                state[12] = input[3][5] - 64;
                state[13] = input[3][7] - 64;
                state[14] = input[3][9] - 64;
            }
            return state;
        }

        public static void ClearStates() => _allStates.Clear();

        public static State GetState(ulong ID) => _allStates[ID];

        private static ulong Encode(int[] state)
        {
            ulong result = 0;
            foreach (int i in state)
            {
                result *= 5;
                result += (ulong)i;
            }
            result *= 5;
            result += state.Length == 15 ? 1UL : 2UL;
            return result;
        }

        public ulong GoalID()
        {
            int[] goalState = new int[_state.Length];
            for (int i = _state.Length - 4; i >= 7; i -= 4)
            {
                goalState[i + 0] = 1;
                goalState[i + 1] = 2;
                goalState[i + 2] = 3;
                goalState[i + 3] = 4;
            }
            return Encode(goalState);
        }

        public List<(ulong, int)> PossibleNextStates()
        {
            List<(ulong, int)> result = [];
            // Hall to room
            for (int hall = 0; hall < 7; hall++)
            {
                if (_canMove[hall] && _canEnterRoom[_state[hall]] && OpenPath(hall, _state[hall]))
                {
                    int roomPos = NextIndexInRoom(_state[hall]);
                    if (roomPos < 7)
                    {
                        continue;
                    }
                    int cost = Steps(hall, roomPos) * _energyCosts[_state[hall]];
                    _state.Swap(hall, roomPos);
                    result.Add((new State(_state).ID, cost));
                    _state.Swap(hall, roomPos);
                }
            }
            // Room to hall
            for (int i = 1; i <= 4; i++)
            {
                int roomPos = NextIndexInRoom(i) + 4;
                if (roomPos >= _state.Length)
                {
                    continue;
                }
                for (int hall = 0; hall < 7; hall++)
                {
                    if (_canMove[roomPos] && _state[hall] == 0 && OpenPath(hall, i))
                    {
                        int cost = Steps(hall, roomPos) * _energyCosts[_state[roomPos]];
                        _state.Swap(hall, roomPos);
                        result.Add((new State(_state).ID, cost));
                        _state.Swap(hall, roomPos);
                    }
                }
            }
            return result;
        }

        private bool OpenPath(int hallwayIndex, int roomIndex)
        {
            int lastHallwayIndex = AdjacentHallwayIndex(hallwayIndex, roomIndex);
            if (hallwayIndex == lastHallwayIndex)
            {
                return true;
            }
            int sign = Math.Sign(lastHallwayIndex - hallwayIndex);
            int i = hallwayIndex;
            while (true)
            {
                i += sign;
                if (_state[i] != 0)
                {
                    return false;
                }
                if (i == lastHallwayIndex)
                {
                    return true;
                }
            }
        }

        private static int AdjacentHallwayIndex(int hallwayIndex, int roomIndex)
        {
            return roomIndex switch
            {
                1 => hallwayIndex < 2 ? 1 : 2,
                2 => hallwayIndex < 3 ? 2 : 3,
                3 => hallwayIndex < 4 ? 3 : 4,
                _ => hallwayIndex < 5 ? 4 : 5,
            };
        }

        private static int Steps(int hallwayIndex, int roomPos)
        {
            int y = (roomPos - 3) / 4;
            int room = (roomPos - 7) % 4 + 1;
            return hallwayIndex switch
            {
                0 => 2 * room + y,
                1 => 2 * room - 1 + y,
                2 => Math.Abs(2 * room - 3) + y,
                3 => Math.Abs(2 * room - 5) + y,
                4 => Math.Abs(2 * room - 7) + y,
                5 => 9 - 2 * room + y,
                6 => 10 - 2 * room + y,
                _ => -1
            };
        }

        private int NextIndexInRoom(int room)
        {
            for (int i = _state.Length - 5 + room; i >= 7; i -= 4)
            {
                if (_state[i] == 0)
                {
                    return i;
                }
            }
            return room + 2;
        }
    }

    private class MinHeap
    {
        public int Count { get; private set; }

        private readonly List<State> _states = [];

        public void Insert(State state)
        {
            state.Open = true;
            if (Count < _states.Count)
            {
                _states[Count] = state;
            }
            else
            {
                _states.Add(state);
            }
            UpHeap(Count);
            Count++;
        }

        public State FindMin()
        {
            return _states[0];
        }

        public void DeleteMin()
        {
            _states[0].Open = false;
            Count--;
            if (Count != 0)
            {
                _states[0] = _states[Count];
                DownHeap(0);
            }
        }

        public void KeyDecreased(State state)
        {
            for (int i = 0; i < Count; i++)
            {
                if (state.ID == _states[i].ID)
                {
                    UpHeap(i);
                    return;
                }
            }
        }

        private void UpHeap(int index)
        {
            if (index == 0)
            {
                return;
            }
            int parentIndex = (index - 1) / 2;
            if (_states[parentIndex].Score > _states[index].Score)
            {
                _states.Swap(index, parentIndex);
                UpHeap(parentIndex);
            }
        }

        private void DownHeap(int index)
        {
            int leftChildIndex = 2 * index + 1;
            int rightChildIndex = 2 * index + 2;
            int lowest = index;
            if (leftChildIndex < Count && _states[leftChildIndex].Score < _states[lowest].Score)
            {
                lowest = leftChildIndex;
            }
            if (rightChildIndex < Count && _states[rightChildIndex].Score < _states[lowest].Score)
            {
                lowest = rightChildIndex;
            }
            if (lowest != index)
            {
                _states.Swap(index, lowest);
                DownHeap(lowest);
            }
        }
    }
}
