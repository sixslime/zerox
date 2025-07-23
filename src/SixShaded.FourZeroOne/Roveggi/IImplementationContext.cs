namespace SixShaded.FourZeroOne.Roveggi;

public interface IImplementationContext<C>
    where C : IRovetu
{
    public IImplementationContext<C> ImplementGet<R, A>(Defined.AbstractGetRovu<A, R> abstractGetRovu, Func<DynamicRoda<IRoveggi<C>>, IKorssa<R>> implementation)
        where A : IRovetu
        where R : class, Rog;

    public IImplementationContext<C> ImplementSet<R, A>(Defined.AbstractSetRovu<A, R> abstractSetRovu, Func<DynamicRoda<R>, IKorssa<IRoveggi<C>>> implementation)
        where A : IRovetu
        where R : class, Rog;
}