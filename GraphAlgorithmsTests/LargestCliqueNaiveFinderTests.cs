using GraphAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace GraphAlgorithms.Tests
{
    [TestFixture]
    public class LargestCliqueNaiveFinderTests
    {
        private static IEnumerable<TestCaseData> TestCases()
        {
            Graph graph;
            // graphs with no edges
            graph = new Graph(1);
            yield return new TestCaseData(graph, new List<int> { 0 });

            graph = new Graph(5);
            yield return new TestCaseData(graph, new List<int> { 0 });

            graph = new Graph(10);
            yield return new TestCaseData(graph, new List<int> { 0 });

            // graphs that are clique
            graph = new Graph(2);
            graph.AddEdge(0, 1);
            graph.AddEdge(1, 0);
            yield return new TestCaseData(graph, new List<int> { 0, 1 });

            graph = new Graph(5);
            graph.AdjacencyMatrix = new int[][] {
                new int[] { 0, 1, 1, 1, 1 },
                new int[] { 1, 0, 1, 1, 1 },
                new int[] { 1, 1, 0, 1, 1 },
                new int[] { 1, 1, 1, 0, 1 },
                new int[] { 1, 1, 1, 1, 0 },
            };
            yield return new TestCaseData(graph, new List<int> { 0, 1, 2, 3, 4 });

            graph = new Graph(10);
            graph.AdjacencyMatrix = new int[][] {
                new int[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                new int[] { 1, 0, 1, 1, 1, 1, 1, 1, 1, 1 },
                new int[] { 1, 1, 0, 1, 1, 1, 1, 1, 1, 1 },
                new int[] { 1, 1, 1, 0, 1, 1, 1, 1, 1, 1 },
                new int[] { 1, 1, 1, 1, 0, 1, 1, 1, 1, 1 },
                new int[] { 1, 1, 1, 1, 1, 0, 1, 1, 1, 1 },
                new int[] { 1, 1, 1, 1, 1, 1, 0, 1, 1, 1 },
                new int[] { 1, 1, 1, 1, 1, 1, 1, 0, 1, 1 },
                new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 0, 1 },
                new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 }
            };
            yield return new TestCaseData(graph, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });

            // graph with max clique of size 1
            graph = new Graph(5);
            yield return new TestCaseData(graph, new List<int> { 0 });

            // graph with many cliques of size 2
            graph = new Graph(8);
            for(int i = 0; i < 8; i += 2)
            {
                graph.AddEdge(i, i + 1);
                graph.AddEdge(i + 1, i);
            }
            yield return new TestCaseData(graph, new List<int> { 0, 1 });

            // graphs with edges that not belong to any cliques
            graph = new Graph(10);
            graph.AdjacencyMatrix = new int[][] {
                new int[] { 0, 1, 0, 0, 1, 0, 0, 1, 0, 0 },
                new int[] { 1, 0, 0, 0, 1, 0, 1, 1, 0, 0 },
                new int[] { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                new int[] { 1, 1, 0, 0, 0, 0, 1, 1, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 1, 0, 1, 1, 0, 0, 0, 1, 0 },
                new int[] { 1, 1, 0, 0, 1, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 1, 1, 0, 0, 0 },
                new int[] { 1, 0, 0, 1, 0, 0, 0, 0, 1, 0 }
            };
            yield return new TestCaseData(graph, new List<int> { 0, 1, 4, 7 });

            graph = new Graph(10);
            graph.AdjacencyMatrix = new int[][] {
                new int[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                new int[] { 1, 0, 1, 1, 1, 1, 1, 1, 1, 1 },
                new int[] { 1, 1, 0, 1, 1, 1, 1, 1, 1, 1 },
                new int[] { 1, 1, 1, 0, 1, 1, 1, 1, 1, 1 },
                new int[] { 1, 1, 1, 1, 0, 0, 1, 1, 1, 1 },
                new int[] { 1, 1, 1, 1, 1, 0, 1, 1, 1, 1 },
                new int[] { 1, 1, 0, 1, 1, 1, 0, 1, 1, 1 },
                new int[] { 1, 1, 1, 1, 1, 1, 1, 0, 1, 1 },
                new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 0, 1 },
                new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 }
            };
            yield return new TestCaseData(graph, new List<int> { 0, 1, 2, 3, 4, 7, 8, 9});


            graph = new Graph(10);
            graph.AdjacencyMatrix = new int[][] {
                new int[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                new int[] { 1, 0, 1, 1, 1, 1, 1, 1, 1, 1 },
                new int[] { 1, 1, 0, 1, 1, 1, 1, 1, 1, 1 },
                new int[] { 1, 1, 1, 0, 1, 1, 1, 1, 1, 1 },
                new int[] { 1, 1, 1, 1, 0, 1, 1, 1, 1, 1 },
                new int[] { 1, 1, 1, 1, 1, 0, 1, 1, 1, 1 },
                new int[] { 1, 1, 1, 1, 1, 1, 0, 1, 1, 1 },
                new int[] { 1, 1, 1, 1, 1, 1, 1, 0, 1, 1 },
                new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 0, 1 },
                new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 }
            };
            yield return new TestCaseData(graph, Enumerable.Range(0, 9).ToList());

            // many equal cliques
            graph = new Graph(6);
            graph.AdjacencyMatrix = new int[][] {
                new int[] { 0, 1, 1, 0, 1, 1 },
                new int[] { 1, 0, 1, 1, 0, 1 },
                new int[] { 1, 1, 0, 1, 1, 0 },
                new int[] { 0, 1, 1, 0, 1, 1 },
                new int[] { 1, 0, 1, 1, 0, 1 },
                new int[] { 1, 1, 0, 1, 1, 0 },
            };
            yield return new TestCaseData(graph, new List<int> { 0, 1, 2 });

            //
            // DEF: MAX_CLIQUE is number of vertices from largest clique
            // DEF: deg = deg + 1 when there are edges in both sides between vertices
            // graphs with vertices 'v' that deg(v) >= MAX_CLIQUE, but don't expand the biggest clique
            //

            // 1) v = 3: 3 = deg(v) == MAX_CLIQUE = 3
            graph = new Graph(5);
            graph.AdjacencyMatrix = new int[][] {
                new int[] { 0, 1, 1, 1, 0 },
                new int[] { 1, 0, 1, 1, 0 },
                new int[] { 1, 1, 0, 1, 0 },
                new int[] { 1, 1, 0, 0, 1 },
                new int[] { 0, 0, 0, 1, 0 },
            };
            yield return new TestCaseData(graph, new List<int> { 0, 1, 2 });

            // 2) v = 3 : 4 = deg(v) > MAX_CLIQUE = 3
            graph = new Graph(6);
            graph.AdjacencyMatrix = new int[][] {
                new int[] { 0, 1, 1, 1, 0, 0 },
                new int[] { 1, 0, 1, 0, 0, 0 },
                new int[] { 1, 1, 0, 1, 0, 0 },
                new int[] { 1, 0, 1, 0, 1, 1 },
                new int[] { 0, 0, 0, 1, 0, 0 },
                new int[] { 0, 0, 0, 1, 0, 0 },
            };
            yield return new TestCaseData(graph, new List<int> { 0, 1, 2 });

            // some random graphs, graphs in which vertices 'v' that don't belong to clique have
            // deg(v) < MAX_CLIQUE
            var random = new Random();
            int maxSize = 25;
            for (int i = 0; i < 100; ++i)
            {
                graph = new Graph(random.Next(1, maxSize));
                int cliqueSize = random.Next(1, graph.Size);
                var clique = GenerateClique(graph, cliqueSize);
                GenerateAdditionalEdges(graph, clique);
                yield return new TestCaseData(graph, clique);
            }
        }

        private static List<int> GenerateClique(Graph graph, int cliqueSize)
        {
            if (cliqueSize == 1)
                return new List<int>() { 0 };
            Random random = new Random();
            List<int> clique = new List<int>();
            while (clique.Count < cliqueSize)
            {
                int u = random.Next(graph.Size);
                if (clique.Contains(u))
                    continue;
                foreach (var v in clique)
                    graph.AddBothSidesEdge(u, v);
                clique.Add(u);
            }
            return clique;
        }

        private static void GenerateAdditionalEdges(Graph graph, List<int> clique)
        {
            Random random = new Random();
            int add = random.Next(graph.Size);
            for (int i = 0; i < add; ++i)
            {
                int u = random.Next(graph.Size);
                int v = random.Next(graph.Size);
                if (graph.GetDegree(u) < clique.Count - 1 && graph.GetDegree(v) < clique.Count - 1)
                    graph.AddEdge(u, v);
            }
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void FindTest(Graph graph, List<int> expectedResult)
        {
            List<int> result = LargestCliqueNaiveFinder.Find(graph);
            Assert.That(result, Is.EquivalentTo(expectedResult));
        }
    }
}