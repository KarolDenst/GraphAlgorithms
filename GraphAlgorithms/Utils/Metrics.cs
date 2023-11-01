using GraphAlgorithms.Clique;
using GraphAlgorithms.Graphs;
using GraphAlgorithms.MCS;

namespace GraphAlgorithms.Utils
{
    public class Metrics
    {
        public static double CalculateBasedOnMCS(Graph graph1, Graph graph2)
        {
            var subgraphs = MCSFinder.FindFast(graph1, graph2, new MaxCliqueExactFinder());
            return 1 - (double)subgraphs.Item1.Length / Math.Max(graph1.Size, graph2.Size);
        }
    }
}
