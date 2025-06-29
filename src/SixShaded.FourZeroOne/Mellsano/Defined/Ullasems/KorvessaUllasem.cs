namespace SixShaded.FourZeroOne.Mellsano.Defined.Ullasems;

using SixShaded.FourZeroOne.Korvessa.Defined;

public abstract record KorvessaUllasem : IUllasem<Kor>
{
    public abstract bool MatchesKorssa(Kor korssa);
    public required Korvedu Dusem { get; init; }
    public override string ToString() => $"is({Dusem.GetSignature()})";
}
public record KorvessaUllasem<RVal> : KorvessaUllasem, IUllasem<IKorvessaSignature<RVal>>
    where RVal : class, Rog
{
    public override bool MatchesKorssa(Kor korssa) => korssa is Korvessa<RVal> korvessa && korvessa.Du.Equals(Dusem);
}

public record KorvessaUllasem<RArg1, ROut> : KorvessaUllasem, IUllasem<IKorvessaSignature<RArg1, ROut>>
    where RArg1 : class, Rog
    where ROut : class, Rog
{
    public override bool MatchesKorssa(Kor korssa) => korssa is Korvessa<RArg1, ROut> korvessa && korvessa.Du.Equals(Dusem);
}

public record KorvessaUllasem<RArg1, RArg2, ROut> : KorvessaUllasem, IUllasem<IKorvessaSignature<RArg1, RArg2, ROut>>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where ROut : class, Rog
{
    public override bool MatchesKorssa(Kor korssa) => korssa is Korvessa<RArg1, RArg2, ROut> korvessa && korvessa.Du.Equals(Dusem);
}

public record KorvessaUllasem<RArg1, RArg2, RArg3, ROut> : KorvessaUllasem, IUllasem<IKorvessaSignature<RArg1, RArg2, RArg3, ROut>>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where ROut : class, Rog
{
    public override bool MatchesKorssa(Kor korssa) => korssa is Korvessa<RArg1, RArg2, RArg3, ROut> korvessa && korvessa.Du.Equals(Dusem);
}