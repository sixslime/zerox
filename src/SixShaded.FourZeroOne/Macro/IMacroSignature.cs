namespace SixShaded.FourZeroOne.Macro;

public interface IMacroSignature<RVal> : IMacro<RVal>, IHasNoArgs<RVal>
    where RVal : class, Res;

public interface IMacroSignature<RArg1, ROut> : IMacro<ROut>, IHasArgs<RArg1, ROut>
    where RArg1 : class, Res
    where ROut : class, Res;

public interface IMacroSignature<RArg1, RArg2, ROut> : IMacro<ROut>, IHasArgs<RArg1, RArg2, ROut>
    where RArg1 : class, Res
    where RArg2 : class, Res
    where ROut : class, Res;
public interface IMacroSignature<RArg1, RArg2, RArg3, ROut> : IMacro<ROut>, IHasArgs<RArg1, RArg2, RArg3, ROut>
    where RArg1 : class, Res
    where RArg2 : class, Res
    where RArg3 : class, Res
    where ROut : class, Res;