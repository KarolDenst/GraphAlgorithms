using System.CommandLine;
using GraphAlgorithms.Clique;

namespace GraphAlgorithms.Commands;

public static class CliqueCommand
{
    public static Command Create()
    {
        var fileOption = OptionsFactory.CreateFileOption();
        var indexOption = OptionsFactory.CreateIndexOption();
        var algorithmTypeOption = OptionsFactory.CreateAlgorithmTypeOption();
        var cmpOption = OptionsFactory.CreateCmpOption();

        var command = new Command("max-clique", "Find maximum clique in a graph.")
        {
            fileOption,
            indexOption,
            algorithmTypeOption,
            cmpOption
        };
        command.SetHandler(RunCliqueFinder, fileOption, indexOption, algorithmTypeOption, cmpOption);

        return command;
    }

    private static void RunCliqueFinder(string path, int index, string algType, string comparison)
    {
        var graph = Utils.GetGraph(path, index);

        ICliqueFastFinder finder = GetFinder(algType);
        var clique = comparison switch
        {
            "vertices" => finder.Find(graph),
            "vertices-then-edges" => finder.FindWithEdges(graph),
            _ => throw new NotImplementedException()
        };

        Console.WriteLine(string.Join(", ", clique.ToArray()));
        Console.WriteLine($"Number of vertices: {clique.Count}");
        Console.WriteLine();
        Console.WriteLine(graph.ToString(clique));
    }

    private static ICliqueFastFinder GetFinder(string algType) => algType switch
    {
        "heuristic" => new MaxCliqueHeuFinder(),
        "exact" => new MaxCliqueExactFinder(),
        _ => throw new NotImplementedException()
    };
}