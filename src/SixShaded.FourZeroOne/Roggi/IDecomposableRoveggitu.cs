namespace SixShaded.FourZeroOne.Roggi;

public interface IDecomposableRoveggitu<Self, R> : IRoveggitu where Self : IDecomposableRoveggitu<Self, R>, new() where R : class, Rog
{
    public Core.Roggis.MetaFunction<IRoveggi<Self>, R> DecomposeFunction { get; }
}