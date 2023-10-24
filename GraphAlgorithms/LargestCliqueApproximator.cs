namespace GraphAlgorithms;

public static class LargestCliqueApproximator
{
    public static (List<int> Vertices, int EdgeCount) Find(Graph graph)
    {
        var vertexDegrees = new int[graph.Size];
        for (int i = 0; i < graph.Size; i++)
        {
            vertexDegrees[i] = graph.GetDegree(i);
        }

        var aliveVertices = Enumerable.Range(0, graph.Size).ToList();
        aliveVertices.Sort((u, v) => CompareVertices(u, v, vertexDegrees));
        
        while (aliveVertices.Count > 1)
        {
            if (IsCliqueApproximation(aliveVertices, vertexDegrees)) break;
            var removed = aliveVertices[^1];
            aliveVertices.RemoveAt(aliveVertices.Count - 1);
            foreach (var vertex in aliveVertices)
            {
                vertexDegrees[vertex] -= graph.GetEdge(vertex, removed);
                vertexDegrees[vertex] -= graph.GetEdge(removed, vertex);
            }

            vertexDegrees[removed] = 0;
            aliveVertices.Sort((u, v) => CompareVertices(u, v, vertexDegrees));
        }

        int edgeCount = 0;
        foreach (var vertex in aliveVertices)
        {
            edgeCount += vertexDegrees[vertex];
        }
        return (aliveVertices, edgeCount / 2);
    }

    private static bool IsCliqueApproximation(List<int> aliveVertices, int[] vertexDegrees)
    {
        int counter = 0;
        foreach (var vertex in aliveVertices)
        {
            counter += vertexDegrees[vertex];
        }
        var cliqueCount = aliveVertices.Count * (aliveVertices.Count - 1);
        if (counter / 2.0 * 1.1 > cliqueCount)
        {
            return true;
        }

        return false;
    }

    private static int CompareVertices(int u, int v, int[] vertexDegrees) => vertexDegrees[v] - vertexDegrees[u];
}