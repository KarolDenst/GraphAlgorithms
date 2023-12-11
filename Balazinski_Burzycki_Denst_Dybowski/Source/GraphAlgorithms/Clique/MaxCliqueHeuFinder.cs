using GraphAlgorithms.Comparers.VertexNumber;
using GraphAlgorithms.Comparers.VertexThenEdge;
using GraphAlgorithms.Graphs;

namespace GraphAlgorithms.Clique
{
    public class MaxCliqueHeuFinder : ICliqueFastFinder
    {
        public List<int> FindWithEdges(Graph graph)
        {
            var sizeComparer = new VertexThenEdgeComparer(graph);
            var finder = new MaxCliqueFinder<VertexThenEdgeSize>(graph, sizeComparer);
            return finder.FindHeuristic();
        }

        public List<int> Find(Graph graph)
        {
            var sizeComparer = new VertexNumberComparer();
            var finder = new MaxCliqueFinder<VertexNumberSize>(graph, sizeComparer);
            return finder.FindHeuristic();
        }
    }
}
