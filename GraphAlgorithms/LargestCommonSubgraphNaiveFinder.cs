namespace GraphAlgorithms;

public static class LargestCommonSubgraphFinder
{
    public static int Find(Graph graph1, Graph graph2)
    {
        int minSize = Math.Min(graph1.Size, graph2.Size);
        var graph1Subsets = GenerateAllSubsetsSmallerOrEqualToSize(graph1.Size, minSize);
        var graph2Subsets = GenerateAllSubsetsSmallerOrEqualToSize(graph2.Size, minSize);
        var maxCommonSize = 0;

        for (int i = 0; i < graph1Subsets.Count; i++)
        {
            for (int j = 0; j < graph2Subsets.Count; j++)
            {
                if (graph1Subsets[i].Count != graph2Subsets[j].Count) continue;

                foreach (var perm1 in Permutations(graph1Subsets[i]))
                {
                    foreach (var perm2 in Permutations(graph2Subsets[j]))
                    {
                        if (AreCommon(graph1, graph2, perm1, perm2))
                        {
                            if (graph1Subsets[i].Count > maxCommonSize)
                            {
                                maxCommonSize = graph1Subsets[i].Count;
                            }
                        }
                    }
                }
            }
        }

        return maxCommonSize;
    }

    public static List<List<int>> GenerateAllSubsetsSmallerOrEqualToSize(int n, int size)
    {
        var allSubsets = new List<List<int>>();
        int totalSubsets = 1 << n;

        for (int i = 0; i < totalSubsets; i++)
        {
            var subset = new List<int>();
            for (int j = 0; j < n; j++)
            {
                if ((i & (1 << j)) != 0)
                {
                    subset.Add(j);
                }
            }

            if (subset.Count > size) continue;
            allSubsets.Add(subset);
        }

        return allSubsets;
    }

    public static List<List<int>> Permutations(List<int> values)
    {
        if (values.Count == 1)
            return new List<List<int>>() { values };
        return values.SelectMany(v => Permutations(values.Where(x => x.CompareTo(v) != 0).ToList()), (v, p) => p.Prepend(v).ToList()).ToList();
    }

    public static bool AreCommon(Graph graph1, Graph graph2, List<int> subset1, List<int> subset2)
    {
        int count = subset1.Count;

        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < count; j++)
            {
                if (graph1.AdjacencyMatrix[subset1[i], subset1[j]] != graph2.AdjacencyMatrix[subset2[i], subset2[j]])
                {
                    return false;
                }
            }
        }

        return true;
    }
}