namespace SixShaded.Aleph.Logical;

internal class Master
{
    private static Master? _instance;
    public static bool IsInitialized => _instance is not null;
    public static Master Instance => _instance ?? throw new("Master object not initialized.");
    public required Language.ILanguageKey LanguageKey { get; init; }
    public required IProcessorFZO Processor { get; init; }
    public IPSequence<Session> Sessions { get; private set; } = new PSequence<Session>();

    public event EventHandler<SessionAddedEventArgs>? SessionAddedEvent;

    public int AddSession(IStateFZO rootState)
    {
        Sessions = Sessions.WithEntries(new Session()
        {
            Processor = Processor,
            Root = rootState,
        });
        NotifyAddSession();
        return Sessions.Count - 1;
    }

    internal static void Init(AlephArgs args)
    {
        if (_instance is not null)
            throw new("Master already initialized.");
        _instance =
            new()
            {
                LanguageKey = args.LanguageKey,
                Processor = args.Processor,
            };
    }

    private void NotifyAddSession() =>
        SessionAddedEvent?.Invoke(
        this, new()
        {
            Index = Sessions.Count - 1,
            Session = Sessions.At(Sessions.Count-1).Unwrap()
        });
    internal record InitArgs
    {
        public required Language.ILanguageKey LanguageKey { get; init; }
        public required IProcessorFZO Processor { get; init; }
    }
}