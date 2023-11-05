using GraphAlgorithms.Clique;
using GraphAlgorithms.Graphs;
using GraphAlgorithms.MCS;
using System.CommandLine;
using System.Diagnostics;

internal class Program
{
    private static readonly string datasetDirPath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName, "Datasets");

    private static void Main(string[] args)
    {
        var rootCommand = new RootCommand("Collection of graph algorithms");

        var dimacsCommand = new Command("dimacs", "Use DIMACS benchmark set (https://iridia.ulb.ac.be/~fmascia/maximum_clique/DIMACS-benchmark)");
        var nameOption = new Option<string>(name: "--name",
            description: "Name of the benchmark");
        dimacsCommand.Add(nameOption);
        var algorithmTypeOption = new Option<string>(name: "--type",
            "Type of the algorithm")
            .FromAmong("exact", "heuristic");
        dimacsCommand.Add(algorithmTypeOption);
        dimacsCommand.SetHandler((nameOptionValue, algTypeOptionValue) =>
        {
            if (!TryGetDatasetPath(nameOptionValue, out var pathToDataset))
                return;
            RunTest(pathToDataset!, algTypeOptionValue);
        }, nameOption, algorithmTypeOption);
        rootCommand.Add(dimacsCommand);

        var cliqueCommand = new Command("max-clique", "Find maximum clique");
        var fileOption = new Option<string>(name: "--file",
            description: "Path to file with graphs");
        var indexOption = new Option<int>(name: "--index",
            description: "Index of the graph in the file",
            getDefaultValue: () => 0);
        var cmpOption = new Option<string>(name: "--cmp",
            description: "Graph comparison type")
            .FromAmong("vertices", "vertices-then-edges");
        cliqueCommand.Add(fileOption);
        cliqueCommand.Add(indexOption);
        cliqueCommand.Add(algorithmTypeOption);
        cliqueCommand.Add(cmpOption);
        cliqueCommand.SetHandler(RunCliqueFinder, fileOption, indexOption, algorithmTypeOption, cmpOption);
        rootCommand.Add(cliqueCommand);

        var subgraphCommand = new Command("mcs", "Find maximum common subgraph");
        var index1Option = new Option<int>(name: "--index1",
            description: "Index of the 1st graph in the file",
            getDefaultValue: () => 0);
        var index2Option = new Option<int>(name: "--index2",
            description: "Index of the 2nd graph in the file",
            getDefaultValue: () => 1);
        subgraphCommand.Add(fileOption);
        subgraphCommand.Add(index1Option);
        subgraphCommand.Add(index2Option);
        subgraphCommand.Add(algorithmTypeOption);
        subgraphCommand.Add(cmpOption);
        subgraphCommand.SetHandler(RunMCSFinder, fileOption, index1Option, index2Option, algorithmTypeOption, cmpOption);
        rootCommand.Add(subgraphCommand);

        rootCommand.Invoke(args);
    }

    private static bool TryGetDatasetPath(string name, out string? pathToDataset)
    {
        if (!name.EndsWith(".clq"))
            name += ".clq";

        string path = Path.Combine(datasetDirPath, name);
        if (File.Exists(path))
        {
            pathToDataset = path;
            return true;
        }

        Console.WriteLine($"File {name} not found locally.\n" +
            "Attempting to download from https://code.ulb.ac.be/lab/IRIDIA.");

        using var client = new HttpClient();
        try
        {
            var httpCallTask = client.GetAsync("https://iridia.ulb.ac.be/~fmascia/files/DIMACS/" + name);
            httpCallTask.Wait();
            using HttpResponseMessage response = httpCallTask.Result;
            response.EnsureSuccessStatusCode();
            HttpContent content = response.Content;
            Directory.CreateDirectory(datasetDirPath);
            using var fileStream = new FileStream(path, FileMode.CreateNew);
            content.CopyToAsync(fileStream).Wait();
            pathToDataset = path;
            return true;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Download failed. Aborting.");
            pathToDataset = null;
            return false;
        }
    }

    private static void RunTest(string path, string algType)
    {
        Stopwatch stopwatch = new Stopwatch();
        Graph graph = ClqParser.ParseGraph(path);

        if (algType == "heuristic")
        {
            var heuristicFinder = new MaxCliqueHeuFinder();
            stopwatch.Start();
            var heuristicClique = heuristicFinder.Find(graph);
            stopwatch.Stop();
            long heuristicAlgorithmTime = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Heuristic algorithm found a clique with {heuristicClique.Count} vertices [{heuristicAlgorithmTime} ms]");
        }
        else
        {
            var exactFinder = new MaxCliqueExactFinder();
            stopwatch.Start();
            var exactClique = exactFinder.Find(graph);
            stopwatch.Stop();
            long exactAlgorithmTime = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Exact algorithm found a clique with {exactClique.Count} vertices [{exactAlgorithmTime} ms]");
        }
    }

    private static void RunCliqueFinder(string path, int index, string algType, string comparison)
    {
        Graph[] graphs;
        try
        {
            graphs = GraphLoader.Load(path);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Couldn't load graphs from file {path}");
            Console.WriteLine(e.Message);
            return;
        }

        if (index > graphs.Length || index < 0)
        {
            Console.WriteLine("Index out of bounds.");
            return;
        }

        List<int> clique;
        if (algType == "exact")
        {
            MaxCliqueExactFinder exactFinder = new MaxCliqueExactFinder();
            if (comparison == "vertices")
            {
                clique = exactFinder.Find(graphs[index]);
            }
            else if (comparison == "vertices-then-edges")
            {
                clique = exactFinder.FindWithEdges(graphs[index]);
            }
            else throw new NotImplementedException();
        }
        else if (algType == "heuristic")
        {
            MaxCliqueHeuFinder heuristicFinder = new MaxCliqueHeuFinder();
            if (comparison == "vertices")
            {
                clique = heuristicFinder.Find(graphs[index]);
            }
            else if (comparison == "vertices-then-edges")
            {
                clique = heuristicFinder.FindWithEdges(graphs[index]);
            }
            else throw new NotImplementedException();
        }
        else throw new NotImplementedException();

        Console.WriteLine(string.Join(", ", clique.ToArray())
            + $" ({clique.Count} vertices)");
    }

    private static void RunMCSFinder(string path, int index1, int index2, string algType, string comparison)
    {
        Graph[] graphs;
        try
        {
            graphs = GraphLoader.Load(path);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Couldn't load graphs from file {path}");
            Console.WriteLine(e.Message);
            return;
        }

        if (index1 > graphs.Length || index1 < 0
            || index2 > graphs.Length || index2 < 0)
        {
            Console.WriteLine("Index out of bounds.");
            return;
        }

        int[] subgraph1;
        int[] subgraph2;
        if (algType == "exact")
        {
            if (comparison == "vertices")
            {
                (subgraph1, subgraph2) = MCSFinder.FindFast(graphs[index1], graphs[index2], new MaxCliqueExactFinder());
            }
            else if (comparison == "vertices-then-edges")
            {
                (subgraph1, subgraph2) = MCSFinder.FindFast(graphs[index1], graphs[index2], new MaxCliqueExactFinder(), withEdges: true);
            }
            else throw new NotImplementedException();
        }
        else if (algType == "heuristic")
        {
            if (comparison == "vertices")
            {
                (subgraph1, subgraph2) = MCSFinder.FindFast(graphs[index1], graphs[index2], new MaxCliqueHeuFinder());
            }
            else if (comparison == "vertices-then-edges")
            {
                (subgraph1, subgraph2) = MCSFinder.FindFast(graphs[index1], graphs[index2], new MaxCliqueHeuFinder(), withEdges: true);
            }
            else throw new NotImplementedException();
        }
        else throw new NotImplementedException();

        Console.WriteLine("MCS in graph 1: " + string.Join(", ", subgraph1));
        Console.WriteLine("MCS in graph 2: " + string.Join(", ", subgraph2));
        Console.WriteLine($"{subgraph1.Length} vertices");
    }
}