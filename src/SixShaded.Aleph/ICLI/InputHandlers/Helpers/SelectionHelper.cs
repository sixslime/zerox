namespace SixShaded.Aleph.ICLI.InputHandlers.Helpers;
using Formatting;
using Config;
internal class SelectionHelper
{
    private bool _tick = false;
    private string _buffer = "";
    private ConsoleText[]? _entries = null;
    private int? _maxBufferLength = null;
    private static readonly TextFormat BUFFER_FORMAT = TextFormat.Default;
    private static readonly ConsoleText INPUT_PROMPT = ConsoleText.Text("> ").Format(TextFormat.Title).Build();
    public EInputProtocol.Direct Protocol =>
        new()
        {
            DirectAction = HandleKeypress
        };
    public required Func<State.ProgramState, ConsoleText[]> EntryGenerator { get; init; }
    public required Action<int, IProgramActions> SelectAction { get; init; }
    public required Action<IProgramActions> CancelAction { get; init; }

    private void HandleKeypress(AlephKeyPress key, IProgramActions program)
    {
        if (_entries is null || _entries.Length == 0)
        {
            ConsoleText.Text("\n No entries available to be selected.\n")
                .Format(TextFormat.Error)
                .Print();
            CancelAction(program);
            return;
        }
        if (key.Equals(Config.Selection.Submit))
        {
            if (_buffer.Length == 0)
            {
                ConsoleText.Text("\n No selection made.\n")
                    .Format(TextFormat.Error)
                    .Print();
                INPUT_PROMPT.Print();
                return;
            }
            DoSelect(program);
            return;
        }
        if (key.Equals(Config.Selection.Cancel))
        {
            Tick(false, program.State);
            ConsoleText.Text("\n").Print();
            CancelAction(program);
            return;
        }
        if (key.Equals(Config.Selection.Delete))
        {
            // cheugy
            if (_buffer.Length == 0) return;
            _buffer = _buffer[..^1];
            Console.CursorLeft -= 1;
            ConsoleText.Text(" ").Print();
            Console.CursorLeft -= 1;
            return;
        }
        if (key.Equals(Config.Selection.ShowAvailable))
        {
            ConsoleText.Text("\n").Print();
            PrintRemainingEntries();
            return;
        }
        string input = key.KeyString.ToLower();
        if (!Config.Selection.Indicators.Contains(input))
            return;
        if ((_buffer + input).FromBase(Config.Selection.Indicators) >= _entries.Length)
            return;
        ConsoleText.Text(input)
            .Format(TextFormat.Default)
            .Print();
        _buffer += input;
        _maxBufferLength ??= (_entries.Length - 1).ToBase(Config.Selection.Indicators).Length;
        if (_buffer.Length >= _maxBufferLength)
            DoSelect(program);
    }
    public void Tick(bool active, State.ProgramState state)
    {
        if (_tick == active) return;
        _tick = active;
        _buffer = "";
        if (active)
        {
            _entries = EntryGenerator(state);
            PrintRemainingEntries();
        }
        else
        {
            _entries = null;
        }
    }

    private void DoSelect(IProgramActions program)
    {
        // assumes all is valid and good:
        int index = _buffer.FromBase(Config.Selection.Indicators);
        ConsoleText.Text("\n").Print();
        SelectAction(index, program);
    }
    private void PrintRemainingEntries()
    {
        if (_entries is null) return;
        ConsoleText? currentlySelected = null;
        for (int i = 0; i < _entries.Length; i++)
        {
            string indicator = i.ToBase(Config.Selection.Indicators);
            if (!indicator.StartsWith(_buffer)) return;
            if (indicator == _buffer) currentlySelected = _entries[i];
            string remainingIndicator = indicator[_buffer.Length..];
            TextBuilder.Start()
                .Text(" [")
                .Format(TextFormat.Structure)
                .Text(_buffer)
                .Format(
                TextFormat.Resolved with
                {
                    Underline = true,
                    Bold = true,
                })
                .Text(remainingIndicator)
                .Format(
                TextFormat.Hint with
                {
                    Bold = false,
                    Underline = false,
                })
                .Text("] ")
                .Format(TextFormat.Structure)
                .Build()
                .Append(_entries[i])
                .Print();
            ConsoleText.Text("\n").Print();
        }
        if (currentlySelected is not null)
        {
            ConsoleText.Text("\nCURRENTLY SELECTED:\n")
                .Format(
                TextFormat.Structure with
                {
                    Bold = true
                })
                .Build()
                .Append(currentlySelected)
                .Print();
        }
        INPUT_PROMPT.Append(ConsoleText.Text(_buffer).Format(BUFFER_FORMAT).Build()).Print();
    }
}