namespace GraphAlgorithms.Graphs;

public static class GraphFactory
{
    public static Graph CreateEmpty(int size)
    {
        return new Graph(size);
    }

    public static Graph CreateClique(int size)
    {
        Graph graph = new Graph(size);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (i == j) continue;
                graph.SetNumberOfEdges(i, j, 1);
            }
        }
        return graph;
    }

    public static Graph CreateRandom(int size, int numberOfEdges, int seed = 0)
    {
        Random random = new Random(seed);
        Graph graph = new Graph(size);

        int counter = 0;
        while (counter < numberOfEdges)
        {
            int from = random.Next(size);
            int to = random.Next(size);
            if (from == to) continue;
            if (graph.GetNumberOfEdges(from, to) > 0) continue;
            graph.AddEdge(from, to);
            counter++;
        }

        return graph;
    }

    public static Graph CreateRandom(int size, double density, int seed = 0)
    {
        return CreateRandom(size, (int)(size * (size - 1) * density), seed);
    }

    public static Graph CreateRandomWithClique(int size, int numberOfEdges, int cliqueSize, int seed = 0)
    {
        var random = new Random(seed);
        var graph = new Graph(size);

        var indices = new List<int>();
        var indicesHashSet = new HashSet<int>();

        while (indices.Count < cliqueSize)
        {
            int num = random.Next(0, size);
            if (indicesHashSet.Add(num))
            {
                indices.Add(num);
            }
        }

        for (int i = 0; i < cliqueSize; i++)
        {
            for (int j = 0; j < cliqueSize; j++)
            {
                if (i == j) continue;
                graph.SetNumberOfEdges(indices[i], indices[j], 1);
            }
        }

        int counter = cliqueSize;
        while (counter < numberOfEdges)
        {
            int from = random.Next(size);
            int to = random.Next(size);
            if (from == to) continue;
            if (graph.GetNumberOfEdges(from, to) > 0) continue;
            graph.AddEdge(from, to);
            counter++;
        }

        return graph;
    }
}