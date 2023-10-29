using System.Text;

namespace GraphAlgorithms;

public class Graph
{
    public int[,] AdjacencyMatrix { get; set; }

    public int Size { get; }

    public Graph(int size)
    {
        Size = size;
        AdjacencyMatrix = new int[size,size];
    }
    
    public void AddEdge(int from, int to) => AdjacencyMatrix[from,to] += 1;

    public void AddBothSidesEdge(int u, int v)
    {
        AdjacencyMatrix[u,v] += 1;
        AdjacencyMatrix[v,u] += 1;
    }
    
    public void SetNumberOfEdges(int from, int to, int value) => AdjacencyMatrix[from,to] = value;
    
    public int GetNumberOfEdges(int from, int to) => AdjacencyMatrix[from,to];

    public int GetNumberOfEdges()
    {
        int numOfEdges = 0;
        foreach(var val in AdjacencyMatrix)
        {
                numOfEdges += val;
        }
        return numOfEdges;
    }
    
    public int GetDegree(int vertex)
    {
        int degree = 0;
        for (int i = 0; i < Size; i++)
        {
            if (AdjacencyMatrix[vertex, i] > 0 && AdjacencyMatrix[i, vertex] > 0)
                degree++;
        }
        return degree;
    }
    
    public bool AreNeighborsInBothDirections(int vertex1, int vertex2)
    {
        if (AdjacencyMatrix[vertex1,vertex2] != 0 && AdjacencyMatrix[vertex2,vertex1] != 0)
            return true;
        return false;
    }
    
    public IEnumerable<int> GetNeighbors(int vertex)
    {
        for (int i = 0; i < Size; i++)
        {
            if (AreNeighborsInBothDirections(vertex, i))
            {
                yield return i;
            }
        }
    }
    
    public IEnumerable<int> GetOutVertices(int vertex)
    {
        for (int i = 0; i < Size; i++)
        {
            if (AdjacencyMatrix[vertex,i] > 0)
            {
                yield return i;
            }
        }
    }
    
    public IEnumerable<int> GetInVertices(int vertex)
    {
        for (int i = 0; i < Size; i++)
        {
            if (AdjacencyMatrix[i,vertex] > 0)
            {
                yield return i;
            }
        }
    }

    public static bool operator ==(Graph graph1, Graph graph2)
    {
        if (ReferenceEquals(graph1, graph2))
            return true;

        if (graph1.Size != graph2.Size)
            return false;

        int size = graph1.Size;
        for(int i = 0; i < size; ++i)
        {
            for(int j = 0; j < size; ++j)
            {
                if (graph1.AdjacencyMatrix[i,j] != graph2.AdjacencyMatrix[i,j])
                    return false;
            }
        }
        return true;
    }

    public static bool operator !=(Graph graph1, Graph graph2)
    {
        return !(graph1 == graph2);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Graph other)
            return this == other;
        return false;
    }

    public override int GetHashCode()
    {
        return AdjacencyMatrix.GetHashCode();
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
                graph.AdjacencyMatrix[i,j] = int.Parse(line[j]);
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
                writer.Write(AdjacencyMatrix[i,j]);
                if (j < Size - 1) writer.Write(' ');
            }
            writer.WriteLine();
        }
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                builder.Append(AdjacencyMatrix[i,j]);
                if (j < Size - 1) builder.Append(' ');
            }
            builder.AppendLine();
        }

        return builder.ToString();
    }
}