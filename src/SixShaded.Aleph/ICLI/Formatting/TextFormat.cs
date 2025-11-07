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
            Foreground = ConsoleColor.White
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
}