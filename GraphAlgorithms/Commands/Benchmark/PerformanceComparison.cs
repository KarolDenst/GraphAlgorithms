namespace GraphAlgorithms.Commands.Benchmark;

public record PerformanceComparison(
    int? Size,
    long? HeuristicVertexTime,
    long? HeuristicVertexEdgeTime,
    long? ExactVertexTime,
    long? ExactVertexEdgeTime,
    long? NaiveTime
);
