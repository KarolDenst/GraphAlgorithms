namespace GraphAlgorithms.Graphs
{
    public class GraphLoader
    {
        private int _currentLine;

        private readonly string[] _lines;

        private GraphLoader(string[] lines)
        {
            _lines = lines;
        }

        public static Graph[] Load(string path)
        {
            string[] lines;
            lines = File.ReadAllLines(path);

            GraphLoader loader = new GraphLoader(lines);
            int numberOfGraphs = int.Parse(lines[0], System.Globalization.NumberStyles.None);
            Graph[] graphs = new Graph[numberOfGraphs];

            loader._currentLine = 1;
            for (int i = 0; i < numberOfGraphs; i++)
            {
                graphs[i] = loader.ParseNextGraph();
            }

            return graphs;
        }

        private Graph ParseNextGraph()
        {
            int graphSize = int.Parse(_lines[_currentLine++]);
            Graph graph = new Graph(graphSize);
            for (int i = 0; i < graphSize; i++)
            {
                List<int> values;

                values = _lines[_currentLine++]
                    .Trim()
                    .Split(' ')
                    .Select(x => int.Parse(x, System.Globalization.NumberStyles.None))
                    .ToList();

                if (values.Count != graphSize)
                    throw new ArgumentOutOfRangeException($"Invalid graph definition at ${_currentLine - 1}");

                if (values[i] != 0)
                    throw new ArgumentOutOfRangeException("The graph cannot have loops");

                for (int j = 0; j < graphSize; j++)
                {
                    if (values[j] < 0)
                        throw new ArgumentOutOfRangeException("Adjacency matrix cannot have negative values");

                    graph.AdjacencyMatrix[i, j] = values[j];
                }
            }

            while (_currentLine < _lines.Length
                && _lines[_currentLine].Trim().Length > 0) // skip comments
            {
                _currentLine++;
            }
            while (_currentLine < _lines.Length
                && _lines[_currentLine].Trim().Length == 0) // skip blank lines
            {
                _currentLine++;
            }

            return graph;
        }
    }
}
