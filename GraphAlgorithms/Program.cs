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

graph1 = new Graph(2);
graph1.AddEdge(0, 1);
graph2 = new Graph(3);
graph2.AddBothSidesEdge(0, 1);
graph2.AddEdge(0, 2);
var result = ModularProduct.Calculate(graph1, graph2);
Console.WriteLine();