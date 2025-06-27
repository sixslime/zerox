namespace SixShaded.FourZeroOne.Core.Korssas.Component.Attachment;

using SixShaded.FourZeroOne.Roveggi;

public sealed record Without<C, R> : Korssa.Defined.PureFunction<IRoveggi<C>, IRoveggi<IRovundantu<C, R>>, IRoveggi<C>>
    where C : IRovetu
    where R : class, Rog
{
    public Without(IKorssa<IRoveggi<C>> subject, IKorssa<IRoveggi<IRovundantu<C, R>>> address) : base(subject, address)
    { }

    protected override IRoveggi<C> EvaluatePure(IRoveggi<C> subject, IRoveggi<IRovundantu<C, R>> address) => subject.WithoutComponents([address.MemWrapped()]);

    protected override IOption<string> CustomToString() => $"{Arg1}@:{{{Arg2} X}}".AsSome();
}