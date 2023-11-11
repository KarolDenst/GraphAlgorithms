using GraphAlgorithms.Graphs;

namespace GraphAlgorithms.Commands;

public static class Utils
{
    public static Graph GetGraph(string path, int index)
    {
        var graphs = GraphLoader.Load(path);

        if (index > graphs.Length || index < 0)
        {
            Console.WriteLine("Index out of bounds.");
            throw new IndexOutOfRangeException();
        }

        return graphs[index];
    }
}