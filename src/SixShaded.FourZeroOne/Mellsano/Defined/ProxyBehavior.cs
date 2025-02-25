namespace SixShaded.FourZeroOne.Mellsano.Defined;

public abstract record ProxyBehavior<R> : Roggi.Defined.NoOp, IProxy<R>
    where R : class, Rog
{
    public required IKorssa<R> Korssa { get; init; }
    public required MellsanoID FromMellsano { get; init; }
    public abstract bool ReallowsMellsano { get; }
}