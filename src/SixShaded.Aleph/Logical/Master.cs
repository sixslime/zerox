namespace SixShaded.Aleph.Logical;

internal class Master
{
    private static Master? _instance;
    public int SessionIndex { get; private set; } = -1;
    public static bool IsInitialized => _instance is not null;
    public static Master Instance => _instance ?? throw new("Master object not initialized.");
    public required Language.ILanguageKey LanguageKey { get; init; }
    public required IProcessorFZO Processor { get; init; }
    public IPSequence<Session> Sessions { get; private set; } = new PSequence<Session>();
    public IOption<Session> CurrentSession => SessionIndex >= 0 ? Sessions.At(SessionIndex) : new None<Session>();
    public event EventHandler<SessionSwitchedEventArgs>? SessionSwitchedEvent;
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

    public bool SwitchSession(int index)
    {
        if (index > Sessions.Count - 1) return false;
        SessionIndex = index;
        NotifySwitchSession();
        return true;
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

    private void NotifySwitchSession() =>
        SessionSwitchedEvent?.Invoke(
        this, new()
        {
            Session = CurrentSession.Unwrap(),
        });
    private void NotifyAddSession() =>
        SessionAddedEvent?.Invoke(
        this, new()
        {
            Session = Sessions.At(Sessions.Count-1).Unwrap()
        });
    internal record InitArgs
    {
        public required Language.ILanguageKey LanguageKey { get; init; }
        public required IProcessorFZO Processor { get; init; }
    }
}