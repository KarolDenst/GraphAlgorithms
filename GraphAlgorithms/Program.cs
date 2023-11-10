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
        var rootCommand = new RootCommand("Collection of graph algorithms");

        var dimacsCommand = DimacsCommand.Create();
        rootCommand.Add(dimacsCommand);

        var cliqueCommand = CliqueCommand.Create();
        rootCommand.Add(cliqueCommand);

        var subgraphCommand = SubgraphCommand.Create();
        rootCommand.Add(subgraphCommand);

        return rootCommand;
    }
}