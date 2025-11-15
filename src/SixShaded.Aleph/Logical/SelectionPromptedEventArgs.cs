namespace SixShaded.Aleph.Logical;

internal class SelectionPromptedEventArgs : EventArgs
{
    public required Rog[] Pool { get; init; }
    public required int MinCount { get; init; }
    public required int MaxCount { get; init; }
    public required ISelectionCallback Callback { get; init; }

}