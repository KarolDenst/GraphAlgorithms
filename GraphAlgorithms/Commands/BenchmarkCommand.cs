using GraphAlgorithms.Clique;
using GraphAlgorithms.Common;
using GraphAlgorithms.Graphs;
using GraphAlgorithms.MCS;
using System.CommandLine;
using System.Diagnostics;
using System.Text.Json;

namespace GraphAlgorithms.Commands
{
    internal static class BenchmarkCommand
    {
        public static Command Create()
        {
            var algorithmOption = OptionsFactory.CreateAlgorithmOption();
            var algorithmTypeOption = OptionsFactory.CreateAlgorithmTypeOption();
            var cmpOption = OptionsFactory.CreateCmpOption();
            var densityOption = OptionsFactory.CreateDensityOption();
            var minGraphSizeOption = OptionsFactory.CreateMinGraphSizeOption();
            var maxGraphSizeOption = OptionsFactory.CreateMaxGraphSizeOption();
            var stepOption = OptionsFactory.CreateStepOption();

            var benchmarkCommand = new Command("benchmark", "Perform benchmark test on random graphs.")
            {
                algorithmOption,
                algorithmTypeOption,
                cmpOption,
                densityOption,
                minGraphSizeOption,
                maxGraphSizeOption,
                stepOption
            };
            benchmarkCommand.SetHandler(Run, algorithmOption, algorithmTypeOption, cmpOption, densityOption, minGraphSizeOption, maxGraphSizeOption, stepOption);

            return benchmarkCommand;
        }

        private static void Run(string alg, string algType, string cmp, double density, int minSize, int maxSize, int step)
        {
            Dictionary<int, double> benchmarkResult;
            if (alg == "mcs")
            {
                var graphs1 = GenerateRandomGraphs(minSize, maxSize, step, density);
                var graphs2 = GenerateRandomGraphs(minSize, maxSize, step, density);

                ICliqueFastFinder finder = GetFinder(algType);
                bool withEdges = cmp == "vertices-then-edges";

                Action<Graph, Graph> algorithm = (g1, g2) => MCSFinder.FindFast(g1, g2, finder, withEdges);
                benchmarkResult = RunMcsBenchmark(graphs1, graphs2, algorithm);
            }
            else if (alg == "max-clique")
            {
                var graphs = GenerateRandomGraphs(minSize, maxSize, step, density);

                ICliqueFastFinder finder = GetFinder(algType);

                Action<Graph> algorithm = cmp switch
                {
                    "vertices" => (g) => finder.Find(g),
                    "vertices-then-edges" => (g) => finder.FindWithEdges(g),
                    _ => throw new NotImplementedException(),
                };

                benchmarkResult = RunMaxCliqueBenchmark(graphs, algorithm);
            }
            else
                throw new NotImplementedException("No benchmark available for this algorithm");

            string benchmarkPath = SaveBenchmarkResults(benchmarkResult);
            PlotResults(benchmarkPath);
        }

        private static void PlotResults(string benchmarkPath)
        {
            if (!File.Exists(Locations.PythonSettingsFilePath))
                throw new FileNotFoundException("Python not configured properly");

            string pythonSettingsJson = File.ReadAllText(Locations.PythonSettingsFilePath);
            PythonSettings? pythonSettings = JsonSerializer.Deserialize<PythonSettings>(pythonSettingsJson)
                ?? throw new Exception("Python not configured properly");

            if (!File.Exists(pythonSettings.Path))
                throw new FileNotFoundException("Python interpreter not found");

            var process = new Process();
            process.StartInfo.FileName = pythonSettings.Path;
            process.StartInfo.Arguments = string.Format("{0} {1}", Locations.PlotterPath, benchmarkPath);
            process.StartInfo.UseShellExecute = false;

            process.Start();
            process.WaitForExit();
        }

        private static Dictionary<int, double> RunMaxCliqueBenchmark(List<Graph> graphs, Action<Graph> algorithm)
        {
            Stopwatch stopwatch = new();
            Dictionary<int, double> result = new();
            foreach (var graph in graphs)
            {
                stopwatch.Restart();
                algorithm.Invoke(graph);
                stopwatch.Stop();
                result.Add(graph.Size, stopwatch.Elapsed.TotalMilliseconds);
            }

            return result;
        }

        private static Dictionary<int, double> RunMcsBenchmark(List<Graph> graphs1, List<Graph> graphs2, Action<Graph, Graph> algorithm)
        {
            Stopwatch stopwatch = new();
            Dictionary<int, double> result = new();
            for (int i = 0; i < graphs1.Count; i++)
            {
                var graph1 = graphs1[i];
                var graph2 = graphs2[i];
                stopwatch.Restart();
                algorithm.Invoke(graph1, graph2);
                stopwatch.Stop();
                result.Add(graph1.Size, stopwatch.Elapsed.TotalMilliseconds);
            }

            return result;
        }

        private static List<Graph> GenerateRandomGraphs(int minSize, int maxSize, int step, double density)
        {
            var graphs = new List<Graph>();
            for (int size = minSize; size <= maxSize; size += step)
            {
                graphs.Add(GraphFactory.CreateRandom(size, density));
            }

            return graphs;
        }

        private static ICliqueFastFinder GetFinder(string algType) => algType switch
        {
            "heuristic" => new MaxCliqueHeuFinder(),
            "exact" => new MaxCliqueExactFinder(),
            _ => throw new NotImplementedException()
        };

        private static string SaveBenchmarkResults(Dictionary<int, double> benchmarkResults)
        {
            var serializerOptions = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(benchmarkResults, serializerOptions);
            string benchmarkName = DateTime.Now.ToString("MM-dd-yy_HH-mm-ss") + ".json";
            Directory.CreateDirectory(Locations.BenchmarkResultsDir);
            string benchmarkPath = Path.Combine(Locations.BenchmarkResultsDir, benchmarkName);
            File.WriteAllText(benchmarkPath, jsonString);

            return benchmarkPath;
        }
    }
}
