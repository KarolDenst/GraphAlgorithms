using GraphAlgorithms.Comparers.VertexNumber;
using GraphAlgorithms.Comparers.VertexThenEdge;
using GraphAlgorithms.Graphs;

namespace GraphAlgorithms.Clique
{
    public class MaxCliqueExactFinder : ICliqueFastFinder
    {
        public List<int> FindWithEdges(Graph graph)
        {
            var sizeComparer = new VertexThenEdgeComparer(graph);
            var finder = new MaxCliqueFinder<VertexThenEdgeSize>(graph, sizeComparer);
            return finder.FindExact();
        }

        public List<int> Find(Graph graph)
        {
            var sizeComparer = new VertexNumberComparer();
            var finder = new MaxCliqueFinder<VertexNumberSize>(graph, sizeComparer);
            return finder.FindExact();
        }
    }
}
