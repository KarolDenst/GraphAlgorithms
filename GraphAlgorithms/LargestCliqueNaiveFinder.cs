using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms
{
    public static class LargestCliqueNaiveFinder
    {
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
                List<int> newClique = new List<int>(clique){ i };
                if (IsClique(graph, newClique) && newClique.Count > largestClique.Count)
                    largestClique = new List<int>(newClique);
                Find(graph, n, newClique, i + 1, ref largestClique);
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
