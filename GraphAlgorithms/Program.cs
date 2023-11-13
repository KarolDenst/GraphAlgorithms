using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using GraphAlgorithms.Commands;
using GraphAlgorithms.Commands.Benchmark;

namespace GraphAlgorithms;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var rootCommand = CreateRootCommand();
        var parser = new CommandLineBuilder(rootCommand)
            .AddMiddleware(async (context, next) =>
            {
                try
                {
                    await next(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    context.ExitCode = 1;
                }
            })
            .UseDefaults()
            .Build();

        await parser.InvokeAsync(args);
    }

    private static Command CreateRootCommand()
    {
        var rootCommand = new RootCommand("Collection of graph algorithms. For information how to use each command, use <command> --help.");

        var dimacsCommand = DimacsCommand.Create();
        rootCommand.Add(dimacsCommand);

        var cliqueCommand = CliqueCommand.Create();
        rootCommand.Add(cliqueCommand);

        var subgraphCommand = SubgraphCommand.Create();
        rootCommand.Add(subgraphCommand);

        var metricCommand = MetricCommand.Create();
        rootCommand.Add(metricCommand);

        var sizeCommand = SizeCommand.Create();
        rootCommand.Add(sizeCommand);
        
        var graphCommand = GraphCommand.Create();
        rootCommand.Add(graphCommand);
        
        var benchmarkCommand = BenchmarkCommand.Create();
        rootCommand.Add(benchmarkCommand);

        return rootCommand;
    }
}