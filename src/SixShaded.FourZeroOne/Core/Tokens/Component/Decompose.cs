#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens.Component
{
    public sealed record Decompose<D, R> : RuntimeHandledFunction<ICompositionOf<D>, R> where D : IDecomposableType<D, R>, new() where R : class, Res
    {
        public Decompose(IToken<ICompositionOf<D>> composition) : base(composition) { }

        protected override EStateImplemented MakeData(ICompositionOf<D> in1)
        {
            return new D().DecompositionFunction.GenerateMetaExecute(in1.AsSome());
        }
    }
}