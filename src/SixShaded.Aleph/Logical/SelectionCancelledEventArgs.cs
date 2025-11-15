namespace SixShaded.Aleph.Logical;

internal class SelectionCancelledEventArgs : EventArgs
{
    public required SelectionPromptedEventArgs OriginalArgs { get; init; }

}