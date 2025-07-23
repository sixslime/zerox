namespace SixShaded.FourZeroOne.Roveggi;

using Defined;

// we are really fucking around with type safety with this shit.
/// <summary>
/// DEV: Must have an empty constructor. (See <see cref="Master"/>)
/// </summary>
/// <typeparam name="C"></typeparam>
internal class ImplementationContext<C> : IImplementationContext<C>
    where C : IRovetu
{
    /// <summary>
    /// Property name that holds the dictonary of get mappings.
    /// </summary>
    internal const string GETMAPPINGS_PROPERTY = "GetMappings";
    internal const string SETMAPPINGS_PROPERTY = "SetMappings";
    /// <summary>
    /// every entry should satisfy <b>{ for A, R | AbstractGetRovu&lt;A, R&gt; -&gt; MetaFunction&lt;IRoveggi&lt;C&gt;, R&gt; }</b>
    /// </summary>
    internal Dictionary<Unsafe.IAbstractRovu, Roggi.Unsafe.IMetaFunction<Rog>> GetMappings { get; } = new();
    /// <summary>
    /// every entry should satisfy <b>{ for A, R | AbstractSetRovu&lt;A, R&gt; -&gt; MetaFunction&lt;R, IRoveggi&lt;C&gt;&gt; }</b>
    /// </summary>
    internal Dictionary<Unsafe.IAbstractRovu, Roggi.Unsafe.IMetaFunction<Rog>> SetMappings { get; } = new();
    internal ImplementationContext()
    {

    }
    public IImplementationContext<C> ImplementGet<R, A>(Defined.AbstractGetRovu<A, R> abstractGetRovu, Func<DynamicRoda<IRoveggi<C>>, IKorssa<R>> implementation)
        where A : IRovetu
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

    public IImplementationContext<C> ImplementSet<R, A>(AbstractSetRovu<A, R> abstractSetRovu, Func<DynamicRoda<R>, IKorssa<IRoveggi<C>>> implementation)
        where R : class, Rog
        where A : IRovetu
    {
        var sourceAddr = new DynamicRoda<R>();
        SetMappings.Add(
        abstractSetRovu, new Core.Roggis.MetaFunction<R, IRoveggi<C>>(sourceAddr)
        {
            Korssa = implementation(sourceAddr),
            SelfRoda = new(),
            CapturedVariables = [],
            CapturedMemory = KorvessaDummyMemory.INSTANCE,
        });
        return this;
    }
}