namespace SixShaded.FourZeroOne.Core.Korssas.Component;

using Roveggi;
using Roveggi.Unsafe;

public sealed record Without<C> : Korssa.Defined.RegularKorssa<IRoveggi<C>>, IHasAttachedComponentIdentifier<C, IRoveggi<C>>
    where C : IRovetu
{
    public Without(IKorssa<IRoveggi<C>> holder) : base(holder)
    { }

    public required IRovu<C> Rovu { get; init; }
    protected override ITask<IOption<IRoveggi<C>>> StandardResolve(IKorssaContext _, RogOpt[] args) => args[0].RemapAs(x => ((IRoveggi<C>)x).WithoutComponents([Rovu])).ToCompletedITask();
    protected override IOption<string> CustomToString() => $"{ArgKorssas[0]}:{{{Rovu} X}}".AsSome();
    IRovu<C> IHasAttachedComponentIdentifier<C, IRoveggi<C>>._attachedRovu => Rovu;
}