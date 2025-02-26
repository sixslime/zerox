namespace SixShaded.FourZeroOne.Roggi.Defined;

using Core.Roggis;

public abstract class DecomposableRoveggitu<Self, R> : IDecomposableRoveggitu<Self, R>
    where Self : IDecomposableRoveggitu<Self, R>, new()
    where R : class, Rog
{
    public abstract MetaFunction<IRoveggi<Self>, R> DecomposeFunction { get; }
}
