namespace SixShaded.FourZeroOne.Core.Korssas.Component;

using Roveggi;
using Roveggi.Unsafe;

public sealed record WithOld<C, R> : Korssa.Defined.RegularKorssa<IRoveggi<C>>
    where R : class, Rog
    where C : IRovetu
{
    public With(IKorssa<IRoveggi<C>> holder, IKorssa<R> component) : base(holder, component)
    { }

    public required IRovu<C, R> Rovu { get; init; }

    protected override ITask<IOption<IRoveggi<C>>> StandardResolve(IKorssaContext _, RogOpt[] args) =>
        (args[0].RemapAs(x => (IRoveggi<C>)x).Check(out var holder)
            ? (args[1].RemapAs(x => (R)x).Check(out var component)
                ? holder.WithComponent(Rovu, component)
                : holder
            ).AsSome()
            : new None<IRoveggi<C>>()
        ).ToCompletedITask();

    protected override IOption<string> CustomToString() => $"({ArgKorssas[0]}~{Rovu}={ArgKorssas[1]})".AsSome();
}