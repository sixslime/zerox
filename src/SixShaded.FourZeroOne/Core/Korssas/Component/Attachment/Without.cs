namespace SixShaded.FourZeroOne.Core.Korssas.Component.Attachment;

using SixShaded.FourZeroOne.Roveggi;

public sealed record Without<C, RKey, RVal> : Korssa.Defined.RegularKorssa<IRoveggi<C>>
    where C : IRovetu
    where RKey : class, Rog
    where RVal : class, Rog
{
    public Without(IKorssa<IRoveggi<C>> subject, IKorssa<RKey> key) : base(subject, key)
    { }

    public required IVarovu<C, RKey, RVal> Varovu { get; init; }

    protected override ITask<IOption<IRoveggi<C>>> StandardResolve(IKorssaContext runtime, RogOpt[] args) =>
        (args[0].RemapAs(x => (IRoveggi<C>)x).Check(out var subject)
         && args[1].RemapAs(x => (RKey)x).Check(out var key)
            ? subject.WithoutComponents([Varovu.GenerateRovu(key)]).AsSome()
            : new None<IRoveggi<C>>())
        .ToCompletedITask();
    protected override IOption<string> CustomToString() => $"({ArgKorssas[0]}@{Varovu.Identifier}~{ArgKorssas[1]}=\u2205)".AsSome();
}