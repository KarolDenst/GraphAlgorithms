namespace GraphAlgorithms
{
    public static class ModularProduct
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph1"></param>
        /// <param name="graph2"></param>
        /// <returns>Modular product of two graphs, where vertex p corresponds to (p / |V(graph2)|, p % |V(graph2)|)</returns>
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

                    if (u1 != u2 && v1 != v2
                        && graph1.AdjacencyMatrix[u1, u2] == graph2.AdjacencyMatrix[v1, v2]
                        && graph1.AdjacencyMatrix[u2, u1] == graph2.AdjacencyMatrix[v2, v1])
                    {
                        product.AdjacencyMatrix[i, j] = 1;
                        product.AdjacencyMatrix[j, i] = 1;
                    }
                }
            }

            return product;
        }
    }
}
