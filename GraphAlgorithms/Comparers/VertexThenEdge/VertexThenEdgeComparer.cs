namespace GraphAlgorithms.Comparers.VertexThenEdge;

public class VertexThenEdgeComparer : ISizeComparer<VertexThenEdgeSize>
{
    private readonly Graph _graph;

    public VertexThenEdgeComparer(Graph graph)
    {
        _graph = graph;
    }

    public int Compare(List<int> vertices1, List<int> vertices2)
    {
        var size1 = GetSize(vertices1);
        var size2 = GetSize(vertices2);
        return size1!.CompareTo(size2);
    }

    public int Compare(List<int> vertices, VertexThenEdgeSize size)
    {
        var size2 = GetSize(vertices);
        return size2!.CompareTo(size);
    }

    public VertexThenEdgeSize GetSize(List<int> vertices)
    {
        int vertexCount = vertices.Count;

        int edgeCount = 0;
        foreach (var v in vertices)
        {
            foreach (var u in vertices)
            {
                edgeCount += _graph.GetNumberOfEdges(v, u);
            }
        }

        return new VertexThenEdgeSize((vertexCount, edgeCount));
    }
}