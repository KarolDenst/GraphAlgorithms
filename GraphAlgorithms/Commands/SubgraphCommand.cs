using System.CommandLine;
using GraphAlgorithms.Clique;
using GraphAlgorithms.MCS;

namespace GraphAlgorithms.Commands;

public static class SubgraphCommand
{
    public static Command Create()
    {
        var fileOption = OptionsFactory.CreateFileOption();
        var file2Option = OptionsFactory.CreateFile2Option();
        var index1Option = OptionsFactory.CreateIndex1Option();
        var index2Option = OptionsFactory.CreateIndex2Option();
        var algorithmTypeOption = OptionsFactory.CreateAlgorithmTypeOption();
        var cmpOption = OptionsFactory.CreateCmpOption();

        var command = new Command("mcs", "Find maximum common subgraph.")
        {
            fileOption,
            file2Option,
            index1Option,
            index2Option,
            algorithmTypeOption,
            cmpOption
        };
        command.SetHandler(Run, fileOption, file2Option, index1Option, index2Option, algorithmTypeOption, cmpOption);

        return command;
    }

    private static void Run(string path1, string? path2, int index1, int index2, string algType, string comparison)
    {
        if (path2 is null)
            RunMcsFinder(path1, path1, index1, index2, algType, comparison);
        else
            RunMcsFinder(path1, path2, index1, index2, algType, comparison);
    }

    private static void RunMcsFinder(string path1, string path2, int index1, int index2, string algType, string comparison)
    {
        var graph1 = Utils.GetGraph(path1, index1);
        var graph2 = Utils.GetGraph(path2, index2);

        ICliqueFastFinder finder = GetFinder(algType);
        bool withEdges = comparison == "vertices-then-edges";

        (int[] subgraph1, int[] subgraph2) = MCSFinder.FindFast(graph1, graph2, finder, withEdges);

        Console.WriteLine("MCS in graph 1: " + string.Join(", ", subgraph1));
        Console.WriteLine("MCS in graph 2: " + string.Join(", ", subgraph2));
        Console.WriteLine($"{subgraph1.Length} vertices");
        
        if (graph1.Size <= 15 && graph2.Size <= 15)
        {
            Console.WriteLine();
            Console.WriteLine("Graph 1:");
            graph1.Print(subgraph1);
            Console.WriteLine("Graph 2:");
            graph2.Print(subgraph2);
        }
    }

    private static ICliqueFastFinder GetFinder(string algType) => algType switch
    {
        "heuristic" => new MaxCliqueHeuFinder(),
        "exact" => new MaxCliqueExactFinder(),
        _ => throw new NotImplementedException()
    };
}