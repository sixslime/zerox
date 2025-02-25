namespace SixShaded.FourZeroOne.FZOSpec;

public abstract record EKorssaMutation
{
    public required Kor Result { get; init; }

    public sealed record Identity : EKorssaMutation
    { }

    public sealed record MellsanoApply : EKorssaMutation
    {
        public required Mel Mellsano { get; init; }
    }
}