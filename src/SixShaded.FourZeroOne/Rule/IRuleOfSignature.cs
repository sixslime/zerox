namespace SixShaded.FourZeroOne.Rule;

using Core.Resolutions;
using Defined.Proxies;
using Unsafe;

public interface IRuleOfSignature<RVal> : IRule<RVal>
    where RVal : class, Res
{
    public MetaFunction<OriginalProxy<RVal>, RVal> Definition { get; }
    public IRuleMatcher<IHasNoArgs<RVal>> Matcher { get; }
}

public interface IRuleOfSignature<RArg1, ROut> : IRule<ROut>
    where RArg1 : class, Res
    where ROut : class, Res
{
    public MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ROut> Definition { get; }
    public IRuleMatcher<IHasArgs<RArg1, ROut>> Matcher { get; }
}

public interface IRuleOfSignature<RArg1, RArg2, ROut> : IRule<ROut>
    where RArg1 : class, Res
    where RArg2 : class, Res
    where ROut : class, Res
{
    public MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ROut> Definition { get; }
    public IRuleMatcher<IHasArgs<RArg1, RArg2, ROut>> Matcher { get; }
}

public interface IRuleOfSignature<RArg1, RArg2, RArg3, ROut> : IRule<ROut>
    where RArg1 : class, Res
    where RArg2 : class, Res
    where RArg3 : class, Res
    where ROut : class, Res
{
    public OverflowingMetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ArgProxy<RArg3>, ROut> Definition { get; }
    public IRuleMatcher<IHasArgs<RArg1, RArg2, RArg3, ROut>> Matcher { get; }
}