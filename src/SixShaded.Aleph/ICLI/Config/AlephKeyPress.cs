namespace SixShaded.Aleph.ICLI.Config;

internal class AlephKeyPress
{
    private static readonly Dictionary<ConsoleKey, string> SPECIAL_KEYS =
        new(
        Iter.Range(1, 24, true)
            .Map(i => new KeyValuePair<ConsoleKey, string>((ConsoleKey)(111 + i), $"f{i}")))
        {
            {
                ConsoleKey.Backspace, "backspace"
            },
            {
                ConsoleKey.UpArrow, "up"
            },
            {
                ConsoleKey.DownArrow, "down"
            },
            {
                ConsoleKey.LeftArrow, "left"
            },
            {
                ConsoleKey.RightArrow, "right"
            },
            {
                ConsoleKey.Spacebar, "space"
            },
            {
                ConsoleKey.Enter, "enter"
            },
            {
                ConsoleKey.Tab, "tab"
            },
        };
    public required bool Shift { get; init; }
    public required bool Control { get; init; }
    public required bool Alt { get; init; }
    public required string KeyString { get; init; }

    public static implicit operator AlephKeyPress(ConsoleKeyInfo keyInfo) =>
        new()
        {
            Shift = (keyInfo.Modifiers & ConsoleModifiers.Shift) != 0,
            Alt = (keyInfo.Modifiers & ConsoleModifiers.Alt) != 0,
            Control = (keyInfo.Modifiers & ConsoleModifiers.Control) != 0,
            KeyString =
                SPECIAL_KEYS.TryGetValue(keyInfo.Key, out string? special)
                    ? $"({special})"
                    : keyInfo.KeyChar.ToString(),
        };

    public override int GetHashCode() => HashCode.Combine(Shift, Control, Alt, KeyString);

    public override bool Equals(object? obj) =>
        obj is AlephKeyPress other &&
        other.KeyString == KeyString &&
        other.Shift == Shift &&
        other.Alt == Alt &&
        other.Control == Control;

    public override string ToString() => $"{(Control ? "Ctrl+" : "")}{(Shift ? "Shift+" : "")}{(Alt ? "Alt+" : "")}{KeyString.ToLower()}";
}