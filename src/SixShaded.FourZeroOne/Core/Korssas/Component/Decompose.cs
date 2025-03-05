namespace SixShaded.FourZeroOne.Core.Korssas.Component;

public sealed record Decompose<D, R> : Korssa.Defined.RuntimeHandledFunction<IRoveggi<D>, R> where D : IDecomposableRovetu<D, R>, new() where R : class, Rog
{
    public Decompose(IKorssa<IRoveggi<D>> roveggi) : base(roveggi) { }

    protected override FZOSpec.EStateImplemented MakeData(IRoveggi<D> in1) => new D().DecomposeFunction.GenerateMetaExecute(in1.AsSome());
}