namespace SixShaded.CoreTypeMatcher.Types.Korssa.Memory.Rovedanggi;

public record AllKeys : IKorssaType
{
    public required RovetuTypeInfo RovedantuType { get; init; }
}