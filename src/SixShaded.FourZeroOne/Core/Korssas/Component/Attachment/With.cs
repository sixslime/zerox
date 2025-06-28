namespace SixShaded.FourZeroOne.Core.Korssas.Component.Attachment;

using SixShaded.FourZeroOne.Roveggi;

public sealed record With<C, RKey, RVal> : Korssa.Defined.RegularKorssa<IRoveggi<C>>
    where C : IVarovetu<RKey, RVal>
    where RKey : class, Rog
    where RVal : class, Rog
{
    public With(IKorssa<IRoveggi<C>> subject, IKorssa<RKey> key, IKorssa<RVal> value) : base(subject, key, value)
    { }

    public required IVarovu<C, RKey, RVal> Varovu { get; init; }

    protected override ITask<IOption<IRoveggi<C>>> StandardResolve(IKorssaContext runtime, RogOpt[] args) =>
        (args[0].RemapAs(x => (IRoveggi<C>)x).Check(out var subject)
         && args[1].RemapAs(x => (RKey)x).Check(out var key)
            ? (args[2].RemapAs(x => (RVal)x).Check(out var value)
                ? subject.WithComponent(Varovu.GenerateRovu(key), value)
                : subject.WithoutComponents([Varovu.GenerateRovu(key)])).AsSome()
            : new None<IRoveggi<C>>())
        .ToCompletedITask();
    protected override IOption<string> CustomToString() => $"{ArgKorssas[0]}@{Varovu.Identifier}:{{{ArgKorssas[1]}={ArgKorssas[2]}}}".AsSome();
}