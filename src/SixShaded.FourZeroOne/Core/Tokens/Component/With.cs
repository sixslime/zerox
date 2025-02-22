#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens.Component
{
    public sealed record With<C, R> : StandardToken<ICompositionOf<C>>, IHasAttachedComponentIdentifier<C, ICompositionOf<C>> where R : class, Res where C : ICompositionType
    {
        public required IComponentIdentifier<C, R> ComponentIdentifier { get; init; }
        IComponentIdentifier<C> IHasAttachedComponentIdentifier<C, ICompositionOf<C>>.AttachedComponentIdentifier => ComponentIdentifier;
        public With(IToken<ICompositionOf<C>> holder, IToken<R> component) : base(holder, component) { }
        protected override ITask<IOption<ICompositionOf<C>>> StandardResolve(ITokenContext _, IOption<Res>[] args)
        {
            return
                (args[0].RemapAs(x => (ICompositionOf<C>)x).Check(out var holder)
                ? (args[1].RemapAs(x => (R)x).Check(out var component)
                    ? holder.WithComponent(ComponentIdentifier, component)
                    : holder
                    ).AsSome()
                : new None<ICompositionOf<C>>()
                ).ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"{ArgTokens[0]}:{{{ComponentIdentifier}={ArgTokens[1]}}}".AsSome();
    }
}