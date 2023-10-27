namespace GraphAlgorithms
{
    internal class MaximumCommonSubgraph
    {
        internal class Subgraph
        {
            public Graph Graph;
            public int[] Mapping;

            public Subgraph(Graph graph, int[] mapping)
            {
                Graph = graph;
                Mapping = mapping;
            }
        }

        public static (int[]?, int[]?) Find(Graph graph1, Graph graph2)
        {
            Graph maxCommonSubgraph = new Graph(0);
            int[]? verticesInGraph1 = null;
            int[]? verticesInGraph2 = null;

            var subgraphs1 = GenerateSubgraphs(graph1);
            var subgraphs2 = GenerateSubgraphs(graph2);

            foreach (var subgraph1 in subgraphs1)
            {
                foreach (var subgraph2 in subgraphs2)
                {
                    if (AreIsomorphic(subgraph1.Graph, subgraph2.Graph))
                    {
                        if (maxCommonSubgraph.Size < subgraph1.Graph.Size)
                        {
                            maxCommonSubgraph = subgraph1.Graph;
                            verticesInGraph1 = subgraph1.Mapping;
                            verticesInGraph2 = subgraph2.Mapping;
                        }
                    }
                }
            }

            return (verticesInGraph1, verticesInGraph2);
        }

        private static List<Subgraph> GenerateSubgraphs(Graph graph)
        {
            int n = graph.Size;
            var subgraphs = new List<Subgraph>();
            for (int r = 1; r <= n; r++)
            {
                foreach (var subgraphIndices in GetKCombs(Enumerable.Range(0, n), r))
                {
                    Graph subgraph = new Graph(r);
                    for (int i = 0; i < r; i++)
                    {
                        for (int j = 0; j < r; j++)
                        {
                            subgraph.AdjacencyMatrix[i, j]
                                = graph.AdjacencyMatrix[subgraphIndices[i], subgraphIndices[j]];
                        }
                    }

                    subgraphs.Add(new Subgraph(subgraph, subgraphIndices));
                }
            }

            return subgraphs;
        }

        private static bool AreIsomorphic(Graph graph1, Graph graph2)
        {
            if (graph1.Size != graph2.Size)
                return false;

            int n = graph1.Size;
            var vertices = Enumerable.Range(0, n).ToList();

            foreach (var perm in Permutations(vertices, vertices.Count))
            {
                bool isomorphic = true;

                for (int i = 0; i < n; i++)
                {
                    var v1 = perm[i];
                    for (int j = 0; j < n; j++)
                    {
                        var v2 = perm[j];
                        if (graph1.AdjacencyMatrix[i, j] != graph2.AdjacencyMatrix[v1, v2])
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
