#nullable enable
namespace SixShaded.FourZeroOne.Rule.Defined.Proxies
{
    public record OriginalProxy<R> : ProxyBehavior<R> where R : Res
    {
        public override bool ReallowsRule => false;
    }
}
