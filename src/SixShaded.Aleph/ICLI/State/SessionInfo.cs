namespace SixShaded.Aleph.ICLI.State;

using Logical;

internal record SessionInfo
{
    public required int SessionIndex { get; init; }
    public ESessionUIContext UIContext { get; init; } = new ESessionUIContext.TopLevel();
    public IOption<SelectionPromptedEventArgs> AwaitingSelection { get; init; } = new None<SelectionPromptedEventArgs>();
    public IOption<Progressor> CurrentProgressor { get; init; } = new None<Progressor>();
    public int SelectedOperationIndex { get; init; } = 0;
    public OperationExpansion SelectedOperationExpansion { get; init; } = new();

    public Session GetLogicalSession() => Master.Instance.Sessions.At(SessionIndex).Expect($"No logical session at index {SessionIndex}?");

}