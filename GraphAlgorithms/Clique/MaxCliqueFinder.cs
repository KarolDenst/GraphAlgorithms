using GraphAlgorithms.Comparers;

namespace GraphAlgorithms.Clique;

/// <summary>
/// The MaxCliqueFinder class finds the maximum clique in a graph.
/// It uses the algorithm described in this paper https://www.internetmathematicsjournal.com/article/1586.
/// The algorithm is also slightly modified to return a list of vertices not just the number.
/// Also it's modified to treat a single vertex as a clique.
/// There are naming changes to keep up with the naming convention of the rest of the project.
/// Here is the change list:
/// U -> availableVertices
/// N'(u) -> neighbors
/// d(v) -> _degrees[v]
/// max -> _maxSize
/// </summary>
public class MaxCliqueFinder<TCliqueSize>
{
    private readonly Graph _graph;

    private readonly int[] _degrees;

    private List<int> _maxClique = new();

    private readonly ISizeComparer<TCliqueSize> _comparer;

    private TCliqueSize _maxSize;

    private MaxCliqueFinder(Graph graph, ISizeComparer<TCliqueSize> sizeComparer)
    {
        _graph = graph;
        _degrees = new int[graph.Size];
        for (int i = 0; i < graph.Size; i++)
        {
            _degrees[i] = graph.GetDegree(i);
        }

        _comparer = sizeComparer;
        _maxSize = _comparer.GetSize(new List<int>());
    }

    public static List<int> FindHeuristic(Graph graph, ISizeComparer<TCliqueSize> sizeComparer)
    {
        var finder = new MaxCliqueFinder<TCliqueSize>(graph, sizeComparer);
        return finder.FindHeuristic();
    }

    private List<int> FindHeuristic()
    {
        if (_graph.Size == 0)
            return new List<int>();

        for (int i = 0; i < _graph.Size; i++)
        {
            if (_degrees[i] >= _maxClique.Count)
            {
                var availableVertices = new List<int>();
                foreach (var v in _graph.GetNeighbors(i))
                {
                    if (_degrees[v] >= _maxClique.Count)
                    {
                        availableVertices.Add(v);
                    }
                }
                CliqueHeuristic(availableVertices, new List<int> { i });
            }
        }

        if (_maxClique.Count == 0)
            return new List<int> { 0 };
        _maxClique.Sort();
        return _maxClique;
    }

    private void CliqueHeuristic(List<int> availableVertices, List<int> clique)
    {
        if (availableVertices.Count == 0)
        {
            if (_comparer.Compare(clique, _maxSize) > 0)
            {
                _maxClique = clique;
                _maxSize = _comparer.GetSize(clique);
            }

            return;
        }

        int u = GetMaxDegreeVertex(availableVertices);
        availableVertices.Remove(u);
        var neighbors = GetNeighbors(u);
        clique.Add(u);

        CliqueHeuristic(GetUnion(availableVertices, neighbors), clique);
    }

    private int GetMaxDegreeVertex(List<int> availableVertices)
    {
        int max = 0;
        int maxVertex = 0;
        foreach (var vertex in availableVertices)
        {
            if (_degrees[vertex] > max)
            {
                max = _degrees[vertex];
                maxVertex = vertex;
            }
        }

        return maxVertex;
    }

    public static List<int> FindExact(Graph graph, ISizeComparer<TCliqueSize> sizeComparer)
    {
        var finder = new MaxCliqueFinder<TCliqueSize>(graph, sizeComparer);
        return finder.FindExact();
    }

    private List<int> FindExact()
    {
        if (_graph.Size == 0)
            return new List<int>();

        for (int i = 0; i < _graph.Size; i++)
        {
            if (_degrees[i] >= _maxClique.Count)
            {
                var availableVertices = new List<int>();
                foreach (var v in _graph.GetNeighbors(i))
                {
                    if (v > i)
                    {
                        if (_degrees[v] >= _maxClique.Count)
                        {
                            availableVertices.Add(v);
                        }
                    }
                }

                CliqueExact(availableVertices, new List<int> { i });
            }
        }

        if (_maxClique.Count == 0)
            return new List<int> { 0 };
        _maxClique.Sort();
        return _maxClique;
    }

    private void CliqueExact(List<int> availableVertices, List<int> clique)
    {
        if (availableVertices.Count == 0)
        {
            if (_comparer.Compare(clique, _maxSize) > 0)
            {
                _maxClique = new List<int>(clique);
                _maxSize = _comparer.GetSize(clique);
            }

            return;
        }

        while (availableVertices.Count > 0)
        {
            if (clique.Count + availableVertices.Count <= _maxClique.Count)
                return;

            var u = availableVertices[0];
            availableVertices.Remove(u);
            var neighbors = GetNeighbors(u);

            clique.Add(u);
            CliqueExact(GetUnion(availableVertices, neighbors), clique);
            clique.Remove(u);
        }
    }

    private List<int> GetNeighbors(int u)
    {
        List<int> neighbors = new List<int>();
        foreach (var w in _graph.GetNeighbors(u))
        {
            if (_degrees[w] >= _maxClique.Count)
            {
                neighbors.Add(w);
            }
        }

        return neighbors;
    }

    // This method uses the fact that both sets are sorted. The algorithm breaks if they are not sorted.
    private List<int> GetUnion(List<int> set1, List<int> set2)
    {
        var union = new List<int>();
        int i = 0;
        int j = 0;
        while (i < set1.Count && j < set2.Count)
        {
            if (j == set2.Count || set1[i] < set2[j])
            {
                i++;
            }
            else if (i == set1.Count || set1[i] > set2[j])
            {
                j++;
            }
            else
            {
                union.Add(set1[i]);
                i++;
                j++;
            }
        }

        return union;
    }
}