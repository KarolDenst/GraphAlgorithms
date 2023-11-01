using GraphAlgorithms.Comparers.VertexThenEdge;
using GraphAlgorithms.Graphs;

namespace GraphAlgorithms.Utils
{
    public class GraphProperties
    {
        public static (int, int) GetSize(Graph graph)
            => GetSubgraphSize(graph, graph.Vertices);

        public static (int, int) GetSubgraphSize(Graph graph, List<int> subgraph)
            => new VertexThenEdgeComparer(graph).GetSize(subgraph);
    }
}
