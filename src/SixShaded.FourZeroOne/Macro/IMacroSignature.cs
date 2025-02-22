#nullable enable
namespace SixShaded.FourZeroOne.Macro
{
    using Core.Resolutions.Boxed;
    using FourZeroOne.FZOSpec;
    using SixShaded.NotRust;
    using Token;
    using Res = Resolution.Res;
    public interface IMacroValue<RVal> : IMacro<RVal>, IHasNoArgs<RVal>
        where RVal : Res
    { }
    public interface IMacroFunction<RArg1, ROut> : IMacro<ROut>, IHasArgs<RArg1, ROut>
        where RArg1 : Res
        where ROut : Res
    { }
    public interface IMacroFunction<RArg1, RArg2, ROut> : IMacro<ROut>, IHasArgs<RArg1, RArg2, ROut>
        where RArg1 : Res
        where RArg2 : Res
        where ROut : Res
    { }
    public interface IMacroFunction<RArg1, RArg2, RArg3, ROut> : IMacro<ROut>, IHasArgs<RArg1, RArg2, RArg3, ROut>
        where RArg1 : Res
        where RArg2 : Res
        where RArg3 : Res
        where ROut : Res
    { }
}