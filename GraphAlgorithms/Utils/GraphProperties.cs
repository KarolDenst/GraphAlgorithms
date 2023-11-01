using GraphAlgorithms.Graphs;

namespace GraphAlgorithms.Utils
{
    public class GraphProperties
    {
        public static (int, int) GetSize(Graph graph)
        {
            int edges = 0;
            for (int i = 0; i < graph.Size; i++)
            {
                for (int j = 0; j < graph.Size; j++)
                {
                    edges += graph.AdjacencyMatrix[i, j];
                }
            }

            return (graph.Size, edges);
        }
    }
}
