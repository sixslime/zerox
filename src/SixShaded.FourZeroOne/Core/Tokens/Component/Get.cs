#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens.Component
{
    public sealed record Get<C, R> : StandardToken<R>, IHasAttachedComponentIdentifier<C, R> where R : class, ResObj where C : ICompositionType
    {
        public required IComponentIdentifier<C, R> ComponentIdentifier { get; init; }
        IComponentIdentifier<C> IHasAttachedComponentIdentifier<C, R>.AttachedComponentIdentifier => ComponentIdentifier;
        public Get(IToken<ICompositionOf<C>> holder) : base(holder) { }
        protected override ITask<IOption<R>> StandardResolve(ITokenContext _, IOption<ResObj>[] args)
        {
            return args[0].RemapAs(x => ((ICompositionOf<C>)x).GetComponent(ComponentIdentifier)).Press().ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"{ArgTokens[0]}->{ComponentIdentifier}".AsSome();
    }
}