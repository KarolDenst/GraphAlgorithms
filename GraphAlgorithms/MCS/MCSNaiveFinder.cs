using GraphAlgorithms.Graphs;

namespace GraphAlgorithms.MCS
{
    public class MCSNaiveFinder
    {
        /// <summary>
        /// Finds a maximum common subgraph of two graphs
        /// </summary>
        /// <param name="graph1"></param>
        /// <param name="graph2"></param>
        /// <returns>A pair of arrays which contain the vertices in the MCS in both graphs</returns>
        public static (int[], int[]) Find(Graph graph1, Graph graph2)
        {
            int[] maxCommonSubgraph = Array.Empty<int>();
            int[] verticesInGraph1 = Array.Empty<int>();
            int[] verticesInGraph2 = Array.Empty<int>();

            var subgraphs1 = GenerateSubgraphs(graph1);
            var subgraphs2 = GenerateSubgraphs(graph2);

            foreach (var subgraph1 in subgraphs1)
            {
                foreach (var subgraph2 in subgraphs2)
                {
                    if (AreSubgraphsIsomorphic(subgraph1, graph1, subgraph2, graph2))
                    {
                        if (maxCommonSubgraph.Length < subgraph1.Length)
                        {
                            maxCommonSubgraph = subgraph1;
                            verticesInGraph1 = subgraph1;
                            verticesInGraph2 = subgraph2;
                        }
                    }
                }
            }

            return (verticesInGraph1, verticesInGraph2);
        }

        private static List<int[]> GenerateSubgraphs(Graph graph)
        {
            int n = graph.Size;
            var subgraphs = new List<int[]>();
            for (int r = 1; r <= n; r++)
            {
                foreach (var subgraphIndices in GetKCombs(Enumerable.Range(0, n), r))
                {
                    subgraphs.Add(subgraphIndices);
                }
            }

            return subgraphs;
        }

        private static bool AreSubgraphsIsomorphic(int[] subgraph1, Graph graph1, int[] subgraph2, Graph graph2)
        {
            if (subgraph1.Length != subgraph2.Length)
                return false;

            int n = subgraph1.Length;
            var vertices = Enumerable.Range(0, n).ToList();

            foreach (var perm in Permutations(vertices, vertices.Count))
            {
                bool isomorphic = true;

                for (int i = 0; i < n; i++)
                {
                    var permutedI = perm[i];
                    for (int j = 0; j < n; j++)
                    {
                        var permutedJ = perm[j];
                        if (graph1.AdjacencyMatrix[subgraph1[i], subgraph1[j]]
                            != graph2.AdjacencyMatrix[subgraph2[permutedI], subgraph2[permutedJ]])
                        {
                            isomorphic = false;
                            break;
                        }
                    }
                    if (!isomorphic)
                        break;
                }

                if (isomorphic)
                    return true;
            }

            return false;
        }

        // all permutations of `values`
        // https://rosettacode.org/wiki/Permutations
        // possibly slow as hell
        static List<T[]> Permutations<T>(List<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t }).ToList();

            return Permutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }).ToArray()).ToList();
        }

        // https://stackoverflow.com/questions/1952153/what-is-the-best-way-to-find-all-combinations-of-items-in-an-array/10629938#10629938
        static List<T[]> GetKCombs<T>(IEnumerable<T> list, int length) where T : IComparable
        {
            if (length == 1) return list.Select(t => new T[] { t }).ToList();
            return GetKCombs(list, length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0),
                    (t1, t2) => t1.Concat(new T[] { t2 }).ToArray()).ToList();
        }
    }
}
