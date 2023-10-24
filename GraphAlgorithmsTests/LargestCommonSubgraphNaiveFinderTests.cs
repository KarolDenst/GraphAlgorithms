using GraphAlgorithms;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Tests
{
    [TestFixture]
    public class LargestCommonSubgraphNaiveFinderTests
    {
        private static IEnumerable<TestCaseData> TestCases()
        {
            Graph graph1;
            Graph graph2;
            Graph result;

            // two completely different graphs
            graph1 = new Graph(3);
            graph1.AddBothSidesEdge(0, 1);
            graph2 = new Graph(3);
            result = new Graph(3);
            yield return new TestCaseData(graph1, graph2, result);

            // two equal graph
            graph1 = new Graph(3);
            graph1.AddBothSidesEdge(0, 1);
            graph1.AddBothSidesEdge(1, 2);
            graph2 = new Graph(3);
            graph2.AddBothSidesEdge(0, 1);
            graph2.AddBothSidesEdge(1, 2);
            result = new Graph(3);
            result.AddBothSidesEdge(0, 1);
            result.AddBothSidesEdge(1, 2);
            yield return new TestCaseData(graph1, graph2, result);

            // two graphs with common subgraph
            graph1 = new Graph(3);
            graph1.AddBothSidesEdge(0, 1);
            graph1.AddEdge(1, 2);
            graph2 = new Graph(3);
            graph2.AddBothSidesEdge(0, 1);
            graph2.AddEdge(0, 2);
            result = new Graph(3);
            result.AddBothSidesEdge(0, 1);
            result.AddEdge(1, 2);
            yield return new TestCaseData(graph1, graph2, result);

            // two graphs with common subgraphs with indexing change
            graph1 = new Graph(3);
            graph1.AddBothSidesEdge(0, 1);
            graph2 = new Graph(3);
            graph2.AddBothSidesEdge(1, 2);
            result = new Graph(3);
            result.AddBothSidesEdge(0, 1);
            yield return new TestCaseData(graph1, graph2, result);

            // two graphs with different sizes
            graph1 = new Graph(3);
            graph1.AddBothSidesEdge(0, 1);
            graph1.AddEdge(1, 2);
            graph2 = new Graph(4);
            graph2.AddBothSidesEdge(2, 3);
            graph2.AddBothSidesEdge(1, 2);
            result = new Graph(3);
            result.AddBothSidesEdge(0, 1);
            result.AddEdge(1, 2);
            yield return new TestCaseData(graph1, graph2, result);
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void FindTest(Graph graph1, Graph graph2, Graph expectedResult)
        {
            var result = LargestCommonSubgraphNaiveFinder.Find(graph1, graph2);
            Assert.That(result.CommonSubgraph.GetNumberOfEdges(), Is.EqualTo(expectedResult.GetNumberOfEdges()));
        }
    }
}