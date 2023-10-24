using GraphAlgorithms;
using System;


var clique = GraphFactory.CreateClique(10);
var found = LargestCliqueApproximator.Find(clique);
Console.WriteLine($"Clique size: {clique.Size}, found: {found.Vertices.Count}, edges: {found.EdgeCount}");

for (int i = 0; i < 100; i++)
{
    var cliqueSize = 8;
    var random = GraphFactory.CreateRandomWithClique(30, 300, cliqueSize, i);
    found = LargestCliqueApproximator.Find(random);
    Console.WriteLine($"Min clique size: {cliqueSize}, found: {found.Vertices.Count}, edges: {found.EdgeCount}");
}



var result = LargestCommonSubgraphNaiveFinder.GenerateColumnsPermutations(new int[][] { new int[] { 1, 2, 3 }, new int[] { 4, 5, 6 }, new int[] { 7, 8, 9 } });
for(int i = 0; i < result.Count; i++)
{
    for (int j = 0; j < result[i].Length; j++)
    {
        for (int k = 0; k < result[i][j].Length; k++)
        {
            Console.Write(result[i][j][k] + " ");
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}