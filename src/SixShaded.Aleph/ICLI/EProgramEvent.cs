namespace SixShaded.Aleph.ICLI;

internal abstract record EProgramEvent
{
    public sealed record WriteText : EProgramEvent
    {
        public required ConsoleText Text { get; init; }
    }
    public sealed record KeyPressed : EProgramEvent
    {
        public required ConsoleKeyInfo KeyInfo { get; init; }
    }

    public sealed record StopProgram : EProgramEvent;

    public sealed record NewSessionRequest : EProgramEvent
    {
        public required IStateFZO RootState { get; init; }
    }

    public sealed record SessionSwitched : EProgramEvent
    {
        public required Logical.SessionSwitchedEventArgs Args { get; init; }
    }

    public sealed record SessionAdded : EProgramEvent
    {
        public required Logical.SessionAddedEventArgs Args { get; init; }
    }

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