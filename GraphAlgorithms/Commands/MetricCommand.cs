using System.CommandLine;
using GraphAlgorithms.Clique;
using GraphAlgorithms.MCS;

namespace GraphAlgorithms.Commands;

public static class MetricCommand
{
    public static Command Create()
    {
        var fileOption = OptionsFactory.CreateFileOption();
        var file2Option = OptionsFactory.CreateFile2Option();
        var index1Option = OptionsFactory.CreateIndex1Option();
        var index2Option = OptionsFactory.CreateIndex2Option();
        var algorithmTypeOption = OptionsFactory.CreateAlgorithmTypeOption();

        var command = new Command("metric", "Find the distance between two graphs.")
        {
            fileOption,
            file2Option,
            index1Option,
            index2Option,
            algorithmTypeOption,
        };
        command.SetHandler(Run, fileOption, file2Option, index1Option, index2Option, algorithmTypeOption);

        return command;
    }

    private static void Run(string path1, string? path2, int index1, int index2, string algType)
    {
        if (path2 is null)
            RunMcsFinder(path1, path1, index1, index2, algType);
        else
            RunMcsFinder(path1, path2, index1, index2, algType);
    }

    private static void RunMcsFinder(string path1, string path2, int index1, int index2, string algType)
    {
        var graph1 = Utils.GetGraph(path1, index1);
        var graph2 = Utils.GetGraph(path2, index2);

        ICliqueFastFinder finder = GetFinder(algType);
        bool withEdges = false;

        (int[] subgraph1, int[] subgraph2) = MCSFinder.FindFast(graph1, graph2, finder, withEdges);

        var metric = 1 - (double)subgraph1.Length / Math.Max(graph1.Size, graph2.Size);
        Console.WriteLine($"Metric: {metric}.");
    }

    private static ICliqueFastFinder GetFinder(string algType) => algType switch
    {
        "heuristic" => new MaxCliqueHeuFinder(),
        "exact" => new MaxCliqueExactFinder(),
        _ => throw new NotImplementedException()
    };
}