namespace SixShaded.FourZeroOne.Mellsano.Defined;

public record RealizeProxy<R> : Korssa.Defined.RuntimeHandledFunction<IProxy<R>, R>
    where R : class, Rog
{
    public RealizeProxy(IKorssa<IProxy<R>> proxy) : base(proxy) { }

    protected override FZOSpec.EStateImplemented MakeData(IProxy<R> proxy) =>
        new FZOSpec.EStateImplemented.MetaExecute { Korssa = proxy.Korssa, MellsanoAllows = proxy.ReallowsMellsano ? proxy.FromMellsano.Yield() : [] };
}