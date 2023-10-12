namespace GraphAlgorithms;

public class Graph
{
    public int[,] AdjacencyMatrix { get; set; }

    public int Size { get; }

    public Graph(int size)
    {
        Size = size;
        AdjacencyMatrix = new int[size, size];
    }
    
    public void AddEdge(int from, int to) => AdjacencyMatrix[from, to] += 1;
    
    public void SetEdge(int from, int to, int value) => AdjacencyMatrix[from, to] = value;
    
    public int GetEdge(int from, int to) => AdjacencyMatrix[from, to];
    
    public int GetDegree(int vertex)
    {
        int degree = 0;
        for (int i = 0; i < Size; i++)
        {
            degree += AdjacencyMatrix[vertex, i];
            degree += AdjacencyMatrix[i, vertex];
        }
        return degree;
    }
    
    public IEnumerable<int> GetOutVertices(int vertex)
    {
        for (int i = 0; i < Size; i++)
        {
            if (AdjacencyMatrix[vertex, i] > 0)
            {
                yield return i;
            }
        }
    }
    
    public IEnumerable<int> GetInVertices(int vertex)
    {
        for (int i = 0; i < Size; i++)
        {
            if (AdjacencyMatrix[i, vertex] > 0)
            {
                yield return i;
            }
        }
    }

    public static Graph Load(string fileName)
    {
        using StreamReader reader = new StreamReader(fileName);
        int size = int.Parse(reader.ReadLine()!);
        Graph graph = new Graph(size);

        for (int i = 0; i < size; i++)
        {
            string[] line = reader.ReadLine()!.Split(' ');
            for (int j = 0; j < size; j++)
            {
                graph.AdjacencyMatrix[i, j] = int.Parse(line[j]);
            }
        }
        return graph;
    }
    
    public void Save(string fileName)
    {
        using StreamWriter writer = new StreamWriter(fileName);
        writer.WriteLine(Size);

        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                writer.Write(AdjacencyMatrix[i, j]);
                if (j < Size - 1) writer.Write(' ');
            }
            writer.WriteLine();
        }
    }
}