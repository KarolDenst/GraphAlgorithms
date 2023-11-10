using System.CommandLine;
using GraphAlgorithms.Clique;
using GraphAlgorithms.Graphs;
using GraphAlgorithms.MCS;

namespace GraphAlgorithms.Commands;

public static class SubgraphCommand
{
    public static Command Create()
    {
        var fileOption = BasicCommandFactory.CreateFileOption();
        var index1Option = BasicCommandFactory.CreateIndexOption();
        var index2Option = BasicCommandFactory.CreateIndexOption();
        var algorithmTypeOption = BasicCommandFactory.CreateAlgorithmTypeOption();
        var cmpOption = BasicCommandFactory.CreateCmpOption();
        
        var subgraphCommand = new Command("mcs", "Find maximum common subgraph.")
        {
            fileOption,
            index1Option,
            index2Option,
            algorithmTypeOption,
            cmpOption
        };
        subgraphCommand.SetHandler(RunMcsFinder, fileOption, index1Option, index2Option, algorithmTypeOption, cmpOption);

        return subgraphCommand;
    }
    
    private static void RunMcsFinder(string path, int index1, int index2, string algType, string comparison)
    {
        var (graph1, graph2) = GetGraphs(path, index1, index2);

        ICliqueFastFinder finder = GetFinder(algType);
        bool withEdges = comparison == "vertices-then-edges";
        
        (int[] subgraph1, int[] subgraph2) = MCSFinder.FindFast(graph1, graph2, finder, withEdges);

        Console.WriteLine("MCS in graph 1: " + string.Join(", ", subgraph1));
        Console.WriteLine("MCS in graph 2: " + string.Join(", ", subgraph2));
        Console.WriteLine($"{subgraph1.Length} vertices");
    }

    private static (Graph, Graph) GetGraphs(string path, int index1, int index2)
    {
        var graphs = GraphLoader.Load(path);

        if (index1 > graphs.Length || index1 < 0
                                   || index2 > graphs.Length || index2 < 0)
        {
            Console.WriteLine("Index out of bounds.");
            throw new IndexOutOfRangeException();
        }
        
        return (graphs[index1], graphs[index2]);
    }

    private static ICliqueFastFinder GetFinder(string algType) => algType switch
    {
        "heuristic" => new MaxCliqueHeuFinder(),
        "exact" => new MaxCliqueExactFinder(),
        _ => throw new NotImplementedException()
    };
}