namespace SixShaded.FourZeroOne.Core.Korssas.Rovi.Varovi;

using SixShaded.FourZeroOne.Roveggi;

public sealed record Get<C, RKey, RVal> : Korssa.Defined.RegularKorssa<RVal>
    where C : IRovetu
    where RKey : class, Rog
    where RVal : class, Rog
{
    public Get(IKorssa<IRoveggi<C>> subject, IKorssa<RKey> key) : base(subject, key)
    { }

    public required IVarovu<C, RKey, RVal> Varovu { get; init; }

    protected override ITask<IOption<RVal>> StandardResolve(IKorssaContext runtime, RogOpt[] args) =>
        (args[0].RemapAs(x => (IRoveggi<C>)x).Check(out var subject)
         && args[1].RemapAs(x => (RKey)x).Check(out var key)
            ? subject.GetComponent(Varovu.GenerateRovu(key))
            : new None<RVal>())
        .ToCompletedITask();
    protected override IOption<string> CustomToString() => $"{ArgKorssas[0]}@{Varovu}->{ArgKorssas[1]}".AsSome();
}