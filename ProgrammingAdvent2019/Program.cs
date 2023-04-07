// ProgrammingAdvent2019 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProgrammingAdvent2019.Common;

Console.Title = "ProgrammingAdvent2019 by Nonparoxysmic";

IHost host = new HostBuilder()
    .ConfigureServices(services =>
    {
        services.AddHostedService<CommandLineService>();
    })
    .Build();

await host.RunAsync();
