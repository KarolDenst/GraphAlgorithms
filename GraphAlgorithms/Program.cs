using System.Diagnostics;
using GraphAlgorithms;
using GraphAlgorithms.Clique;

// example from here https://arxiv.org/pdf/1908.06418.pdf
int size = 20;
Random random = new Random(0);
Stopwatch exactStopwatch = new Stopwatch();
Stopwatch approxStopwatch = new Stopwatch();
for (int i = 0; i < 100; i++)
{
    var density = random.NextDouble();
    Graph graph = GraphFactory.CreateRandom(size, (int)(size * (size - 1) / 2 * density), i);
    exactStopwatch.Start();
    var maxClique = MaxCliqueNaiveFinder.Find(graph);
    exactStopwatch.Stop();
    approxStopwatch.Start();
    var maxCliqueFinder = new MaxCliqueHeuristicFinder(graph);
    var approxClique = maxCliqueFinder.Find();
    approxStopwatch.Stop();
    Console.WriteLine($"{i}) Max: {maxClique.Count}, Approx: {approxClique.Count}");
    Console.WriteLine($"Exact found: {String.Join(", ", maxClique)}");
    Console.WriteLine($"Approx found: {String.Join(", ", approxClique)}");
    if (maxClique.Count != approxClique.Count)
    {
        Console.WriteLine($"Warning. The graph sizes are different");
    }
    
    if (Math.Abs(maxClique.Count - approxClique.Count) > 1)
    {
        Console.WriteLine($"Warning!!!!! The graph sizes are very different!");
        Console.WriteLine(graph.ToString());
    }
}

Console.WriteLine($"Exact time: {exactStopwatch.ElapsedMilliseconds} ms");
Console.WriteLine($"Approx time: {approxStopwatch.ElapsedMilliseconds} ms");
