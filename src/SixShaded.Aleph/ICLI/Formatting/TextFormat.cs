namespace SixShaded.Aleph.ICLI.Formatting;
internal record TextFormat
{
    
    public ConsoleColor? Background { get; init; } = null;
    public ConsoleColor? Foreground { get; init; } = null;
    public bool Bold { get; init; } = false;
    public bool Underline { get; init; } = false;
    public static TextFormat Default { get; } = new();

    public static TextFormat Error { get; } =
        new()
        {
            Foreground = ConsoleColor.DarkRed,
            Bold = true,
        };

    public static TextFormat Structure { get; } =
        new()
        {
            Foreground = ConsoleColor.DarkCyan
        };

    public static TextFormat Notification { get; } =
        new()
        {
            Foreground = ConsoleColor.Yellow,
            Background = ConsoleColor.DarkGray,
        };

    public static TextFormat Info { get; } =
        new()
        {
            Foreground = ConsoleColor.Blue
        };

    public static TextFormat Title { get; } =
        new()
        {
            Foreground = ConsoleColor.White,
            Bold = true,
        };

    public static TextFormat Important { get; } =
        new()
        {
            Foreground = ConsoleColor.Magenta,
            Bold = true,
        };
    public static TextFormat Object { get; } =
        new()
        {
            Foreground = ConsoleColor.DarkYellow,
        };
}