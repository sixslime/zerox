namespace SixShaded.Aleph.Logical;

internal class SessionAddedEventArgs : EventArgs
{
    public required Session Session { get; init; }
}