using GraphAlgorithms.Clique;
using GraphAlgorithms.Graphs;
using System.CommandLine;
using System.Diagnostics;

namespace GraphAlgorithms.Commands;

public static class DimacsCommand
{
    private static readonly string DatasetDirPath = "Datasets";

    public static Command Create()
    {
        var nameOption = OptionsFactory.CreateNameOption();
        var algorithmTypeOption = OptionsFactory.CreateAlgorithmTypeOption();
        var dimacsCommand = new Command("dimacs", "Use DIMACS benchmark set (https://iridia.ulb.ac.be/~fmascia/maximum_clique/DIMACS-benchmark)")
        {
            nameOption,
            algorithmTypeOption
        };
        dimacsCommand.SetHandler((nameOptionValue, algTypeOptionValue) =>
        {
            if (!TryGetDatasetPath(nameOptionValue, out var pathToDataset))
                return;
            RunTest(pathToDataset!, algTypeOptionValue);
        }, nameOption, algorithmTypeOption);

        return dimacsCommand;
    }

    private static bool TryGetDatasetPath(string name, out string? pathToDataset)
    {
        if (!name.EndsWith(".clq"))
            name += ".clq";

        string path = Path.Combine(DatasetDirPath, name);
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
            Directory.CreateDirectory(DatasetDirPath);
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
}