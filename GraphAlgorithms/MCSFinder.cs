namespace GraphAlgorithms
{
    public class MCSFinder
    {
        public static (int[], int[]) Find(Graph graph1, Graph graph2, Func<Graph, List<int>> maxCliqueFinder)
        {
            Graph modularProduct = ModularProduct.Calculate(graph1, graph2);
            List<int> maxClique = maxCliqueFinder(modularProduct);
            int[] vertices1 = maxClique.Select(p => p / graph2.Size).ToArray();
            int[] vertices2 = maxClique.Select(p => p % graph2.Size).ToArray();
            return (vertices1, vertices2);
        }
    }
}
