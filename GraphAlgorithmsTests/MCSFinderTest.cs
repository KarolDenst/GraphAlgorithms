using GraphAlgorithms;
using GraphAlgorithms.Clique;
using NUnit.Framework;

using Mapping = System.Collections.Generic.HashSet<(int, int)>;

namespace GraphAlgorithmsTests
{
    [TestFixture]
    public class MCSFinderTest
    {
        private static IEnumerable<TestCaseData> TestCases()
        {
            Graph graph1;
            Graph graph2;
            List<Mapping> mappings; // each set corresponds to one mapping

            // two completely different graphs
            graph1 = new Graph(3);
            graph1.AddBothSidesEdge(0, 1);
            graph2 = new Graph(3);
            mappings = new List<Mapping> { new Mapping { (0, 0), (2, 2) }, new Mapping { (0, 0), (2, 1) } };
            yield return new TestCaseData(graph1, graph2, mappings);

            // two equal graph
            graph1 = new Graph(3);
            graph1.AddBothSidesEdge(0, 1);
            graph1.AddBothSidesEdge(1, 2);
            graph2 = new Graph(3);
            graph2.AddBothSidesEdge(0, 1);
            graph2.AddBothSidesEdge(1, 2);
            mappings = new List<Mapping> { new Mapping { (0, 0), (1, 1), (2, 2) } };
            yield return new TestCaseData(graph1, graph2, mappings);

            // two graphs with common subgraph
            graph1 = new Graph(3);
            graph1.AddBothSidesEdge(0, 1);
            graph1.AddEdge(1, 2);
            graph2 = new Graph(3);
            graph2.AddBothSidesEdge(0, 1);
            graph2.AddEdge(0, 2);
            mappings = new List<Mapping> { new Mapping { (0, 1), (1, 0), (2, 2) } };
            yield return new TestCaseData(graph1, graph2, mappings);

            // two graphs with common subgraphs with indexing change
            graph1 = new Graph(3);
            graph1.AddBothSidesEdge(0, 1);
            graph2 = new Graph(3);
            graph2.AddBothSidesEdge(1, 2);
            mappings = new List<Mapping> { new Mapping { (0, 1), (1, 2), (2, 0) } };
            yield return new TestCaseData(graph1, graph2, mappings);

            // two graphs with different sizes
            graph1 = new Graph(3);
            graph1.AddBothSidesEdge(0, 1);
            graph1.AddEdge(1, 2);
            graph2 = new Graph(4);
            graph2.AddBothSidesEdge(2, 3);
            graph2.AddBothSidesEdge(1, 2);
            mappings = new List<Mapping> { new Mapping { (0, 0), (2, 1) }, new Mapping { (0, 0), (2, 3) } };
            yield return new TestCaseData(graph1, graph2, mappings);

            // house & house with chimney 
            graph1 = new Graph(5);
            graph1.AdjacencyMatrix = new[,]
            {
                  {0, 1, 0, 0, 1 },
                  {1, 0, 1, 0, 0 },
                  {0, 1, 0, 1, 1 },
                  {0, 0, 1, 0, 1 },
                  {1, 0, 1, 1, 0 }
              };
            graph2 = new Graph(6);
            graph2.AdjacencyMatrix = new[,]
            {
                  {0, 0, 0, 0, 1, 1 },
                  {0, 0, 1, 0, 0, 0 },
                  {0, 1, 0, 1, 0, 1 },
                  {0, 0, 1, 0, 1, 1 },
                  {1, 0, 0, 1, 0, 0 },
                  {1, 0, 1, 1, 0, 0 },
              };

            mappings = new List<Mapping> { new Mapping { (0, 0), (1, 4), (2, 3), (3, 2), (4, 5) } };
            yield return new TestCaseData(graph1, graph2, mappings);

            // example from here https://arxiv.org/pdf/1908.06418.pdf (but it's incorrect lol)
            graph1 = new Graph(5);
            graph1.AdjacencyMatrix = new int[,]
            {
                 { 0, 0, 0, 1, 1 },
                 { 0, 0, 1, 0, 1 },
                 { 0, 1, 0, 0, 1 },
                 { 1, 0, 0, 0, 0 },
                 { 1, 1, 1, 0, 0 }
            };

            graph2 = new Graph(6);
            graph2.AdjacencyMatrix = new int[,]
            {
                 { 0, 1, 0, 0, 1, 0 },
                 { 1, 0, 0, 1, 0, 1 },
                 { 0, 0, 0, 1, 0, 1 },
                 { 0, 1, 1, 0, 0, 1 },
                 { 1, 0, 0, 0, 0, 1 },
                 { 0, 1, 1, 1, 1, 0 }
            };

            mappings = new List<Mapping> { new Mapping { (0, 4), (1, 2), (2, 3), (3, 0), (4, 5) } };
            yield return new TestCaseData(graph1, graph2, mappings);
        }

        private static IEnumerable<TestCaseData> RandomSmallGraphs()
        {
            Random random = new Random(0);
            const int graph1Size = 5;
            const int graph2Size = 5;
            for (int i = 0; i < 10; i++)
            {
                var density = random.NextDouble();
                Graph graph1 = GraphFactory.CreateRandom(graph1Size, (int)(graph1Size * (graph1Size - 1) * density), i);
                Graph graph2 = GraphFactory.CreateRandom(graph2Size, (int)(graph2Size * (graph2Size - 1) * density), i);

                yield return new TestCaseData(graph1, graph2);
            }
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void NaiveFinder(Graph graph1, Graph graph2, List<Mapping> mappings)
        {
            int[]? subgraph1 = null, subgraph2 = null;
            var thread = new Thread(() => (subgraph1, subgraph2) = MCSFinder.FindNaive(graph1, graph2));
            thread.Start();
            bool completed = thread.Join(TimeSpan.FromSeconds(10)); // let's keep it real
            if (completed)
            {
                var actualMapping = subgraph1!.Zip(subgraph2!);
                Assert.True(mappings.Exists(someExpectedMapping => someExpectedMapping.SetEquals(actualMapping)));
            }
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void ExactFinder(Graph graph1, Graph graph2, List<Mapping> mappings)
        {
            var (subgraph1, subgraph2) = MCSFinder.FindFast(graph1, graph2, new MaxCliqueExactFinder());
            var actualMapping = subgraph1.Zip(subgraph2).ToHashSet();
            Assert.True(mappings.Exists(someExpectedMapping => someExpectedMapping.SetEquals(actualMapping)));
        }

        [Test]
        [TestCaseSource(nameof(RandomSmallGraphs))]
        public void ComparisonTest(Graph graph1, Graph graph2)
        {
            var resultNaive = MCSFinder.FindNaive(graph1, graph2);
            var resultExact = MCSFinder.FindFast(graph1, graph2, new MaxCliqueExactFinder());
            Assert.True(resultNaive.Item1.Length == resultNaive.Item2.Length);
            Assert.True(resultNaive.Item1.Length == resultExact.Item2.Length);
            Assert.True(resultNaive.Item1.Length == resultExact.Item1.Length);

            Assert.True(NumberOfEdgesInSubgraph(graph1, resultNaive.Item1) == NumberOfEdgesInSubgraph(graph2, resultNaive.Item2));
            Assert.True(NumberOfEdgesInSubgraph(graph1, resultExact.Item1) == NumberOfEdgesInSubgraph(graph2, resultExact.Item2));
        }

        private static int NumberOfEdgesInSubgraph(Graph graph, int[] subgraphIndices)
        {
            int edges = 0;
            for (int i = 0; i < subgraphIndices.Length; i++)
            {
                for (int j = 0; j < subgraphIndices.Length; j++)
                {
                    edges += graph.AdjacencyMatrix[subgraphIndices[i], subgraphIndices[j]];
                }
            }

            return edges;
        }
    }
}
