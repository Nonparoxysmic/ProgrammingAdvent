// ProgrammingAdvent2019 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using Microsoft.Extensions.Hosting;
using ProgrammingAdvent2019.Solutions;

namespace ProgrammingAdvent2019.Common;

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
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.Write("> ");
            string input = Console.ReadLine() ?? string.Empty;
            if (input == "exit")
            {
                break;
            }
        }
    }
}
