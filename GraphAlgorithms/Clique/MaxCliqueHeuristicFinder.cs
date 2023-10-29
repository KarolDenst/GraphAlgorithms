namespace GraphAlgorithms.Clique;

public class MaxCliqueHeuristicFinder
{
    private Graph _graph;

    private int[] _degrees;
    
    private int _max = 0;
    
    private List<int> _maxClique = new List<int>();
    
    public MaxCliqueHeuristicFinder(Graph graph)
    {
        _graph = graph;
        _degrees = new int[graph.Size];
        for (int i = 0; i < graph.Size; i++)
        {
            _degrees[i] = graph.GetDegree(i);
        }
    }
    
    public List<int> Find()
    {
        for (int i = 0; i < _graph.Size; i++)
        {
            if (_degrees[i] >= _max)
            {
                var U = new List<int>();
                foreach (var v in _graph.GetNeighbors(i))
                {
                    if (_degrees[v] >= _max)
                    {
                        U.Add(v);
                    }
                }
                CliqueHeu(U, U.Count, new List<int>(){i});
            }
        }

        return _maxClique;
    }

    private void CliqueHeu(List<int> U, int size, List<int> clique)
    {
        if (U.Count == 0)
        {
            if (size > _max)
            {
                _max = size;
                _maxClique = clique;
            }

            return;
        }

        int u = GetMaxDegreeVertex(U);
        U.Remove(u);
        var N = GetN(u);
        clique.Add(u);
        
        CliqueHeu(GetSum(U, N), size + 1, clique);
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

    private List<int> GetN(int u)
    {
        List<int> N = new List<int>();
        foreach (var w in _graph.GetNeighbors(u))
        {
            if (_degrees[w] >= _max)
            {
                N.Add(w);
            }
        }

        return N;
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