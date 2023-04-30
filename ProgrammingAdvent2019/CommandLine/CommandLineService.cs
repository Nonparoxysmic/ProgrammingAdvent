// ProgrammingAdvent2019 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using Microsoft.Extensions.Hosting;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.CommandLine;

internal class CommandLineService : BackgroundService
{
    private readonly IHostApplicationLifetime _applicationLifetime;

    public CommandLineService(IHostApplicationLifetime applicationLifetime)
    {
        _applicationLifetime = applicationLifetime;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            ExecuteCommandLineInterface(stoppingToken);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{ex.GetType()}: {ex.Message}");
            Console.WriteLine("The application will now terminate.");
            Console.ResetColor();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey(true);
        }
        _applicationLifetime.StopApplication();
        return Task.CompletedTask;
    }

    private static void ExecuteCommandLineInterface(CancellationToken stoppingToken)
    {
        ExampleSource.Initialize();
        InputManager.Update();
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.Write("> ");
            string input = Console.ReadLine() ?? string.Empty;
            string[] terms = input.Split(null as char[], StringSplitOptions.RemoveEmptyEntries);
            if (terms.Length == 0)
            {
                continue;
            }
            ICommand? command = null;
            switch (terms[0].ToLower())
            {
                case "exit":
                case "quit":
                case "x":
                case "q":
                    return;
                case "solve":
                case "s":
                    command = new SolveCommand();
                    break;
                case "test":
                case "t":
                    command = new TestCommand();
                    break;
                case "input":
                case "i":
                    command = new InputCommand();
                    break;
                case "help":
                case "?":
                case "\"help\"":
                case "h":
                    command = new HelpCommand();
                    break;
                default:
                    Console.WriteLine("Unrecognized command. Type \"help\" for a list of commands.");
                    break;
            }
            command?.Execute(terms.TakeLast(terms.Length - 1).ToArray());
        }
    }
}
