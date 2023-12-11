namespace GraphAlgorithms.Common
{
    internal static class Locations
    {
        public static readonly string ScriptsDirPath = "Scripts";

        public static readonly string PlotterPath = Path.Combine(ScriptsDirPath, "plotter.py");

        public static readonly string ConfigDirPath = "Config";

        public static readonly string PythonSettingsFilePath = "PythonSettings.json";

        public static readonly string BenchmarkResultsDir = "Benchmarks";
    }
}
