using System.CommandLine;
using System.Diagnostics;
using System.Globalization;
using CsvHelper;
using GraphAlgorithms.Clique;
using GraphAlgorithms.Graphs;

namespace GraphAlgorithms.Commands.Benchmark;

public static class BenchmarkCommand
{
    public static Command Create()
    {
        var command = new Command("benchmark", "Compare the speed of various algorithms. The result is saved in a CSV file.");
        
        var maxSizeOption = new Option<int>(name: "--max-size",
                description: "Maximum size of the graph.")
            { IsRequired = true };
        maxSizeOption.AddAlias("-s");
        maxSizeOption.SetDefaultValue(50);
        command.AddOption(maxSizeOption);
        
        command.SetHandler(Run, maxSizeOption);

        return command;
    }

    private static void Run(int maxSize)
    {
        const int repetitions = 10;
        var results = new List<PerformanceComparison>();
        
        Stopwatch stopwatch = new Stopwatch();
        var heuFinder = new MaxCliqueHeuFinder();
        var exactFinder = new MaxCliqueExactFinder();
        
        for (int i = 1; i < maxSize; i++)
        {
            long heuVertexTime = 0;
            long heuVertexEdgeTime = 0;
            long exactVertexTime = 0;
            long exactVertexEdgeTime = 0;
            long? naiveTime = null;
            
            for (int j = 0; j < repetitions; j++)
            {
                int edges = (int)(i * (i - 1) * 0.9 * (j + 1) / repetitions); // 0.9 so that the creation is always quick
                var graph = GraphFactory.CreateRandom(i, edges);
                
                stopwatch.Restart();
                heuFinder.Find(graph);
                stopwatch.Stop();
                heuVertexTime += stopwatch.ElapsedTicks;
                
                stopwatch.Restart();
                heuFinder.FindWithEdges(graph);
                stopwatch.Stop();
                heuVertexEdgeTime += stopwatch.ElapsedTicks;
                
                stopwatch.Restart();
                exactFinder.Find(graph);
                stopwatch.Stop();
                exactVertexTime += stopwatch.ElapsedTicks;
                
                stopwatch.Restart();
                exactFinder.FindWithEdges(graph);
                stopwatch.Stop();
                exactVertexEdgeTime += stopwatch.ElapsedTicks;

                if (i < 15)
                {
                    if (naiveTime == null) naiveTime = 0;
                    stopwatch.Restart();
                    MaxCliqueNaiveFinder.Find(graph);
                    stopwatch.Stop();
                    naiveTime += stopwatch.ElapsedTicks;
                }
            }
            
            results.Add(new PerformanceComparison(i, heuVertexTime, heuVertexEdgeTime, exactVertexTime, exactVertexEdgeTime, naiveTime));
        }

        using var writer = new StreamWriter($"../../../../results.csv");
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(results);
    }
}