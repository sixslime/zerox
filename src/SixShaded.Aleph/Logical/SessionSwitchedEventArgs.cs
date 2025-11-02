namespace SixShaded.Aleph.Logical;

internal class SessionSwitchedEventArgs : EventArgs
{
    public required int Index { get; init; }
    public required Session Session { get; init; }
}