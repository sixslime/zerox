namespace SixShaded.FourZeroOne.Core.Korssas.Component.Attachment;

using Roggis;
using SixShaded.FourZeroOne.Roveggi;

public sealed record GetKeys<C, RKey, RVal> : Korssa.Defined.RegularKorssa<Multi<RKey>>
    where C : IRovetu
    where RKey : class, Rog
    where RVal : class, Rog
{
    public GetKeys(IKorssa<IRoveggi<C>> subject) : base(subject)
    { }

    public required IVarovu<C, RKey, RVal> Varovu { get; init; }

    protected override ITask<IOption<Multi<RKey>>> StandardResolve(IKorssaContext runtime, RogOpt[] args) =>
        (args[0].RemapAs(x => (IRoveggi<C>)x).Check(out var subject)
         && args[1].RemapAs(x => (RKey)x).Check(out var key)
            ? new Multi<RKey>
            {
                Values =
                    subject.ComponentsUnsafe
                        .FilterMap(
                        x =>
                            x.A.MightBeA<VarovaWrapper<C, RKey, RVal>>()
                                .Retain(y => y.Varovu.Equals(Varovu))
                                .RemapAs(y => y.KeyRoggi))
                        .ToPSequence()
            }.AsSome()
            : new None<Multi<RKey>>())
        .ToCompletedITask();
    protected override IOption<string> CustomToString() => $"{ArgKorssas[0]}@{Varovu.Identifier}<keys>".AsSome();
}