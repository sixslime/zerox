namespace SixShaded.Aleph.Logical;

public class TrackpointUpdatedEventArgs : EventArgs
{
    public required Trackpoint NewTrackpoint { get; init; }
}