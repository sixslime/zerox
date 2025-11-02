namespace SixShaded.Aleph.ICLI.State;

using Logical;

internal record SessionInfo
{
    public IOption<ISelectionCallback> AwaitingSelection { get; init; } = new None<ISelectionCallback>();
    public IOption<IProgressor> CurrentProgressor { get; init; } = new None<IProgressor>();
    public int SelectedOperationIndex { get; init; } = 0;
}