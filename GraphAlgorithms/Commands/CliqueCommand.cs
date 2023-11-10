using System.CommandLine;
using GraphAlgorithms.Clique;
using GraphAlgorithms.Graphs;

namespace GraphAlgorithms.Commands;

public static class CliqueCommand
{
    public static Command Create()
    {
        var fileOption = BasicCommandFactory.CreateFileOption();
        var indexOption = BasicCommandFactory.CreateIndexOption();
        var algorithmTypeOption = BasicCommandFactory.CreateAlgorithmTypeOption();
        var cmpOption = BasicCommandFactory.CreateCmpOption();
        
        var cliqueCommand = new Command("max-clique", "Find maximum clique is a graph.")
        {
            fileOption,
            indexOption,
            algorithmTypeOption,
            cmpOption
        };
        cliqueCommand.SetHandler(RunCliqueFinder, fileOption, indexOption, algorithmTypeOption, cmpOption);

        return cliqueCommand;
    }
    
    private static void RunCliqueFinder(string path, int index, string algType, string comparison)
    {
        var graph = GetGraph(path, index);
        
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

    private static Graph GetGraph(string path, int index)
    {
        var graphs = GraphLoader.Load(path);

        if (index > graphs.Length || index < 0)
        {
            Console.WriteLine("Index out of bounds.");
            throw new IndexOutOfRangeException();
        }

        return graphs[index];
    }
    
    private static ICliqueFastFinder GetFinder(string algType) => algType switch
    {
        "heuristic" => new MaxCliqueHeuFinder(),
        "exact" => new MaxCliqueExactFinder(),
        _ => throw new NotImplementedException()
    };
}