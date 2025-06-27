namespace SixShaded.FourZeroOne.Core.Korssas.Memory.Attachment;

using Roveggi;

public sealed record Insert<C, R> : Korssa.Defined.PureFunction<IRoveggi<C>, IRoveggi<IRovundantu<C, R>>, R, IRoveggi<C>>
    where C : IRovetu
    where R : class, Rog
{
    public Insert(IKorssa<IRoveggi<C>> subject, IKorssa<IRoveggi<IRovundantu<C, R>>> address, IKorssa<R> data) : base(subject, address, data)
    { }

    protected override IRoveggi<C> EvaluatePure(IRoveggi<C> subject, IRoveggi<IRovundantu<C, R>> address, R data) => subject.WithComponent(address.MemWrapped(), data);

    protected override IOption<string> CustomToString() => $"{Arg1}.{Arg2} <== {Arg3}".AsSome();
}