using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms
{
    public static class LargestCommonSubgraphNaiveFinder
    {
        public static LargestCommonSubgraphFinderResult Find(Graph graph1, Graph graph2)
        {
            int minSize = Math.Min(graph1.Size, graph2.Size);
            int maxSize = Math.Max(graph1.Size, graph2.Size);
            Graph result = new Graph(maxSize);
            int resultNumOfEdges = 0;
            if (graph1.Size < graph2.Size)
                (graph1, graph2) = (graph2, graph1);
            List<int> listOfIndices = Enumerable.Range(0, graph1.Size).ToList();
            List<int> rowPermutationUsedInResult = Enumerable.Range(0, graph1.Size).ToList();
            List<int> columnPermutationUsedInResult = Enumerable.Range(0, graph1.Size).ToList();
            List<List<int>> indicesPermutations = GeneratePermutations(listOfIndices);
            foreach (var columnPermutation in indicesPermutations)
            {
                foreach (var rowPermutation in indicesPermutations)
                {
                    // search for potential result
                    Graph potentialResult = new Graph(maxSize);
                    int numOfEdges = 0;
                    for (int i = 0; i < minSize; i++)
                    {
                        for (int j = 0; j < minSize; j++)
                        {
                            // remove edeges from a vertex to itself
                            if (i == j) continue;
                            var graph1Value = graph1.AdjacencyMatrix[rowPermutation[i], columnPermutation[j]];
                            var graph2Value = graph2.AdjacencyMatrix[i, j];
                            if (graph1Value != 0 && graph2Value != 0)
                            {
                                potentialResult.AddEdge(i, j);
                                ++numOfEdges;
                            }
                        }
                    }
                    if (numOfEdges > resultNumOfEdges)
                    {
                        result = potentialResult;
                        resultNumOfEdges = numOfEdges;
                        rowPermutationUsedInResult = rowPermutation;
                        columnPermutationUsedInResult = columnPermutation;
                    }
                }
            }
            var newGraph1 = GetGraphAfterPermutations(graph1, rowPermutationUsedInResult, columnPermutationUsedInResult);
            return new LargestCommonSubgraphFinderResult(newGraph1, graph2, result);
        }

        private static Graph GetGraphAfterPermutations(Graph graph, List<int> rowPermutationUsedInResult, List<int> columnPermutationUsedInResult)
        {
            var newGraph = new Graph(graph.Size);
            for (int i = 0; i < newGraph.Size; ++i)
            {
                for (int j = 0; j < newGraph.Size; ++j)
                {
                    if (graph.AdjacencyMatrix[i, j] == 1)
                    {
                        int newRowIndex = rowPermutationUsedInResult[i];
                        int newColumnIndex = columnPermutationUsedInResult[j];
                        newGraph.AdjacencyMatrix[newRowIndex, newColumnIndex] = 1;
                    }
                }
            }
            return newGraph;
        }

        private static List<List<int>> GeneratePermutations(List<int> elements)
        {
            List<List<int>> permutations = new List<List<int>>();
            GeneratePermutationsRecursive(elements, 0, elements.Count - 1, permutations);
            return permutations;
        }

        private static void GeneratePermutationsRecursive(List<int> elements, int start, int end, List<List<int>> permutations)
        {
            if (start == end)
            {
                permutations.Add(new List<int>(elements));
            }
            else
            {
                for (int i = start; i <= end; i++)
                {
                    (elements[start], elements[i]) = (elements[i], elements[start]);
                    GeneratePermutationsRecursive(elements, start + 1, end, permutations);
                    (elements[start], elements[i]) = (elements[i], elements[start]);
                }
            }
        }
    }
}
