namespace GraphAlgorithms.Comparers;

public class VertexNumberSize : IComparable
{
    private readonly int _size;
    
    public VertexNumberSize(int size)
    {
        _size = size;
    }

    public int CompareTo(object? obj)
    {
        if (obj == null)
            throw new ArgumentException("Object is null");

        if (obj is VertexNumberSize otherTemperature)
            return _size.CompareTo(otherTemperature._size);
        
        throw new ArgumentException("Object is not a VertexNumberSize");
    }
}