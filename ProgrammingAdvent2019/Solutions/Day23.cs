// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day23 : Day
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

        Network network = new(inputLines[0]);
        long partOneAnswer = network.Run();

        return output.WriteAnswers(partOneAnswer, null);
    }

    public class Network
    {
        public Queue<Packet> PacketsToDeliver { get; private set; } = new();

        private readonly NetworkComputer[] _computers;

        public Network(string intcode)
        {
            _computers = new NetworkComputer[50];
            for (int i = 0; i < 50; i++)
            {
                _computers[i] = new NetworkComputer(this, intcode, i);
            }
        }

        public long Run()
        {
            while (true)
            {
                for (int i = 0; i < 50; i++)
                {
                    _computers[i].Process();
                    while (PacketsToDeliver.Any())
                    {
                        Packet packet = PacketsToDeliver.Dequeue();
                        if (packet.Address == 255)
                        {
                            return packet.Y;
                        }
                        if (packet.Address < 0 || packet.Address >= 50)
                        {
                            return 0;
                        }
                        _computers[packet.Address].PacketsToProcess.Enqueue(packet);
                    }
                }
            }
        }
    }

    public class NetworkComputer
    {
        public Queue<Packet> PacketsToProcess { get; private set; } = new();

        private readonly Network _network;
        private readonly Day09.Day09Program _program;

        public NetworkComputer(Network network, string intcode, int address)
        {
            _network = network;
            _program = new(intcode);
            _program.EnqueueInput(address);
        }

        public void Process()
        {
            if (_program.Status == Day09.Day09Program.ProgramStatus.Error ||
                _program.Status == Day09.Day09Program.ProgramStatus.Halted)
            {
                return;
            }
            int timeout = 0;
            while (_program.Tick() && ++timeout < 1000) { }
            if (_program.Status == Day09.Day09Program.ProgramStatus.Waiting)
            {
                if (PacketsToProcess.Any())
                {
                    Packet packet = PacketsToProcess.Dequeue();
                    _program.EnqueueInput(packet.X);
                    _program.EnqueueInput(packet.Y);
                }
                else
                {
                    _program.EnqueueInput(-1);
                }
            }
            while (_program.OutputCount >= 3)
            {
                long address = _program.DequeueOutput();
                long x = _program.DequeueOutput();
                long y = _program.DequeueOutput();
                Packet packet = new(address, x, y);
                _network.PacketsToDeliver.Enqueue(packet);
            }
        }
    }

    public class Packet
    {
        public long Address { get; private set; }
        public long X { get; private set; }
        public long Y { get; private set; }

        public Packet(long address, long x, long y)
        {
            Address = address;
            X = x;
            Y = y;
        }
    }
}
