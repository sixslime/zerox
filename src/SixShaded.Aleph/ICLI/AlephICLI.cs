namespace SixShaded.Aleph.ICLI;

using Logical;
using Language;
using MinimaFZO;
using System.Threading.Channels;

public static class AlephICLI
{
    private static IInputHandler? _inputHandler;
    private static ReaderWriterLock? _stateLock;
    private static ProgramState? _programState;
    private static MasterListener? _masterListener;
    private static HashSet<SessionListener>? _sessionListeners;

    private static Channel<EProgramEvent>? _eventsChannel;
        

    internal static ChannelWriter<EProgramEvent> EventWriter => _eventsChannel!.Writer;
    internal static ProgramState ProgramState
    {
        get
        {
            if (_programState == null) throw new("ProgramState not yet initialized.");
            _stateLock!.AcquireReaderLock(500);
            try { return _programState; }
            finally { _stateLock.ReleaseReaderLock(); }
        }
        private set
        {
            _stateLock!.AcquireWriterLock(100);
            try { _programState = value; }
            finally { _stateLock.ReleaseWriterLock(); }
        }
    }

    public static IAlephICLIHandle Run(AlephArgs args)
    {
        Init(args);
        Task.Run(ProgramLoop);
        return new ICLIHandleObject();
    }

    internal static void FireEventAndForget(EProgramEvent programEvent)
    {
        Task.Run(async () => await EventWriter.WriteAsync(programEvent));
    }
    private static void Init(AlephArgs args)
    {
        if (Master.IsInitialized)
            throw new("Aleph is already initialized.");
        Master.Init(
        new()
        {
            LanguageKey = args.LanguageKey,
            Processor = args.Processor,
        });
        _masterListener = new(Master.Instance);
        _sessionListeners = new(3);
        _stateLock = new();
        _programState = new();
        _eventsChannel = Channel.CreateUnbounded<EProgramEvent>(
        new()
        {
            AllowSynchronousContinuations = true,
            SingleReader = true,
            SingleWriter = false,
        });
        KeyReader.Start();
    }

    private static async Task ProgramLoop()
    {
        bool exit = false;
        while (!exit)
        {
            var reader = _eventsChannel!.Reader;
            if (!await reader.WaitToReadAsync())
            {
                exit = true;
                break;
            }
            await foreach (var currentEvent in reader.ReadAllAsync())
            {
                switch (currentEvent)
                {
                case EProgramEvent.StopProgram:
                    exit = true;
                    break;
                case EProgramEvent.NewSessionRequest args:
                    await HandleNewSessionRequest(args);
                    break;
                case EProgramEvent.KeyPressed args:
                    await HandleKeyPressed(args);
                    break;
                case EProgramEvent.SelectionSend args:
                    await HandleSelectionSend(args);
                    break;
                case EProgramEvent.WriteText args:
                    await HandleWriteText(args);
                    break;
                case EProgramEvent.SelectionPrompted args:
                    await HandleSelectionPrompted(args);
                    break;
                case EProgramEvent.SelectionCancelled args:
                    await HandleSelectionCancelled(args);
                    break;
                case EProgramEvent.TrackpointUpdated args:
                    await HandleTrackpointUpdated(args);
                    break;
                case EProgramEvent.SessionAdded args:
                    await HandleSessionAdded(args);
                    break;
                case EProgramEvent.SessionSwitched args:
                    await HandleSessionSwitched(args);
                    break;
                default:
                    exit = true;
                    Console.WriteLine($"Encountered unknown program event: {currentEvent}");
                    break;
                }
                if (exit) break;
            }
        }
        await Shutdown();
    }

    private static async Task HandleWriteText(EProgramEvent.WriteText args)
    {
        foreach (var segment in args.Text.Segments)
        {
            if (segment.Foreground is not null) Console.ForegroundColor = (ConsoleColor)segment.Foreground;
            if (segment.Background is not null) Console.BackgroundColor = (ConsoleColor)segment.Background;
            string prefix = (segment.Bold ? "\x1b[1m" : "") + (segment.Underline ? "\x1b[4m" : "");
            string suffix = (segment.Bold ? "\x1b[0m" : "") + (segment.Underline ? "\x1b[0m" : "");
            await Console.Out.WriteAsync($"{prefix}{segment.Text}{suffix}");
        }
        Console.ResetColor();

    }

    private static async Task HandleKeyPressed(EProgramEvent.KeyPressed args)
    {
        if (_inputHandler is null) return;
        await _inputHandler.RecieveInput(args.KeyInfo);
    }

    private static async Task HandleNewSessionRequest(EProgramEvent.NewSessionRequest args)
    { }

    private static async Task HandleSelectionSend(EProgramEvent.SelectionSend args)
    { }

    private static async Task HandleSelectionPrompted(EProgramEvent.SelectionPrompted args)
    {

    }
    private static async Task HandleSelectionCancelled(EProgramEvent.SelectionCancelled args)
    {

    }
    private static async Task HandleTrackpointUpdated(EProgramEvent.TrackpointUpdated args)
    {

    }
    private static async Task HandleSessionAdded(EProgramEvent.SessionAdded args)
    {
        _sessionListeners!.Add(new(args.Args.Session));
    }
    private static async Task HandleSessionSwitched(EProgramEvent.SessionSwitched args)
    {

    }
    private static async Task Shutdown()
    {
        KeyReader.Stop();
        _masterListener!.Dispose();
        foreach (var listener in _sessionListeners!) listener.Dispose();
        _eventsChannel!.Writer.TryComplete();
        _eventsChannel = null;
        _masterListener = null;
        _sessionListeners = null;
    }

    private class ICLIHandleObject : IAlephICLIHandle
    {
        public async Task AddSession(IStateFZO rootState) =>
            await EventWriter.WriteAsync(
            new EProgramEvent.NewSessionRequest
            {
                RootState = rootState,
            });

        public async Task Stop() => await EventWriter.WriteAsync(new EProgramEvent.StopProgram());
    }

    
}

internal record ProgramState
{ }

internal interface IInputHandler
{
    public Task RecieveInput(ConsoleKeyInfo key);
}