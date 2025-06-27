namespace SixShaded.FourZeroOne.Core.Korssas.Component.Attachment;

using SixShaded.FourZeroOne.Roveggi;

public sealed record With<C, R> : Korssa.Defined.PureFunction<IRoveggi<C>, IRoveggi<IRovundantu<C, R>>, R, IRoveggi<C>>
    where C : IRovetu
    where R : class, Rog
{
    public With(IKorssa<IRoveggi<C>> subject, IKorssa<IRoveggi<IRovundantu<C, R>>> address, IKorssa<R> data) : base(subject, address, data)
    { }

    protected override IRoveggi<C> EvaluatePure(IRoveggi<C> subject, IRoveggi<IRovundantu<C, R>> address, R data) => subject.WithComponent(address.MemWrapped(), data);

    protected override IOption<string> CustomToString() => $"{Arg1}@:{{{Arg2}={Arg3}}}".AsSome();
}