using System.CommandLine;

namespace GraphAlgorithms.Commands;

public static class GraphCommand
{
    public static Command Create()
    {
        var fileOption = OptionsFactory.CreateFileOption();
        var indexOption = OptionsFactory.CreateIndexOption();

        var command = new Command("graph", "Shows the adjacency matrix of a graph.")
        {
            fileOption,
            indexOption
        };
        command.SetHandler(Run, fileOption, indexOption);

        return command;
    }

    private static void Run(string path, int index)
    {
        var graph = Utils.GetGraph(path, index);

        Console.WriteLine(graph.ToString());
    }
}