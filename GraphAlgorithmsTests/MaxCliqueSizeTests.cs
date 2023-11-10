using GraphAlgorithms.Clique;
using GraphAlgorithms.Graphs;
using NUnit.Framework;

namespace GraphAlgorithmsTests
{
    [TestFixture]
    public class MaxCliqueSizeTests
    {
        private static IEnumerable<TestCaseData> TestCases()
        {
            Graph graph;

            // Simple clique
            graph = new Graph(3);
            graph.AdjacencyMatrix = new[,]
            {
                { 0, 2, 2 },
                { 2, 0, 2 },
                { 2, 2, 0 },
            };
            yield return new TestCaseData(graph, 3, 12);

            // Same vertex number different edge number.
            graph = new Graph(6);
            graph.AdjacencyMatrix = new[,]
            {
                { 0, 2, 2, 0, 0, 0 },
                { 2, 0, 2, 0, 0, 0 },
                { 2, 2, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 1 },
                { 0, 0, 0, 1, 0, 1 },
                { 0, 0, 0, 1, 1, 0 },
            };
            yield return new TestCaseData(graph, 3, 12);

            // Same vertex number different edge number.
            graph = new Graph(6);
            graph.AdjacencyMatrix = new[,]
            {
                { 0, 1, 1, 0, 0, 0 },
                { 1, 0, 1, 0, 0, 0 },
                { 1, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 2, 2 },
                { 0, 0, 0, 2, 0, 2 },
                { 0, 0, 0, 2, 2, 0 },
            };
            yield return new TestCaseData(graph, 3, 12);

            // Same vertex number different edge number with some self connection.
            graph = new Graph(6);
            graph.AdjacencyMatrix = new[,]
            {
                { 0, 1, 1, 0, 0, 0 },
                { 1, 40, 1, 0, 0, 0 },
                { 1, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 2, 2 },
                { 0, 0, 0, 2, 0, 2 },
                { 0, 0, 0, 2, 2, 0 },
            };
            yield return new TestCaseData(graph, 3, 12);
        }

        private int GetNumberOfEdges(Graph graph, List<int> indices)
        {
            var numberOfEdges = 0;
            foreach (var index in indices)
            {
                foreach (var index2 in indices)
                {
                    if (index2 >= index) continue;
                    numberOfEdges += graph.AdjacencyMatrix[index, index2];
                    numberOfEdges += graph.AdjacencyMatrix[index2, index];
                }
            }
            return numberOfEdges;
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void FindCliquesExact(Graph graph, int size, int edges)
        {
            var finder = new MaxCliqueExactFinder();
            var result = finder.FindWithEdges(graph);
            var numberOfEdges = GetNumberOfEdges(graph, result);

            Assert.AreEqual(size, result.Count);
            Assert.AreEqual(edges, numberOfEdges);
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void FindCliquesHeu(Graph graph, int size, int edges)
        {
            var finder = new MaxCliqueHeuFinder();
            var result = finder.FindWithEdges(graph);
            var numberOfEdges = GetNumberOfEdges(graph, result);

            Assert.AreEqual(size, result.Count);
            Assert.AreEqual(edges, numberOfEdges);
        }
    }
}
