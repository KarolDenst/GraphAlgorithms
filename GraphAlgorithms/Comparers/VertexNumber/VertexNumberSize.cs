namespace GraphAlgorithms.Comparers.VertexNumber;

public class VertexNumberSize : IComparable<VertexNumberSize>
{
    private readonly int _size;

    public VertexNumberSize(int size)
    {
        _size = size;
    }

    public int CompareTo(VertexNumberSize? obj)
    {
        if (obj == null)
            throw new ArgumentException("Object is null");

        return _size.CompareTo(obj._size);
    }
}