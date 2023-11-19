using System.CommandLine;

namespace GraphAlgorithms.Commands;

public static class OptionsFactory
{
    public static Option<string> CreateNameOption()
    {
        var nameOption = new Option<string>(name: "--name",
                description: "Name of the benchmark we want to test.",
                getDefaultValue: () => "C125.9");
        nameOption.AddAlias("-n");

        return nameOption;
    }

    public static Option<string> CreateAlgorithmTypeOption()
    {
        var algorithmTypeOption = new Option<string>(name: "--type",
                description: "Type of the algorithm",
                getDefaultValue: () => "heuristic")
            .FromAmong("exact", "heuristic");
        algorithmTypeOption.AddAlias("-t");

        return algorithmTypeOption;
    }

    public static Option<string> CreateCmpOption()
    {
        var cmpOption = new Option<string>(name: "--cmp",
                description: "Graph comparison type",
                getDefaultValue: () => "vertices-then-edges")
            .FromAmong("vertices", "vertices-then-edges");

        return cmpOption;
    }

    public static Option<string> CreateFileOption()
    {
        var fileOption = new Option<string>(name: "--file",
                description: "Path to file with graphs")
        { IsRequired = true };
        fileOption.AddAlias("-f");

        return fileOption;
    }

    public static Option<string> CreateFile2Option()
    {
        var fileOption = new Option<string>(name: "--file2",
            description: "Path to the second file with graphs");
        fileOption.AddAlias("-f2");

        return fileOption;
    }

    public static Option<int> CreateIndexOption()
    {
        var indexOption = new Option<int>(name: "--index",
                description: "Index of the graph in the file",
                getDefaultValue: () => 0);
        indexOption.AddAlias("-i");

        return indexOption;
    }

    public static Option<int> CreateIndex1Option()
    {
        var index1Option = new Option<int>(name: "--index1",
                description: "Index of the 1st graph in the file",
                getDefaultValue: () => 0);
        index1Option.AddAlias("-i1");

        return index1Option;
    }

    public static Option<int> CreateIndex2Option()
    {
        var index2Option = new Option<int>(name: "--index2",
                description: "Index of the 2nd graph in the file",
                getDefaultValue: () => 0);
        index2Option.AddAlias("-i2");

        return index2Option;
    }

    public static Option<string> CreatePyPathOption()
    {
        var pyPathOption = new Option<string>(name: "--pypath",
            description: "Path to Python interpreter");

        return pyPathOption;
    }

    public static Option<string> CreateAlgorithmOption()
    {
        var algorithmOption = new Option<string>(name: "--algorithm",
            description: "Algorithm to execute",
            getDefaultValue: () => "max-clique")
            .FromAmong("mcs", "max-clique");

        return algorithmOption;
    }

    public static Option<double> CreateDensityOption()
    {
        var densityOption = new Option<double>(name: "--density",
            description: "Density of the generated graphs",
            getDefaultValue: () => 0.5);

        return densityOption;
    }

    public static Option<int> CreateMinGraphSizeOption()
    {
        var minGraphSizeOption = new Option<int>(name: "--min-size",
            description: "Minimum number of vertices in the generated graph",
            getDefaultValue: () => 3);

        return minGraphSizeOption;
    }

    public static Option<int> CreateMaxGraphSizeOption()
    {
        var maxGraphSizeOption = new Option<int>(name: "--max-size",
            description: "Maximum number of vertices in tbe generated graph",
            getDefaultValue: () => 100);

        return maxGraphSizeOption;
    }

    public static Option<int> CreateStepOption()
    {
        var stepOption = new Option<int>(name: "--step",
            description: "Difference between numbers of vertices in consecutive graphs",
            getDefaultValue: () => 1);

        return stepOption;
    }

    public static Option<int> CreateSamplesOption()
    {
        var samples = new Option<int>(name: "--samples",
            description: "Number of graphs of the same size to take the average from",
            getDefaultValue: () => 5);

        return samples;
    }

    public static Option<string> CreateAlgorithmTypeOptionForBenchmark()
    {
        var algorithmTypeOption = new Option<string>(name: "--type",
                description: "Type of the algorithm to run the benchmark on. Choose 'both' to run the benchmark for both algorithm types",
                getDefaultValue: () => "both")
            .FromAmong("exact", "heuristic", "both");
        algorithmTypeOption.AddAlias("-t");

        return algorithmTypeOption;
    }
}