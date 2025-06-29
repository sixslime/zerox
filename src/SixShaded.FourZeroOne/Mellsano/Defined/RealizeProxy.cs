namespace SixShaded.FourZeroOne.Mellsano.Defined;

public record RealizeProxy<R> : Korssa.Defined.StateImplementedKorssa<IProxy<R>, R>
    where R : class, Rog
{
    public RealizeProxy(IKorssa<IProxy<R>> proxy) : base(proxy)
    { }

    protected override IOption<FZOSpec.EStateImplemented> MakeData(IKorssaContext context, IOption<IProxy<R>> proxyOpt) =>
        proxyOpt.RemapAs(
        proxy =>
            new FZOSpec.EStateImplemented.MetaExecute
            {
                Korssa = proxy.Korssa,
                MellsanoAllows = proxy.ReallowsMellsano ? proxy.FromMellsano.Yield() : [],
            });

    protected override IOption<string> CustomToString() => $"{Arg1}!".AsSome();

}