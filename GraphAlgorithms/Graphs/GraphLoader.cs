namespace GraphAlgorithms.Graphs
{
    public class GraphLoader
    {
        private int currentLine;

        private readonly string[] lines;

        private GraphLoader(string[] lines)
        {
            this.lines = lines;
        }

        public static Graph[] Load(string path)
        {
            string[] lines;
            lines = File.ReadAllLines(path);

            GraphLoader loader = new GraphLoader(lines);
            int numberOfGraphs = int.Parse(lines[0], System.Globalization.NumberStyles.None);
            Graph[] graphs = new Graph[numberOfGraphs];

            loader.currentLine = 1;
            for (int i = 0; i < numberOfGraphs; i++)
            {
                graphs[i] = loader.ParseNextGraph();
            }

            return graphs;
        }

        private Graph ParseNextGraph()
        {
            int graphSize = int.Parse(lines[currentLine++]);
            Graph graph = new Graph(graphSize);
            for (int i = 0; i < graphSize; i++)
            {
                List<int> values;

                values = lines[currentLine++]
                    .Trim()
                    .Split(' ')
                    .Select(x => int.Parse(x, System.Globalization.NumberStyles.None))
                    .ToList();

                if (values.Count != graphSize)
                {
                    throw new ArgumentOutOfRangeException($"Invalid graph definition at ${currentLine - 1}");
                }

                for (int j = 0; j < graphSize; j++)
                {
                    graph.AdjacencyMatrix[i, j] = values[j];
                }
            }

            while (currentLine < lines.Length && lines[currentLine++].Trim().Length > 0)
                ;

            return graph;
        }
    }
}
