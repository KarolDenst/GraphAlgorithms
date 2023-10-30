namespace GraphAlgorithms.Comparers.VertexThenEdge;

public class VertexThenEdgeSize : IComparable
{
    private readonly (int, int) _size;

    public VertexThenEdgeSize((int, int) size)
    {
        _size = size;
    }

    public int CompareTo(object? obj)
    {
        if (obj == null)
            throw new ArgumentException("Object is null");

        if (obj is VertexThenEdgeSize otherTemperature)
            return _size.CompareTo(otherTemperature._size);

        throw new ArgumentException("Object is not a VertexNumberSize");
    }
}