namespace SixShaded.FourZeroOne.Mellsano.Defined.Proxies;

public record ArgProxy<R> : ProxyBehavior<R>
    where R : class, Rog
{
    public override bool ReallowsMellsano => true;
}