namespace GraphAlgorithms.Clique;

// TODO add source for this algorithm
public class MaxCliqueFinder
{
    private readonly Graph _graph;

    private readonly int[] _degrees;
    
    private List<int> _maxClique = new();
    
    public MaxCliqueFinder(Graph graph)
    {
        _graph = graph;
        _degrees = new int[graph.Size];
        for (int i = 0; i < graph.Size; i++)
        {
            _degrees[i] = graph.GetDegree(i);
        }
    }

    public static List<int> FindHeuristic(Graph graph)
    {
        var finder = new MaxCliqueFinder(graph);
        return finder.FindHeuristic();
    }
    
    public List<int> FindHeuristic()
    {
        if (_graph.Size == 0)
            return new List<int>();
        
        for (int i = 0; i < _graph.Size; i++)
        {
            if (_degrees[i] >= _maxClique.Count)
            {
                var U = new List<int>();
                foreach (var v in _graph.GetNeighbors(i))
                {
                    if (_degrees[v] >= _maxClique.Count)
                    {
                        U.Add(v);
                    }
                }
                CliqueHeuristic(U, new List<int>(){i});
            }
        }

        if (_maxClique.Count == 0)
            return new List<int>() { 0 };
        return _maxClique;
    }

    private void CliqueHeuristic(List<int> U, List<int> clique)
    {
        if (U.Count == 0)
        {
            if (clique.Count > _maxClique.Count)
            {
                _maxClique = clique;
            }

            return;
        }

        int u = GetMaxDegreeVertex(U);
        U.Remove(u);
        var neighbors = GetN(u);
        clique.Add(u);
        
        CliqueHeuristic(GetSum(U, neighbors), clique);
    }
    
    private int GetMaxDegreeVertex(List<int> U)
    {
        int max = 0;
        int maxVertex = 0;
        foreach (var vertex in U)
        {
            if (_degrees[vertex] > max)
            {
                max = _degrees[vertex];
                maxVertex = vertex;
            }
        }

        return maxVertex;
    }
    
    public static List<int> FindExact(Graph graph)
    {
        var finder = new MaxCliqueFinder(graph);
        return finder.FindExact();
    }

    public List<int> FindExact()
    {
        if (_graph.Size == 0)
            return new List<int>();

        for (int i = 0; i < _graph.Size; i++)
        {
            if (_degrees[i] >= _maxClique.Count)
            {
                var U = new List<int>();
                foreach (var v in _graph.GetNeighbors(i))
                {
                    if (v > i)
                    {
                        if (_degrees[v] >= _maxClique.Count)
                        {
                            U.Add(v);
                        }
                    }
                }

                CliqueExact(U, new List<int>() { i });
            }
        }

        if (_maxClique.Count == 0)
            return new List<int>() { 0 };
        return _maxClique;
    }

    private void CliqueExact(List<int> U, List<int> clique)
    {
        if (U.Count == 0)
        {
            if (clique.Count > _maxClique.Count)
            {
                _maxClique = new List<int>(clique);
            }

            return;
        }

        while (U.Count > 0)
        {
            if (clique.Count + U.Count <= _maxClique.Count)
                return;

            var u = U[0];
            U.Remove(u);
            var neighbors = GetN(u);
            
            clique.Add(u);
            CliqueExact(GetSum(U, neighbors), clique);
            clique.Remove(u);
        }
    }

    private List<int> GetN(int u)
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
    
    // Could be optimized if we knew that both are sorted. Which I think is the case
    private List<int> GetSum(List<int> U, List<int> N)
    {
        List<int> sum = new List<int>();
        foreach (var vertex in U)
        {
            if (N.Contains(vertex))
            {
                sum.Add(vertex);
            }
        }

        return sum;
    }
}