namespace SixShaded.Aleph.ICLI;

internal abstract record EProgramEvent
{

    public sealed record TrackpointUpdated : EProgramEvent
    {
        public required Logical.Session Source { get; init; }
        public required Logical.TrackpointUpdatedEventArgs Args { get; init; }
    }

    public sealed record SelectionPrompted : EProgramEvent
    {
        public required Logical.Session Source { get; init; }
        public required Logical.SelectionPromptedEventArgs Args { get; init; }
    }

    public sealed record SelectionCancelled : EProgramEvent
    {
        public required Logical.Session Source { get; init; }
        public required Logical.SelectionCancelledEventArgs Args { get; init; }
    }

    public sealed record SelectionSend : EProgramEvent
    {
        public required int[] Selection { get; init; }
    }
    
}