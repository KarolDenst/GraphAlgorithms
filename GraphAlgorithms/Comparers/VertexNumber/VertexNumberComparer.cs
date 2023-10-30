namespace GraphAlgorithms.Comparers;

public class VertexNumberComparer : ISizeComparer
{
    public VertexNumberComparer() { }
    
    public int Compare(List<int> vertices1, List<int> vertices2)
    {
        var size1 = GetSize(vertices1) as VertexNumberSize;
        var size2 = GetSize(vertices2) as VertexNumberSize;
        return size1!.CompareTo(size2);
    }

    public int Compare(List<int> vertices, object size)
    {
        var size2 = GetSize(vertices) as VertexNumberSize;
        return size2!.CompareTo(size);
    }

    public object GetSize(List<int> vertices)
    {
        return new VertexNumberSize(vertices.Count);
    }
}