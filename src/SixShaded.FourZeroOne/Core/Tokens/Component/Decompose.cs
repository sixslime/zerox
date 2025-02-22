namespace SixShaded.FourZeroOne.Core.Tokens.Component;

public sealed record Decompose<D, R> : Token.Defined.RuntimeHandledFunction<ICompositionOf<D>, R> where D : IDecomposableType<D, R>, new() where R : class, Res
{
    public Decompose(IToken<ICompositionOf<D>> composition) : base(composition) { }

    protected override FZOSpec.EStateImplemented MakeData(ICompositionOf<D> in1) => new D().DecompositionFunction.GenerateMetaExecute(in1.AsSome());
}