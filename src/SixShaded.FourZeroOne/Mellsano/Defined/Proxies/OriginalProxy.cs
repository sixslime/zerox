namespace SixShaded.FourZeroOne.Mellsano.Defined.Proxies;

public record OriginalProxy<R> : ProxyBehavior<R>
    where R : class, Rog
{
    public override bool ReallowsMellsano => false;
}