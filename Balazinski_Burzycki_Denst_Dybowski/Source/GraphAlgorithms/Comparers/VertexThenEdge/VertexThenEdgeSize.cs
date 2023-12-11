namespace GraphAlgorithms.Comparers.VertexThenEdge;

public class VertexThenEdgeSize : IComparable<VertexThenEdgeSize>
{
    private readonly (int, int) _size;

    public static implicit operator (int, int)(VertexThenEdgeSize size)
        => size._size;

    public VertexThenEdgeSize((int, int) size)
    {
        _size = size;
    }

    public int CompareTo(VertexThenEdgeSize? obj)
    {
        if (obj == null)
            throw new ArgumentException("Object is null");

        return _size.CompareTo(obj._size);
    }
}