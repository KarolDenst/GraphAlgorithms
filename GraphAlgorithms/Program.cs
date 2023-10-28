using GraphAlgorithms;

// example from here https://arxiv.org/pdf/1908.06418.pdf
var graph1 = new Graph(5);
graph1.AdjacencyMatrix = new int[,]
{
    { 0, 0, 0, 1, 1 },
    { 0,0,1,0,1 },
    { 0,1,0,0,1 },
    { 1,0,0,0,0 },
    { 1,1,1,0,0 }
};

var graph2 = new Graph(6);
graph2.AdjacencyMatrix = new int[,]
{
    {0,1,0,0,1,0 },
    {1,0,0,1,0,1 },
    {0,0,0,1,0,1 },
    {0,1,1,0,0,1 },
    {1,0,0,0,0,1 },
    {0,1,1,1,1,0 }
};

PrintResult(graph1, graph2);

graph1 = new Graph(5);
graph1.AdjacencyMatrix = new[,]
{
    {0, 1, 0, 0, 1 },
    {1, 0, 1, 0, 0 },
    {0, 1, 0, 1, 1 },
    {0, 0, 1, 0, 1 },
    {1, 0, 1, 1, 0 }
};
graph2 = new Graph(6);
graph2.AdjacencyMatrix = new[,]
{
    {0, 0, 0, 0, 1, 1 },
    {0, 0, 1, 0, 0, 0 },
    {0, 1, 0, 1, 0, 1 },
    {0, 0, 1, 0, 1, 1 },
    {1, 0, 0, 1, 0, 0 },
    {1, 0, 1, 1, 0, 0 },
};

PrintResult(graph1, graph2);

void PrintResult(Graph graph1, Graph graph2)
{
    var result = MCSNaiveFinder.Find(graph1, graph2);

    Console.WriteLine("Graph 1");
    foreach (var i in result.Item1)
        Console.Write(i + " ");

    Console.WriteLine("\nGraph 2");
    foreach (var i in result.Item2)
        Console.Write(i + " ");
    Console.WriteLine();
}

static List<T[]> GetKCombs<T>(IEnumerable<T> list, int length) where T : IComparable
{
    if (length == 1) return list.Select(t => new T[] { t }).ToList();
    return GetKCombs(list, length - 1)
        .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0),
            (t1, t2) => t1.Concat(new T[] { t2 }).ToArray()).ToList();
}
