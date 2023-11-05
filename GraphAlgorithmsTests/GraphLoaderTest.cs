using GraphAlgorithms.Graphs;
using NUnit.Framework;

namespace GraphAlgorithmsTests
{
    internal class GraphLoaderTest
    {
        private static IEnumerable<TestCaseData> TestCases()
        {
            string file = "threeGraphs.txt";
            string resourceDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
            string path = Path.Combine(resourceDir, file);

            int graph1Size = 3;
            var graph1 = new Graph(graph1Size);
            FillGraphConsecutiveNoLoops(graph1);

            int graph2Size = 4;
            var graph2 = new Graph(graph2Size);
            FillGraphConsecutiveNoLoops(graph2);

            int graph3Size = 2;
            var graph3 = new Graph(graph3Size);
            FillGraphConsecutiveNoLoops(graph3);

            var graphs = new List<Graph> { graph1, graph2, graph3 };
            yield return new TestCaseData(path, graphs);
        }

        private static void FillGraphConsecutiveNoLoops(Graph graph)
        {
            for (int i = 0; i < graph.Size; i++)
            {
                for (int j = 0; j < graph.Size; j++)
                {
                    if (i == j)
                        graph.AdjacencyMatrix[i, j] = 0;
                    else
                        graph.AdjacencyMatrix[i, j] = i * graph.Size + j + 1;
                }
            }
        }

        private static IEnumerable<TestCaseData> InvalidGraphTestCases()
        {
            string file = "threeGraphsLoops.txt";
            string resourceDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
            string path = Path.Combine(resourceDir, file);
            yield return new TestCaseData(path);
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public static void TestLoading(string path, List<Graph> expectedGraphs)
        {
            Graph[] graphs = GraphLoader.Load(path);
            Assert.That(graphs, Is.EqualTo(expectedGraphs));
        }

        [Test]
        [TestCaseSource(nameof(InvalidGraphTestCases))]
        public static void TestInvalidGraphHandling(string path)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => GraphLoader.Load(path));
        }
    }
}
