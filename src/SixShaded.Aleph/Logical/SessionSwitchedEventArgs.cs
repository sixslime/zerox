namespace SixShaded.Aleph.Logical;

internal class SessionSwitchedEventArgs : EventArgs
{
    public required Session Session { get; init; }
}