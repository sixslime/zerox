#nullable enable
namespace SixShaded.FourZeroOne.Rule.Defined.Proxies
{
    public record ArgProxy<R> : ProxyBehavior<R> where R : class, Res
    {
        public override bool ReallowsRule => true;
    }
}
