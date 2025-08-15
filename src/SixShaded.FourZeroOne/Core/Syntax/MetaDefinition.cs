namespace SixShaded.FourZeroOne.Core.Syntax;

public delegate IKorssa<ROut> MetaDefinition<out ROut>() where ROut : class, Rog;

public delegate IKorssa<ROut> MetaDefinition<RArg1, RArg2, RArg3, out ROut>(DynamicRoda<RArg1> arg1, DynamicRoda<RArg2> arg2, DynamicRoda<RArg3> arg3)
    where ROut : class, Rog
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog;

public delegate IKorssa<ROut> MetaDefinition<RArg1, RArg2, out ROut>(DynamicRoda<RArg1> arg1, DynamicRoda<RArg2> arg2)
    where ROut : class, Rog
    where RArg1 : class, Rog
    where RArg2 : class, Rog;

public delegate IKorssa<ROut> MetaDefinition<RArg1, out ROut>(DynamicRoda<RArg1> arg1)
    where ROut : class, Rog
    where RArg1 : class, Rog;
