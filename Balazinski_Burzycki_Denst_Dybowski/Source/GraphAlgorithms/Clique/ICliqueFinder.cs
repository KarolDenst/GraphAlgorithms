using GraphAlgorithms.Graphs;

namespace GraphAlgorithms.Clique
{
    public interface ICliqueFastFinder
    {
        List<int> Find(Graph graph);

        List<int> FindWithEdges(Graph graph);
    }
}
