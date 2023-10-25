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
            int[][]? permutationUsedInResult = null;
            if (graph1.Size < graph2.Size)
                (graph1, graph2) = (graph2, graph1);
            List<int[][]> columnsPermutations = GenerateColumnsPermutations(graph1.AdjacencyMatrix);
            for(int colPer = 0; colPer < columnsPermutations.Count; ++colPer)
            {
                var columnPermutation = columnsPermutations[colPer];
                List<int[][]> rowsPermutations = GenerateRowsPermutations(columnPermutation);
                for (int rowPer = 0; rowPer < rowsPermutations.Count; ++rowPer)
                {
                    var rowPermutation = rowsPermutations[rowPer];
                    // remove edeges from a vertex to itself
                    for (int i = 0; i < rowPermutation.Length; i++)
                        rowPermutation[i][i] = 0;
                    // search for potential result
                    Graph potentialResult = new Graph(maxSize);
                    int numOfEdges = 0;
                    for (int i = 0; i < minSize; i++)
                    {
                        for (int j = 0; j < minSize; j++)
                        {
                            if (rowPermutation[i][j] == 1 && graph2.AdjacencyMatrix[i][j] == 1)
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
                        permutationUsedInResult = rowPermutation;
                    }
                }
            }
            var newGraph1 = new Graph(graph1.Size);
            newGraph1.AdjacencyMatrix = permutationUsedInResult ?? new int[newGraph1.Size][];
            return new LargestCommonSubgraphFinderResult(newGraph1, graph2, result);
        }

        public static List<int[][]> GenerateColumnsPermutations(int[][] table)
        {
            List<int[][]> permutations = new List<int[][]>();
            PermuteColumns(table, 0, permutations);
            return permutations;
        }

        private static void PermuteColumns(int[][] table, int start, List<int[][]> permutations)
        {
            if (start == table[0].Length - 1)
            {
                int[][] clone = table.Select(row => row.ToArray()).ToArray();
                permutations.Add(clone);
            }
            else
            {
                for (int i = start; i < table[0].Length; i++)
                {
                    SwapColumns(table, start, i);
                    PermuteColumns(table, start + 1, permutations);
                    SwapColumns(table, start, i);
                }
            }
        }

        private static void SwapColumns(int[][] table, int column1, int column2)
        {
            for (int i = 0; i < table.Length; i++)
                (table[i][column1], table[i][column2]) = (table[i][column2], table[i][column1]);
        }


        public static List<int[][]> GenerateRowsPermutations(int[][] originalArray)
        {
            List<int[][]> permutations = new List<int[][]>();
            int rows = originalArray.Length;
            int cols = originalArray[0].Length;
            PermuteRows(originalArray, permutations, 0, rows, cols);
            return permutations;
        }

        private static void PermuteRows(int[][] originalArray, List<int[][]> permutations, int startRow, int rows, int cols)
        {
            if (startRow == rows)
            {
                int[][] copy = new int[rows][];
                for (int i = 0; i < rows; i++)
                {
                    copy[i] = new int[cols];
                    for (int j = 0; j < cols; j++)
                        copy[i][j] = originalArray[i][j];
                }
                permutations.Add(copy);
                return;
            }
            for (int i = startRow; i < rows; i++)
            {
                (originalArray[startRow], originalArray[i]) = (originalArray[i], originalArray[startRow]);
                PermuteRows(originalArray, permutations, startRow + 1, rows, cols);
                (originalArray[startRow], originalArray[i]) = (originalArray[i], originalArray[startRow]);
            }
        }
    }
}
