using GraphAlgorithms.Graphs;

namespace GraphAlgorithms.Clique
{
    public static class MaxCliqueNaiveFinder
    {
        /// <summary>
        /// Finds the maximum clique in a graph.
        /// </summary>
        /// <param name="graph"></param>
        /// <returns>List of vertices in the maximum clique</returns>
        public static List<int> Find(Graph graph)
        {
            List<int> largestClique = new();
            Find(graph, graph.Size, new List<int>(), 0, ref largestClique);
            return largestClique;
        }

        private static void Find(Graph graph, int n, List<int> clique, int v, ref List<int> largestClique)
        {
            for (int i = v; i < n; i++)
            {
                clique.Add(i);
                if (IsClique(graph, clique) && clique.Count > largestClique.Count)
                    largestClique = new List<int>(clique);
                Find(graph, n, clique, i + 1, ref largestClique);
                clique.RemoveAt(clique.Count - 1);
            }
        }


        private static bool IsClique(Graph graph, List<int> clique)
        {
            foreach (int u in clique)
            {
                foreach (int v in clique)
                {
                    if (u != v && !graph.AreNeighborsInBothDirections(u, v))
                        return false;
                }
            }
            return true;
        }
    }
}
