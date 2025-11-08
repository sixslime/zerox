namespace SixShaded.Aleph.Logical;

public record Progressor
{
    public required string Name { get; init; }
    public required string StopConditionDescription { get; init; }
    public required Func<IProgressionContext, Task> Function { get; init; }
}