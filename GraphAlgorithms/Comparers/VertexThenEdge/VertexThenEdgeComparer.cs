namespace GraphAlgorithms.Comparers.VertexThenEdge;

public class VertexThenEdgeComparer : ISizeComparer
{
    private readonly Graph _graph;

    public VertexThenEdgeComparer(Graph graph)
    {
        _graph = graph;
    }

    public int Compare(List<int> vertices1, List<int> vertices2)
    {
        var size1 = GetSize(vertices1) as VertexThenEdgeSize;
        var size2 = GetSize(vertices2) as VertexThenEdgeSize;
        return size1!.CompareTo(size2);
    }

    public int Compare(List<int> vertices, object size)
    {
        var size2 = GetSize(vertices) as VertexThenEdgeSize;
        return size2!.CompareTo(size);
    }

    public object GetSize(List<int> vertices)
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