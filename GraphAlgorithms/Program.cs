using System.CommandLine;
using GraphAlgorithms.Commands;

internal class Program
{
    private static void Main(string[] args)
    {
        var rootCommand = CreateRootCommand();

        rootCommand.Invoke(args);
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

        return rootCommand;
    }
}