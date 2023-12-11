using System.CommandLine;

namespace GraphAlgorithms.Commands;

public static class SizeCommand
{
    public static Command Create()
    {
        var fileOption = OptionsFactory.CreateFileOption();
        var indexOption = OptionsFactory.CreateIndexOption();

        var command = new Command("size", "Calculate size of a graph.")
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

        Console.WriteLine($"Graph size: ({graph.Size}, {graph.GetNumberOfEdges()})");
    }
}