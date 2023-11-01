﻿using GraphAlgorithms.Clique;
using GraphAlgorithms.Graphs;
using System.Diagnostics;

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
