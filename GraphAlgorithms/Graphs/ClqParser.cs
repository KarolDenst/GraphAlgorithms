namespace GraphAlgorithms.Graphs
{
    public static class ClqParser
    {
        public static Graph ParseGraph(string clqFilePath)
        {
            var lines = File.ReadLines(clqFilePath);
            List<(int, int)> edges = new();
            int minVertex = int.MaxValue, maxVertex = int.MinValue;

            foreach (var line in lines)
            {
                if (line[0] != 'e')
                    continue;

                var edge = ParseEdge(line);
                edges.Add(edge);

                if (edge.Item1 > maxVertex)
                    maxVertex = edge.Item1;
                if (edge.Item2 > maxVertex)
                    maxVertex = edge.Item2;
                if (edge.Item1 < minVertex)
                    minVertex = edge.Item1;
                if (edge.Item2 < minVertex)
                    minVertex = edge.Item2;
            }

            int graphSize = maxVertex - minVertex + 1;
            Graph graph = new Graph(graphSize);
            foreach (var edge in edges)
            {
                int from = edge.Item1 - minVertex;
                int to = edge.Item2 - minVertex;
                graph.AddBothSidesEdge(from, to); // TODO not sure if that's ok
            }

            return graph;
        }

        private static (int, int) ParseEdge(string line)
        {
            var tokens = line.Split(' ');
            return (int.Parse(tokens[1], System.Globalization.NumberStyles.None),
                int.Parse(tokens[2], System.Globalization.NumberStyles.None));
        }
    }
}
