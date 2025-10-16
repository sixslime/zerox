namespace SixShaded.Aleph.Logical;

public class SessionSwitchedEventArgs : EventArgs
{
    public required Session Session { get; init; }
}