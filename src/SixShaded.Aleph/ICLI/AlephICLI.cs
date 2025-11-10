namespace SixShaded.Aleph.ICLI;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Logical;
using Language;
using MinimaFZO;
using System.Threading.Channels;
using Config;
using ProgramEvents;
using State;
using Formatting;
using InputHandlers;

public static class AlephICLI
{
    private static RunningFields? _runData;
    private static TaskCompletionSource? _terminationCompletionSource;
    private static Task? _runningTask;
    private static RunningFields _program => _runData!;
    private static ChannelWriter<IProgramEvent> _eventWriter => _program.EventsChannel.Writer;
    private static ChannelReader<IProgramEvent> _eventReader => _program.EventsChannel.Reader;

    public static IAlephICLIHandle Run(AlephArgs args)
    {
        Init(args);
        _runningTask = Task.Run(ProgramLoop);
        return new ICLIHandle();
    }

    private static void Init(AlephArgs args)
    {
        if (Master.IsInitialized)
            throw new("Aleph is already initialized.");
        Console.OutputEncoding = Encoding.Unicode;
        Master.Init(
        new()
        {
            LanguageKey = args.LanguageKey,
            Processor = args.Processor,
        });
        _runData =
            new()
            {
                MasterListener = MasterListener.Link(EventSender.Instance, Master.Instance),
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
                KeyReader = KeyReader.Link(EventSender.Instance, 50),
                InputHandlers =
                [
                    new LimboInputHandler(),
                    new ProgressorSelectInputHandler(),
                    new SessionInspectInputHandler(),
                ],
            };
        _terminationCompletionSource = new();
    }

    private static async Task ProgramLoop()
    {
        bool exit = false;
        ConsoleText.Text("Waiting for session...\n")
            .Format(TextFormat.Info)
            .Print();
        UpdateInputProtocol();
        while (!exit)
        {
            try
            {
                if (!await _eventReader.WaitToReadAsync())
                {
                    exit = true;
                    break;
                }
            }
            catch (TaskCanceledException)
            {
                exit = true;
                break;
            }
            await foreach (var currentEvent in _eventReader.ReadAllAsync())
            {
                await currentEvent.Handle(ProgramActions.Instance);
                UpdateInputProtocol();
                if (_program.TerminationRequested) exit = true;
                if (exit) break;
            }
        }
        DoShutdown();
    }

    private static void InitiateShutdown()
    {
        _program.TerminationRequested = true;
        _program.EventsChannel.Writer.TryComplete();
    }

    private static void DoShutdown()
    {
        _program.Dispose();
        _runData = null;
        _terminationCompletionSource!.TrySetResult();
    }

    private static void UpdateInputProtocol()
    {
        EInputProtocol? inputProtocol = null;
        foreach (var handler in _program.InputHandlers)
        {
            if (inputProtocol is not null || handler.ShouldHandle(_program.State).CheckNone(out var prot))
            {
                handler.Tick(false, _program.State);
                continue;
            }
            inputProtocol = prot;
            handler.Tick(true, _program.State);
        }
        _program.CurrentInputProtocol = inputProtocol;
        if (inputProtocol is null)
        {
            ConsoleText.Text("No input handler matches current state?\n")
                .Format(
                TextFormat.Error with
                {
                    Underline = true,
                })
                .Print();
        }
    }

    private static void HandleInput(AlephKeyPress key, IProgramActions actions)
    {
        switch (_program.CurrentInputProtocol)
        {
        case EInputProtocol.Direct p:
            p.DirectAction(key, actions);
            break;
        case EInputProtocol.Keybind p:
            if (Config.Config.Keybinds.At(key).CheckNone(out var keyFunction))
            {
                ConsoleText.Text($"Unbound key: '{key}'\n")
                    .Format(TextFormat.Error)
                    .Print();
                break;
            }

            // DEBUG
            ConsoleText.Text($"Keypress: '{key}' -> {keyFunction}\n")
                .Format(
                TextFormat.Default with
                {
                    Foreground = ConsoleColor.DarkGray,
                })
                .Print();
            if (keyFunction == EKeyFunction.Help)
            {
                PrintHelp(p);
                break;
            }
            if (p.ActionMap.At(keyFunction).CheckNone(out var keyAction))
            {
                ConsoleText.Text($"Key function '{keyFunction}' has no meaning in this context.\n")
                    .Format(
                    TextFormat.Error with
                    {
                        Foreground = ConsoleColor.Yellow,
                    })
                    .Print();
                break;
            }
            keyAction.ActionFunction(actions);
            break;
        }
    }

    private static void PrintHelp(EInputProtocol.Keybind keybindProtocol)
    {
        var text = TextBuilder.Start();
        text.Divider("help")
            .Text("* ")
            .Format(TextFormat.Structure)
            .Text(keybindProtocol.ContextDescription + "\n\n")
            .Format(
            TextFormat.Title with
            {
                Bold = false,
            })
            .Text("AVAILABLE ACTIONS:\n\n")
            .Format(
            TextFormat.Structure with
            {
                Underline = true,
                Bold = true,
            });
        foreach (var keybind in keybindProtocol.ActionMap.Elements)
        {
            string keybindDisplay =
                Config.Config.ReverseKeybindLookup
                    .At(keybind.A)
                    .RemapAs(x => string.Join(" | ", x.Elements))
                    .Or("(none)");
            text
                .Text("- ")
                .Format(TextFormat.Structure)
                .Text(keybind.B.Name.ToUpper())
                .Format(
                TextFormat.Important with
                {
                    Bold = true,
                })
                .Text(" < ")
                .Format(TextFormat.Structure)
                .Text(keybindDisplay)
                .Format(
                TextFormat.Object with
                {
                    Bold = true,
                })
                .Text(" >\n   ")
                .Format(TextFormat.Structure)
                .Text(keybind.B.Description)
                .Format(TextFormat.Info)
                .Text("\n\n");
        }
        text.Divider().Print();
    }

    private class RunningFields : IDisposable
    {
        public required IInputHandler[] InputHandlers { get; set; }
        public EInputProtocol? CurrentInputProtocol { get; set; }
        public required MasterListener MasterListener { get; set; }
        public required KeyReader KeyReader { get; set; }
        public required ProgramState State { get; set; }
        public required HashSet<SessionListener> SessionListeners { get; set; }
        public required Channel<IProgramEvent> EventsChannel { get; set; }
        public bool TerminationRequested { get; set; }

        public void Dispose()
        {
            KeyReader.Dispose();
            MasterListener.Dispose();
            foreach (var listener in SessionListeners) listener.Dispose();
            EventsChannel.Writer.TryComplete();
        }
    }

    // everything performed through this is synchronized.
    private class ProgramActions : IProgramActions
    {
        public static ProgramActions Instance { get; } = new();
        public void SetState(ProgramState state) => _program.State = state;
        public ProgramState State => _program.State;
        public void DoInput(AlephKeyPress key) => HandleInput(key, this);
        public void SetState(Func<ProgramState, ProgramState> changeFunction) => _program.State = changeFunction(_program.State);
        public void SendProgressor(Progressor progressor)
        {
            ConsoleText.Text("Progressing to: ")
                .Format(TextFormat.Info)
                .Text(progressor.Name)
                .Format(TextFormat.Object)
                .Text("\n")
                .Print();
            _program.State.GetCurrentSession().GetLogicalSession().Progress(progressor);

        }
        public void Exit() => InitiateShutdown();
    }

    private class ICLIHandle : IAlephICLIHandle
    {
        public static ICLIHandle Instance { get; } = new();
        public Task Finish => _runningTask ?? Task.CompletedTask;
        public void AddSession(IStateFZO rootState) => Master.Instance.AddSession(rootState);

        public async Task Stop()
        {
            InitiateShutdown();
            await _terminationCompletionSource!.Task;
        }
    }

    // everything sent through this is synchronized.
    private class EventSender : IEventSender
    {
        public static EventSender Instance { get; } = new();
        public void AddSessionListener(Session session) => _program.SessionListeners.Add(SessionListener.Link(this, session));
        public void SendEvent(IProgramEvent action)
        {
            if (_program.TerminationRequested || _eventWriter.TryWrite(action)) return;
            Task.Run(async () => await _eventWriter.WriteAsync(action).ConfigureAwait(false));
        }
    }
}