namespace GraphAlgorithms.Common
{
    internal static class Locations
    {
        public static readonly string ScriptsDirPath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName, "Scripts");

        public static readonly string PlotterPath = Path.Combine(ScriptsDirPath, "plotter.py");

        public static readonly string ConfigDirPath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName, "Config");

        public static readonly string PythonSettingsFilePath = Path.Combine(ConfigDirPath, "PythonSettings.json");

        public static readonly string BenchmarkResultsDir = Path.Combine(Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName, "Benchmarks");
    }
}
