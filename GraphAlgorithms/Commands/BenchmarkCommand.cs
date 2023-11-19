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
            algorithmTypeOption.IsRequired = false;
            var cmpOption = OptionsFactory.CreateCmpOption();
            var densityOption = OptionsFactory.CreateDensityOption();
            var minGraphSizeOption = OptionsFactory.CreateMinGraphSizeOption();
            var maxGraphSizeOption = OptionsFactory.CreateMaxGraphSizeOption();
            var stepOption = OptionsFactory.CreateStepOption();
            var samplesOption = OptionsFactory.CreateSamplesOption();

            var benchmarkCommand = new Command("benchmark", "Perform benchmark test on random graphs.")
            {
                algorithmOption,
                algorithmTypeOption,
                cmpOption,
                densityOption,
                minGraphSizeOption,
                maxGraphSizeOption,
                stepOption,
                samplesOption
            };
            benchmarkCommand.SetHandler(Run, algorithmOption, algorithmTypeOption, cmpOption, densityOption, minGraphSizeOption, maxGraphSizeOption, stepOption, samplesOption);

            return benchmarkCommand;
        }

        private static void Run(string alg, string? algType, string cmp, double density, int minSize, int maxSize, int step, int samples)
        {
            if (density < 0 || density > 1)
                throw new ArgumentOutOfRangeException(nameof(density), "Density must be between 0 and 1");
            if (minSize < 0)
                throw new ArgumentOutOfRangeException(nameof(minSize), "Minimum size can't be negative");
            if (maxSize < 0)
                throw new ArgumentOutOfRangeException(nameof(maxSize), "Maximum size can't be negative");
            if (minSize > maxSize)
                throw new ArgumentOutOfRangeException(nameof(minSize), "Minimum size can't be bigger than maximum size");

            Dictionary<double, double>? benchmarkResultExact = null;
            Dictionary<double, double>? benchmarkResultHeuristic = null;
            if (alg == "mcs")
            {
                if (algType != null)
                {
                    if (algType == "exact")
                        benchmarkResultExact = RunMcsBenchmark(algType, cmp, minSize, maxSize, step, density, samples);
                    else if (algType == "heuristic")
                        benchmarkResultHeuristic = RunMcsBenchmark(algType, cmp, minSize, maxSize, step, density, samples);
                    else
                        throw new NotImplementedException();
                }
                else
                {
                    benchmarkResultExact = RunMcsBenchmark("exact", cmp, minSize, maxSize, step, density, samples);
                    benchmarkResultHeuristic = RunMcsBenchmark("heuristic", cmp, minSize, maxSize, step, density, samples);
                }
            }
            else if (alg == "max-clique")
            {
                if (algType != null)
                {
                    if (algType == "exact")
                        benchmarkResultExact = RunMaxCliqueBenchmark(algType, cmp, minSize, maxSize, step, density, samples);
                    else if (algType == "heuristic")
                        benchmarkResultHeuristic = RunMaxCliqueBenchmark(algType, cmp, minSize, maxSize, step, density, samples);
                    else
                        throw new NotImplementedException();
                }
                else
                {
                    benchmarkResultExact = RunMaxCliqueBenchmark("exact", cmp, minSize, maxSize, step, density, samples);
                    benchmarkResultHeuristic = RunMaxCliqueBenchmark("heuristic", cmp, minSize, maxSize, step, density, samples);
                }
            }
            else
                throw new NotImplementedException("No benchmark available for this algorithm");

            string benchmarkName = CreateBenchmarkName();
            if (benchmarkResultExact != null)
                SaveBenchmarkResults(benchmarkResultExact, benchmarkName, "exact");
            if (benchmarkResultHeuristic != null)
                SaveBenchmarkResults(benchmarkResultHeuristic, benchmarkName, "heuristic");

            PlotResults(benchmarkName);
        }

        private static void PlotResults(string benchmarkName)
        {
            if (!File.Exists(Locations.PythonSettingsFilePath))
                throw new FileNotFoundException("Python not configured properly");

            string pythonSettingsJson = File.ReadAllText(Locations.PythonSettingsFilePath);
            PythonSettings? pythonSettings = JsonSerializer.Deserialize<PythonSettings>(pythonSettingsJson)
                ?? throw new Exception("Python not configured properly");

            if (!File.Exists(pythonSettings.Path))
                throw new FileNotFoundException("Python interpreter not found");

            string benchmarkDir = Path.Combine(Locations.BenchmarkResultsDir, benchmarkName);
            var process = new Process();
            process.StartInfo.FileName = pythonSettings.Path;
            process.StartInfo.Arguments = string.Format("{0} {1}", Locations.PlotterPath, benchmarkDir);
            process.StartInfo.UseShellExecute = false;

            process.Start();
            process.WaitForExit();
        }

        private static Dictionary<double, double> RunMaxCliqueBenchmark(string algType, string cmp, int minSize, int maxSize, int step, double density, int samples)
        {
            ICliqueFastFinder finder = GetFinder(algType);

            Action<Graph> algorithm = cmp switch
            {
                "vertices" => (g) => finder.Find(g),
                "vertices-then-edges" => (g) => finder.FindWithEdges(g),
                _ => throw new NotImplementedException(),
            };

            Stopwatch stopwatch = new();
            Dictionary<double, double> result = new();

            for (int s = 0; s < samples; s++)
            {
                var graphs = GenerateRandomGraphs(minSize, maxSize, step, density, s);
                foreach (var graph in graphs)
                {
                    stopwatch.Restart();
                    algorithm.Invoke(graph);
                    stopwatch.Stop();
                    if (!result.ContainsKey(graph.Size))
                        result.Add(graph.Size, (double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1e9);
                    else
                        result[graph.Size] += (double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1e9;
                }
            }

            var averagedResult = result.ToDictionary(kv => kv.Key, kv => kv.Value / samples);

            return averagedResult;
        }

        private static Dictionary<double, double> RunMcsBenchmark(string algType, string cmp, int minSize, int maxSize, int step, double density, int samples)
        {


            ICliqueFastFinder finder = GetFinder(algType);
            bool withEdges = cmp == "vertices-then-edges";

            Action<Graph, Graph> algorithm = (g1, g2) => MCSFinder.FindFast(g1, g2, finder, withEdges);

            Stopwatch stopwatch = new();
            Dictionary<double, double> result = new();

            for (int s = 0; s < samples; s++)
            {
                var graphs1 = GenerateRandomGraphs(minSize, maxSize, step, density, s);
                var graphs2 = GenerateRandomGraphs(minSize, maxSize, step, density, s);
                for (int i = 0; i < graphs1.Count; i++)
                {
                    var graph1 = graphs1[i];
                    var graph2 = graphs2[i];
                    stopwatch.Restart();
                    algorithm.Invoke(graph1, graph2);
                    stopwatch.Stop();
                    if (!result.ContainsKey(graph1.Size))
                        result.Add(graph1.Size, (double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1e9);
                    else
                        result[graph1.Size] += (double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1e9;
                }
            }

            var averagedResult = result.ToDictionary(kv => kv.Key, kv => kv.Value / samples);

            return averagedResult;
        }

        private static List<Graph> GenerateRandomGraphs(int minSize, int maxSize, int step, double density, int seed)
        {
            var graphs = new List<Graph>();
            for (int size = minSize; size <= maxSize; size += step)
            {
                graphs.Add(GraphFactory.CreateRandom(size, density, seed));
            }

            return graphs;
        }

        private static ICliqueFastFinder GetFinder(string algType) => algType switch
        {
            "heuristic" => new MaxCliqueHeuFinder(),
            "exact" => new MaxCliqueExactFinder(),
            _ => throw new NotImplementedException()
        };

        private static string SaveBenchmarkResults(Dictionary<double, double> benchmarkResults, string benchmarkName, string type)
        {
            var serializerOptions = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(benchmarkResults, serializerOptions);
            string benchmarkDir = Path.Combine(Locations.BenchmarkResultsDir, benchmarkName);
            Directory.CreateDirectory(benchmarkDir);
            string benchmarkPath = Path.Combine(benchmarkDir, type) + ".json";
            File.WriteAllText(benchmarkPath, jsonString);

            return benchmarkPath;
        }

        private static string CreateBenchmarkName() => DateTime.Now.ToString("MM-dd-yy_HH-mm-ss");
    }
}
