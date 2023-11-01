using GraphAlgorithms.Comparers.VertexNumber;
using GraphAlgorithms.Comparers.VertexThenEdge;
using GraphAlgorithms.Graphs;

namespace GraphAlgorithms.Clique
{
    public class MaxCliqueHeuFinder : ICliqueFastFinder
    {
        public List<int> FindWithEdges(Graph graph)
        {
            var nonHeuristicSizeComparer = new VertexThenEdgeComparer(graph);
            var nonHeuristicClique = MaxCliqueFinder<VertexThenEdgeSize>.FindExact(graph, nonHeuristicSizeComparer);
            return nonHeuristicClique;
        }

        public List<int> Find(Graph graph)
        {
            var nonHeuristicSizeComparer = new VertexNumberComparer();
            var nonHeuristicClique = MaxCliqueFinder<VertexNumberSize>.FindExact(graph, nonHeuristicSizeComparer);
            return nonHeuristicClique;
        }
    }
}
