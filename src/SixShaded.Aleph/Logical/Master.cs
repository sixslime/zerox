namespace SixShaded.Aleph.Logical;

internal class Master
{
    private static Master? _instance;
    private int _sessionIndex = -1;
    public static Master Instance => _instance ?? throw new("Master object not initialized.");
    public required Language.LanguageProvider LanguageProvider { get; init; }
    public required IProcessorFZO Processor { get; init; }
    public IPSequence<Session> Sessions { get; private set; } = new PSequence<Session>();
    public IOption<Session> CurrentSession => Sessions.At(_sessionIndex);
    public event EventHandler<SessionSwitchedEventArgs>? SessionSwitchedEvent;

    public int AddSession(IStateFZO rootState)
    {
        Sessions = Sessions.WithEntries(session);
        return Sessions.Count - 1;
    }

    public bool SwitchSession(int index)
    {
        if (index > Sessions.Count - 1) return false;
        _sessionIndex = index;
        NotifySwitchSession();
        return true;
    }

    internal static void Init(InitArgs args)
    {
        if (_instance is not null)
            throw new("Master already initialized.");
        _instance =
            new()
            {
                LanguageProvider = args.LanguageProvider,
                Processor = args.Processor,
            };
    }

    private void NotifySwitchSession() =>
        SessionSwitchedEvent?.Invoke(
        this, new()
        {
            Session = CurrentSession.Unwrap(),
        });

    internal record InitArgs
    {
        public required Language.LanguageProvider LanguageProvider { get; init; }
        public required IProcessorFZO Processor { get; init; }
    }
}