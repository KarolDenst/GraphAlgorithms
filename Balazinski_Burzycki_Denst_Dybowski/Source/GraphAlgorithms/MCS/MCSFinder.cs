using GraphAlgorithms.Clique;
using GraphAlgorithms.Graphs;
using GraphAlgorithms.Utils;

namespace GraphAlgorithms.MCS
{
    public class MCSFinder
    {
        public static (int[], int[]) FindNaive(Graph graph1, Graph graph2)
        {
            Graph modularProduct = ModularProduct.Calculate(graph1, graph2);
            List<int> maxClique = MaxCliqueNaiveFinder.Find(modularProduct);
            int[] vertices1 = maxClique.Select(p => p / graph2.Size).ToArray();
            int[] vertices2 = maxClique.Select(p => p % graph2.Size).ToArray();

            return (vertices1, vertices2);
        }

        public static (int[], int[]) FindFast(Graph graph1, Graph graph2, ICliqueFastFinder finder, bool withEdges = true)
        {
            Graph modularProduct = ModularProduct.Calculate(graph1, graph2);
            List<int> maxClique = withEdges
                ? finder.FindWithEdges(modularProduct)
                : finder.Find(modularProduct);
            int[] vertices1 = maxClique.Select(p => p / graph2.Size).ToArray();
            int[] vertices2 = maxClique.Select(p => p % graph2.Size).ToArray();

            return (vertices1, vertices2);
        }
    }
}
