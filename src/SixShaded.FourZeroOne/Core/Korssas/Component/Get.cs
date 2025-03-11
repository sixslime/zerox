namespace SixShaded.FourZeroOne.Core.Korssas.Component;

using SixShaded.FourZeroOne.Roveggi;
using SixShaded.FourZeroOne.Roveggi.Unsafe;

public sealed record Get<C, R> : Korssa.Defined.RegularKorssa<R>, IHasAttachedComponentIdentifier<C, R> where R : class, Rog where C : IRovetu
{
    public Get(IKorssa<IRoveggi<C>> holder) : base(holder) { }
    public required IRovu<C, R> Rovu { get; init; }
    IRovu<C> IHasAttachedComponentIdentifier<C, R>._attachedRovu => Rovu;
    protected override ITask<IOption<R>> StandardResolve(IKorssaContext _, RogOpt[] args) => args[0].RemapAs(x => ((IRoveggi<C>)x).GetComponent(Rovu)).Press().ToCompletedITask();
    protected override IOption<string> CustomToString() => $"{ArgKorssas[0]}->{Rovu}".AsSome();
}