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
