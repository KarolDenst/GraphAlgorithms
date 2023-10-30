namespace GraphAlgorithms.Comparers;

public interface ISizeComparer
{
    /// <summary>
    /// Returns 1 if subgraph of vertices1 is larger than subgraph of vertices2.
    /// Returns 0 if they are equal.
    /// Returns -1 if its smaller.
    /// </summary>
    /// <param name="vertices1"></param>
    /// <param name="vertices2"></param>
    /// <returns></returns>
    int Compare(List<int> vertices1, List<int> vertices2);

    /// <summary>
    /// Returns 1 if subgraph of vertices1 is larger than size.
    /// Returns 0 if they are equal.
    /// Returns -1 if its smaller.
    /// </summary>
    /// <param name="vertices"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    int Compare(List<int> vertices, object size);

    /// <summary>
    /// Returns the size of the subgraph.
    /// </summary>
    /// <param name="vertices"></param>
    /// <returns></returns>
    object GetSize(List<int> vertices);
}