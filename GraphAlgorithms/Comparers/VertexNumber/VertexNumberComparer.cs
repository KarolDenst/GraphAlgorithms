namespace GraphAlgorithms.Comparers.VertexNumber;

public class VertexNumberComparer : ISizeComparer<VertexNumberSize>
{
    public VertexNumberComparer() { }

    public int Compare(List<int> vertices1, List<int> vertices2)
    {
        var size1 = GetSize(vertices1);
        var size2 = GetSize(vertices2);
        return size1!.CompareTo(size2);
    }

    public int Compare(List<int> vertices, VertexNumberSize size)
    {
        var size2 = GetSize(vertices);
        return size2.CompareTo(size);
    }

    public VertexNumberSize GetSize(List<int> vertices)
    {
        return new VertexNumberSize(vertices.Count);
    }
}