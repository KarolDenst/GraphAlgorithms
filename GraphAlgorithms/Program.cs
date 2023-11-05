using GraphAlgorithms.Clique;
using GraphAlgorithms.Graphs;
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

        var cliqueCommand = new Command("clique", "Find clique");
        rootCommand.Add(cliqueCommand);

        var subgraphCommand = new Command("subgraph", "Find maximum common subgraph");
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

    private static void SomeTest()
    {
        int graphSize = 20;
        int iterations = 100;
        Random random = new Random(0);
        Stopwatch naiveStopwatch = new Stopwatch();
        Stopwatch heuristicStopwatch = new Stopwatch();
        Stopwatch nonHeuristicStopwatch = new Stopwatch();
        for (int i = 0; i < iterations; i++)
        {
            var density = random.NextDouble();
            Graph graph = GraphFactory.CreateRandom(graphSize, (int)(graphSize * (graphSize - 1) * density), i);

            naiveStopwatch.Start();
            var maxClique = MaxCliqueNaiveFinder.Find(graph);
            maxClique.Sort();
            naiveStopwatch.Stop();

            nonHeuristicStopwatch.Start();
            var nonHeuristicFinder = new MaxCliqueExactFinder();
            var nonHeuristicClique = nonHeuristicFinder.FindWithEdges(graph);
            nonHeuristicClique.Sort();
            nonHeuristicStopwatch.Stop();

            heuristicStopwatch.Start();
            var heuristicFinder = new MaxCliqueHeuFinder();
            var heuristicClique = heuristicFinder.FindWithEdges(graph);
            heuristicClique.Sort();
            heuristicStopwatch.Stop();

            Console.WriteLine($"{i}) Naive: {maxClique.Count}, non-Heuristic: {nonHeuristicClique.Count}, Heuristic: {heuristicClique.Count}");
            Console.WriteLine($"Naive found: {string.Join(", ", maxClique)}");
            Console.WriteLine($"non-Heuristic found: {string.Join(", ", nonHeuristicClique)}");
            Console.WriteLine($"Heuristic found: {string.Join(", ", heuristicClique)}");

            if (maxClique.Count != nonHeuristicClique.Count)
            {
                Console.WriteLine("==============================================");
                Console.WriteLine("Exact algorithms have different results");
                Console.WriteLine("==============================================");
            }

            if (maxClique.Count != heuristicClique.Count)
            {
                Console.WriteLine($"Warning. The graph sizes are different");
            }

            if (Math.Abs(maxClique.Count - heuristicClique.Count) > 1)
            {
                Console.WriteLine($"Warning!!!!! The graph sizes are very different!");
                Console.WriteLine(graph.ToString());
            }
        }
        Console.WriteLine("==============================================");
        Console.WriteLine("                Time Comparison               ");
        Console.WriteLine("==============================================");
        Console.WriteLine($"Exact time: {naiveStopwatch.ElapsedMilliseconds} ms");
        Console.WriteLine($"non-Heuristic time: {nonHeuristicStopwatch.ElapsedMilliseconds} ms");
        Console.WriteLine($"Heuristic time: {heuristicStopwatch.ElapsedMilliseconds} ms");
    }
}