using GraphAlgorithms;
using NUnit.Framework;

namespace GraphAlgorithmsTests
{
    [TestFixture]
    public class MCSFinderTest
    {
        private static IEnumerable<TestCaseData> TestCases()
        {
            Graph graph1;
            Graph graph2;
            HashSet<int[]> subgraphs1; // sets in case someone would like to be very diligent & actually enumerate all possible MCS's (but I'm not doing that here)
            HashSet<int[]> subgraphs2;

            // two completely different graphs
            graph1 = new Graph(3);
            graph1.AddBothSidesEdge(0, 1);
            graph2 = new Graph(3);
            subgraphs1 = new HashSet<int[]>() { new int[] { 0, 2 } };
            subgraphs2 = new HashSet<int[]>() { new int[] { 0, 1 } };
            yield return new TestCaseData(graph1, graph2, subgraphs1, subgraphs2);

            // two equal graph
            graph1 = new Graph(3);
            graph1.AddBothSidesEdge(0, 1);
            graph1.AddBothSidesEdge(1, 2);
            graph2 = new Graph(3);
            graph2.AddBothSidesEdge(0, 1);
            graph2.AddBothSidesEdge(1, 2);
            subgraphs1 = new HashSet<int[]>() { new int[3] { 0, 1, 2 } };
            subgraphs2 = new HashSet<int[]>() { new int[3] { 0, 1, 2 } };
            yield return new TestCaseData(graph1, graph2, subgraphs1, subgraphs2);

            // two graphs with common subgraph
            graph1 = new Graph(3);
            graph1.AddBothSidesEdge(0, 1);
            graph1.AddEdge(1, 2);
            graph2 = new Graph(3);
            graph2.AddBothSidesEdge(0, 1);
            graph2.AddEdge(0, 2);
            subgraphs1 = new HashSet<int[]>() { new int[] { 0, 1, 2 } };
            subgraphs2 = new HashSet<int[]>() { new int[] { 0, 1, 2 } };
            yield return new TestCaseData(graph1, graph2, subgraphs1, subgraphs2);

            // two graphs with common subgraphs with indexing change
            graph1 = new Graph(3);
            graph1.AddBothSidesEdge(0, 1);
            graph2 = new Graph(3);
            graph2.AddBothSidesEdge(1, 2);
            subgraphs1 = new HashSet<int[]>() { new int[] { 0, 1, 2 } };
            subgraphs2 = new HashSet<int[]>() { new int[] { 1, 2, 0 } };
            yield return new TestCaseData(graph1, graph2, subgraphs1, subgraphs2);

            // two graphs with different sizes
            graph1 = new Graph(3);
            graph1.AddBothSidesEdge(0, 1);
            graph1.AddEdge(1, 2);
            graph2 = new Graph(4);
            graph2.AddBothSidesEdge(2, 3);
            graph2.AddBothSidesEdge(1, 2);
            subgraphs1 = new HashSet<int[]>() { new int[] { 0, 1 }, new int[] { 0, 2 } };
            subgraphs2 = new HashSet<int[]>() { new int[] { 1, 2 }, new int[] { 0, 1 } };
            yield return new TestCaseData(graph1, graph2, subgraphs1, subgraphs2);

            // these tests take approx. 1 min to execute but pass. Commented out until we have a faster clique finder.

            /*// house & house with chimney 
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

            subgraphs1 = new HashSet<int[]>() { new int[] { 0, 1, 2, 3, 4 } };
            subgraphs2 = new HashSet<int[]>() { new int[] { 0, 4, 3, 5, 2 } };
            yield return new TestCaseData(graph1, graph2, subgraphs1, subgraphs2);

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

            subgraphs1 = new HashSet<int[]>() { new int[] { 0, 1, 2, 3, 4 } };
            subgraphs2 = new HashSet<int[]>() { new int[] { 0, 2, 3, 4, 5 } };
            yield return new TestCaseData(graph1, graph2, subgraphs1, subgraphs2);*/
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void FindTest(Graph graph1, Graph graph2, HashSet<int[]> subgraphs1, HashSet<int[]> subgraphs2)
        {
            var (subgraph1, subgraph2) = MCSFinder.Find(graph1, graph2, MaxCliqueNaiveFinder.Find);
            Assert.Contains(subgraph1.Order(), subgraphs1.Select(s => s.Order()).ToList());
            Assert.Contains(subgraph2.Order(), subgraphs2.Select(s => s.Order()).ToList());
        }
    }
}
