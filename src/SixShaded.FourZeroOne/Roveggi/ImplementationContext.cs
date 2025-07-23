namespace SixShaded.FourZeroOne.Roveggi;

public interface IImplementationContext<A, C>
    where A : IRovetu
    where C : IRovetu
{
    public IImplementationContext<A, C> AddImplementation<R>(Defined.AbstractGetRovu<A, R> abstractGetRovu, Core.Roggis.MetaFunction<IRoveggi<C>, R> getImplementation)
        where R : class, Rog;
}