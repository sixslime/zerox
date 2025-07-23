namespace SixShaded.FourZeroOne.Core.Korssas.Component;

using Roveggi;
using Roveggi.Unsafe;

public sealed record GetOld<C, R> : Korssa.Defined.RegularKorssa<R>
    where R : class, Rog
    where C : IRovetu
{
    public GetOld(IKorssa<IRoveggi<C>> holder) : base(holder)
    { }

    public required IRovu<C, R> Rovu { get; init; }
    protected override ITask<IOption<R>> StandardResolve(IKorssaContext _, RogOpt[] args) => args[0].RemapAs(x => ((IRoveggi<C>)x).GetComponent(Rovu)).Press().ToCompletedITask();
    protected override IOption<string> CustomToString() => $"{ArgKorssas[0]}->{Rovu}".AsSome();
}