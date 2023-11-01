using GraphAlgorithms.Graphs;
using GraphAlgorithms.Utils;
using NUnit.Framework;

namespace GraphAlgorithmsTests
{
    internal class MetricsTest
    {
        private static IEnumerable<TestCaseData> TestCases()
        {
            Graph graph1;
            Graph graph2;

            graph1 = new Graph(3);
            graph1.AddBothSidesEdge(0, 1);
            graph1.AddBothSidesEdge(0, 2);
            graph1.AddBothSidesEdge(1, 2);
            graph2 = new Graph(4);
            yield return new TestCaseData(graph1, graph2, 0.5, 1);

            graph1 = new Graph(3);
            graph1.AddBothSidesEdge(0, 1);
            graph1.AddBothSidesEdge(0, 2);
            graph1.AddBothSidesEdge(1, 2);
            graph2 = new Graph(3);
            graph2.AddBothSidesEdge(0, 1);
            graph2.AddBothSidesEdge(0, 2);
            graph2.AddBothSidesEdge(1, 2);
            yield return new TestCaseData(graph1, graph2, 0, 0.001);
        }

        private static IEnumerable<TestCaseData> TriangleInequality()
        {
            Random random = new Random(0);
            Graph[] graphs = new Graph[3];
            int[] sizes = new int[3];
            for (int i = 0; i < 10; i++)
            {
                for (int graphId = 0; graphId < 3; graphId++)
                {
                    int size = sizes[graphId] = random.Next(0, 10);
                    double density = random.NextDouble();
                    graphs[graphId] = GraphFactory.CreateRandom(size, (int)(size * (size - 1) * density), i);
                }

                yield return new TestCaseData(graphs[0], graphs[1], graphs[2]);
            }
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void MCSMetricBoundTest(Graph graph1, Graph graph2, double low, double high)
        {
            double d = Metrics.CalculateBasedOnMCS(graph1, graph2);
            Assert.True(d >= low && d <= high);
        }

        [Test]
        [TestCaseSource(nameof(TriangleInequality))]
        public void MCSMetricTriangleIneqTest(Graph graph0, Graph graph1, Graph graph2)
        {
            double d01 = Metrics.CalculateBasedOnMCS(graph0, graph1);
            double d12 = Metrics.CalculateBasedOnMCS(graph1, graph2);
            double d02 = Metrics.CalculateBasedOnMCS(graph0, graph2);
            Assert.True(d01 + d12 >= d02);
        }
    }
}
