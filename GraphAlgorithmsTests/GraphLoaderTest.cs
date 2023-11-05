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
            for (int i = 0; i < graph1Size; i++)
            {
                for (int j = 0; j < graph1Size; j++)
                {
                    graph1.AdjacencyMatrix[i, j] = i * graph1Size + j + 1;
                }
            }
            int graph2Size = 4;
            var graph2 = new Graph(graph2Size);
            for (int i = 0; i < graph2Size; i++)
            {
                for (int j = 0; j < graph2Size; j++)
                {
                    graph2.AdjacencyMatrix[i, j] = i * graph2Size + j + 1;
                }
            }
            int graph3Size = 2;
            var graph3 = new Graph(graph3Size);
            for (int i = 0; i < graph3Size; i++)
            {
                for (int j = 0; j < graph3Size; j++)
                {
                    graph3.AdjacencyMatrix[i, j] = i * graph3Size + j + 1;
                }
            }

            var graphs = new List<Graph> { graph1, graph2, graph3 };
            yield return new TestCaseData(path, graphs);
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public static void TestLoading(string path, List<Graph> expectedGraphs)
        {
            Graph[] graphs = GraphLoader.Load(path);
            Assert.That(graphs, Is.EqualTo(expectedGraphs));
        }
    }
}
