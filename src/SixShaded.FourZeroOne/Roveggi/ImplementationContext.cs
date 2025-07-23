namespace SixShaded.FourZeroOne.Roveggi;

public class ImplementationContext<C, A>
    where A : IRovetu
    where C : IRovetu
{
    internal Dictionary<Unsafe.IGetRovu<A>, Roggi.Unsafe.IMetaFunction<Rog>> GetMappings { get; } = new();

    public ImplementationContext<A, C> ImplementGet<R>(Defined.AbstractGetRovu<A, R> abstractGetRovu, Func<DynamicRoda<IRoveggi<C>>, IKorssa<R>> implementation)
        where R : class, Rog
    {
        var sourceAddr = new DynamicRoda<IRoveggi<C>>();
        GetMappings.Add(
        abstractGetRovu, new Core.Roggis.MetaFunction<IRoveggi<C>, R>(sourceAddr)
        {
            Korssa = implementation(sourceAddr),
            SelfRoda = new(),
            CapturedVariables = [],
            CapturedMemory = KorvessaDummyMemory.INSTANCE,
        });
        return this;
    }
}