namespace SixShaded.Aleph.ICLI;

internal abstract record EInputProtocol
{
    public sealed record Keybind : EInputProtocol
    {
        public required string ContextDescription { get; init; }
        public required IPMap<Config.EKeyFunction, InputAction> ActionMap { get; init; }
    }

    public sealed record Direct : EInputProtocol
    {
        public required Action<Config.AlephKeyPress, IProgramActions> DirectAction { get; init; }
    }
}