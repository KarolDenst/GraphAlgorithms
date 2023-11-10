using System.CommandLine;

namespace GraphAlgorithms.Commands;

public static class BasicCommandFactory
{
    public static Option<string> CreateNameOption()
    {
        var nameOption = new Option<string>(name: "--name",
                description: "Name of the benchmark we want to test.")
            { IsRequired = true };
        nameOption.AddAlias("-n");

        return nameOption;
    }
    
    public static Option<string> CreateAlgorithmTypeOption()
    {
        var algorithmTypeOption = new Option<string>(name: "--type",
                description: "Type of the algorithm")
            { IsRequired = true }
            .FromAmong("exact", "heuristic");
        algorithmTypeOption.AddAlias("-t");

        return algorithmTypeOption;
    }
    
    public static Option<string> CreateFileOption()
    {
        var fileOption = new Option<string>(name: "--file",
                description: "Path to file with graphs")
            { IsRequired = true };
        fileOption.AddAlias("-f");

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
    
    public static Option<string> CreateCmpOption()
    {
        var cmpOption = new Option<string>(name: "--cmp",
                description: "Graph comparison type",
                getDefaultValue: () => "vertices")
            .FromAmong("vertices", "vertices-then-edges");

        return cmpOption;
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
                getDefaultValue: () => 1);
        index2Option.AddAlias("-i2");

        return index2Option;
    }
}