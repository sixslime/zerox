namespace SixShaded.Aleph.ICLI;

using System.Runtime.InteropServices;
using Logical;
using Language;
using MinimaFZO;
using System.Threading.Channels;
using ProgramEvents;
using State;

public static class AlephICLI
{
    private static readonly ReaderWriterLock PROGRAM_LOCK = new();
    private static RunningFields? _runData;
    private static TaskCompletionSource? _terminationCompletionSource;

    private static RunningFields _program
    {
        get
        {
            PROGRAM_LOCK.AcquireReaderLock(500);
            try { return _program; }
            finally { PROGRAM_LOCK.ReleaseReaderLock(); }
        }
        set
        {
            PROGRAM_LOCK.AcquireWriterLock(100);
            try { _program = value; }
            finally { PROGRAM_LOCK.ReleaseWriterLock(); }
        }
    }

    private static ChannelWriter<IProgramEvent> _eventWriter => _program.EventsChannel.Writer;
    private static ChannelReader<IProgramEvent> _eventReader => _program.EventsChannel.Reader;

    public static IAlephICLIHandle Run(AlephArgs args)
    {
        Init(args);
        Task.Run(ProgramLoop);
        return new ICLIHandle();
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
        _runData =
            new()
            {
                InputHandler = null!, // TODO
                MasterListener = MasterListener.Link(ProgramContext.Instance, Master.Instance),
                SessionListeners = new(3),
                State = new(),
                EventsChannel =
                    Channel.CreateUnbounded<IProgramEvent>(
                    new()
                    {
                        AllowSynchronousContinuations = true,
                        SingleReader = true,
                        SingleWriter = false,
                    }),
                KeyReader = KeyReader.Link(ProgramContext.Instance, 50),
            };
        _terminationCompletionSource = new();
    }

    private static async Task ProgramLoop()
    {
        bool exit = false;
        while (!exit)
        {
            if (!await _eventReader.WaitToReadAsync())
            {
                exit = true;
                break;
            }
            await foreach (var currentEvent in _eventReader.ReadAllAsync())
            {
                await currentEvent.Handle(ProgramContext.Instance);
                if (_program.TerminationRequested) exit = true;
                if (exit) break;
            }
        }
        Shutdown();
    }

    private static void Shutdown()
    {
        _program.Dispose();
        _runData = null;
        _terminationCompletionSource!.TrySetResult();
    }

    private class RunningFields : IDisposable
    {
        public required IInputHandler InputHandler { get; set; }
        public required MasterListener MasterListener { get; set; }
        public required KeyReader KeyReader { get; set; }
        public required ProgramState State { get; set; }
        public required HashSet<SessionListener> SessionListeners { get; set; }
        public required Channel<IProgramEvent> EventsChannel { get; set; }
        public bool TerminationRequested { get; set; }

        public void Dispose()
        {
            MasterListener.Dispose();
            KeyReader.Dispose();
            foreach (var listener in SessionListeners) listener.Dispose();
            EventsChannel.Writer.TryComplete();
        }
    }

    private class ICLIHandle : IAlephICLIHandle
    {
        public static ICLIHandle Instance { get; } = new();

        public void AddSession(IStateFZO rootState) =>
            ProgramContext.Instance.SendEvent(
            new AddSession
            {
                RootState = rootState,
            });

        public async Task Stop()
        {
            _program.TerminationRequested = true;
            await _terminationCompletionSource!.Task;
        }
    }

    private class ProgramContext : IProgramContext
    {
        public static ProgramContext Instance { get; } = new();

        public ProgramState State
        {
            get => _program.State;
            set => _program.State = value;
        }

        public void SendEvent(IProgramEvent action)
        {
            if (_eventWriter.TryWrite(action)) return;
            Task.Run(async () => await _eventWriter.WriteAsync(action).ConfigureAwait(false));
        }

        public void SendTerminationRequest() => _program.TerminationRequested = true;
    }
}

internal interface IInputHandler
{
    public Task RecieveInput(ConsoleKeyInfo key);
}