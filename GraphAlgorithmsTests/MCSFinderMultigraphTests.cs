using GraphAlgorithms.Clique;
using GraphAlgorithms.Graphs;
using GraphAlgorithms.MCS;
using NUnit.Framework;

namespace GraphAlgorithmsTests;

using Mapping = System.Collections.Generic.HashSet<(int, int)>;

[TestFixture]
public class MCSFinderMultigraphTests
{
    private static IEnumerable<TestCaseData> TestCases()
    {
        Graph graph1;
        Graph graph2;
        List<Mapping> mappings; // each set corresponds to one mapping
        
        graph1 = new Graph(2);
        graph1.AdjacencyMatrix = new[,]
        {
              { 0, 1 },
              { 1, 0 }, 
        };
        graph2 = new Graph(2);
        graph2.AdjacencyMatrix = new[,]
        {
            { 0, 10 },
            { 10, 0 },
        };

        mappings = new List<Mapping> { new Mapping { (0, 0), (1, 1) } };
        yield return new TestCaseData(graph1, graph2, mappings);
        
        graph1 = new Graph(6)
        {
            AdjacencyMatrix = new[,]
            {
                { 0, 1, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 0, 0 }, 
                { 0, 0, 0, 5, 0, 0 }, 
                { 0, 0, 5, 0, 0, 0 }, 
                { 0, 0, 0, 0, 0, 1 },
                { 0, 0, 0, 0, 1, 0 }, 
            }
        };
        graph2 = new Graph(2)
        {
            AdjacencyMatrix = new[,]
            {
                { 0, 10 },
                { 10, 0 },
            }
        };

        mappings = new List<Mapping> { new Mapping { (2, 0), (3, 1) } };
        yield return new TestCaseData(graph1, graph2, mappings);
    }
    
    [Test]
    [TestCaseSource(nameof(TestCases))]
    public void ExactFinder(Graph graph1, Graph graph2, List<Mapping> mappings)
    {
        var (subgraph1, subgraph2) = MCSFinder.FindFast(graph1, graph2, new MaxCliqueExactFinder());
        var actualMapping = subgraph1.Zip(subgraph2).ToHashSet();
        Assert.True(mappings.Exists(someExpectedMapping => someExpectedMapping.SetEquals(actualMapping)));
    }
}