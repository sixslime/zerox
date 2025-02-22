#nullable enable
namespace SixShaded.FourZeroOne.Rule.Defined.Proxies
{
    public record ArgProxy<R> : ProxyBehavior<R> where R : Res
    {
        public override bool ReallowsRule => true;
    }
}
