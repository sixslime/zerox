namespace SixShaded.CoreTypeMatcher.Types.Korssa.Memory.Rovedanggi;

public record AllValues : IKorssaType
{
    public required RovetuTypeInfo RovedantuType { get; init; }
}