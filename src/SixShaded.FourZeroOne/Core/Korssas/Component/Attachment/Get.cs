namespace SixShaded.FourZeroOne.Core.Korssas.Component.Attachment;

using SixShaded.FourZeroOne.Roveggi;

public sealed record Get<C, R> : Korssa.Defined.Function<IRoveggi<C>, IRoveggi<IRovundantu<C, R>>, R>
    where C : IRovetu
    where R : class, Rog
{
    public Get(IKorssa<IRoveggi<C>> subject, IKorssa<IRoveggi<IRovundantu<C, R>>> address) : base(subject, address)
    { }

    protected override ITask<IOption<R>> Evaluate(IKorssaContext _, IOption<IRoveggi<C>> subjectOpt, IOption<IRoveggi<IRovundantu<C, R>>> addressOpt) =>
        subjectOpt.Check(out var subject) && addressOpt.Check(out var address)
            ? subject.GetComponent(address.MemWrapped()).ToCompletedITask()
            : new None<R>().ToCompletedITask();

    protected override IOption<string> CustomToString() => $"{Arg1}@->{Arg2}".AsSome();
}