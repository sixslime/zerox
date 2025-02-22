#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens.Component
{
    public sealed record Without<C> : StandardToken<ICompositionOf<C>>, IHasAttachedComponentIdentifier<C, ICompositionOf<C>> where C : ICompositionType
    {
        public required IComponentIdentifier<C> ComponentIdentifier { get; init; }
        IComponentIdentifier<C> IHasAttachedComponentIdentifier<C, ICompositionOf<C>>.AttachedComponentIdentifier => ComponentIdentifier;
        public Without(IToken<ICompositionOf<C>> holder) : base(holder) { }
        protected override ITask<IOption<ICompositionOf<C>>> StandardResolve(ITokenContext _, IOption<ResObj>[] args)
        {
            return args[0].RemapAs(x => ((ICompositionOf<C>)x).WithoutComponents([ComponentIdentifier])).ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"{ArgTokens[0]}:{{{ComponentIdentifier} X}}".AsSome();
    }
}