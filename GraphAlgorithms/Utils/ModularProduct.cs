using GraphAlgorithms.Graphs;

namespace GraphAlgorithms.Utils
{
    public static class ModularProduct
    {
        /// <summary>
        /// Calculates the modular product of two graphs.
        /// A two-way edge exists between (u1, v1) and (u2, v2) if there's equal number and orientation of edges joining u1 with u2 in <paramref name="graph1"/> and edges joining v1 with v2 in <paramref name="graph2"/>.
        /// </summary>
        /// <param name="graph1"></param>
        /// <param name="graph2"></param>
        /// <returns>Modular product of two graphs, where vertex index p corresponds to (p / |V(graph2)|, p % |V(graph2)|) in <paramref name="graph1"/> x <paramref name="graph2"/></returns>
        public static Graph Calculate(Graph graph1, Graph graph2)
        {
            int m = graph1.Size;
            int n = graph2.Size;
            Graph product = new(m * n);
            for (int i = 0; i < m * n; i++)
            {
                for (int j = i; j < m * n; j++)
                {
                    int u1 = i / n;
                    int v1 = i % n;

                    int u2 = j / n;
                    int v2 = j % n;

                    if (u1 == u2 || v1 == v2)
                        continue;
                    
                    if (graph1.AdjacencyMatrix[u1, u2] == 0)
                    {
                        if (graph2.AdjacencyMatrix[v1, v2] == 0)
                            product.AdjacencyMatrix[i, j] = 1;
                    }
                    else if (graph2.AdjacencyMatrix[v1, v2] != 0)
                    {
                        product.AdjacencyMatrix[i, j] = 2 * Math.Min(graph1.AdjacencyMatrix[u1, u2], graph2.AdjacencyMatrix[v1, v2]);
                    }

                    if (graph1.AdjacencyMatrix[u2, u1] == 0)
                    {
                        if (graph2.AdjacencyMatrix[v2, v1] == 0)
                            product.AdjacencyMatrix[i, j] = 1;
                    }
                    else if (graph2.AdjacencyMatrix[v2, v1] != 0)
                    {
                        product.AdjacencyMatrix[j, i] = 2 * Math.Min(graph1.AdjacencyMatrix[u2, u1], graph2.AdjacencyMatrix[v2, v1]);
                    }
                }
            }

            return product;
        }
    }
}
