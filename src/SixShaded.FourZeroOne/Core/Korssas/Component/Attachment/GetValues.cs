namespace SixShaded.FourZeroOne.Core.Korssas.Component.Attachment;

using Roggis;
using SixShaded.FourZeroOne.Roveggi;

public sealed record GetValues<C, RKey, RVal> : Korssa.Defined.RegularKorssa<Multi<RVal>>
    where C : IRovetu
    where RKey : class, Rog
    where RVal : class, Rog
{
    public GetValues(IKorssa<IRoveggi<C>> subject) : base(subject)
    { }

    public required IVarovu<C, RKey, RVal> Varovu { get; init; }

    protected override ITask<IOption<Multi<RVal>>> StandardResolve(IKorssaContext runtime, RogOpt[] args) =>
        (args[0].RemapAs(x => (IRoveggi<C>)x).Check(out var subject)
            ? new Multi<RVal>
            {
                Values =
                    subject.ComponentsUnsafe
                        .FilterMap(
                        x =>
                            x.A.MaybeA<VarovaWrapper<C, RKey, RVal>>()
                                .Retain(y => y.Varovu.Equals(Varovu))
                                .RemapAs(_ => x.B.IsA<RVal>()))
                        .ToPSequence()
            }.AsSome()
            : new None<Multi<RVal>>())
        .ToCompletedITask();
    protected override IOption<string> CustomToString() => $"{ArgKorssas[0]}@{Varovu.Identifier}<keys>".AsSome();
}