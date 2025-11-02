namespace SixShaded.Aleph.ICLI.State;

using Logical;

internal record SessionInfo
{
    public required int SessionIndex { get; init; }
    public IOption<SelectionPromptedEventArgs> AwaitingSelection { get; init; } = new None<SelectionPromptedEventArgs>();
    public IOption<IProgressor> CurrentProgressor { get; init; } = new None<IProgressor>();
    public int SelectedOperationIndex { get; init; } = 0;
    public OperationExpansion SelectedOperationExpansion { get; init; } = new();
}