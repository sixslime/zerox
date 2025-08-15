namespace SixShaded.FourZeroOne.Core.Syntax;
using Roggis;

public delegate IKorssa<ROut> RecursiveMetaDefinition<ROut>(DynamicRoda<MetaFunction<ROut>> self) where ROut : class, Rog;

public delegate IKorssa<ROut> RecursiveMetaDefinition<RArg1, RArg2, RArg3, ROut>(DynamicRoda<MetaFunction<RArg1, RArg2, RArg3, ROut>> self, DynamicRoda<RArg1> arg1, DynamicRoda<RArg2> arg2, DynamicRoda<RArg3> arg3)
    where ROut : class, Rog
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog;

public delegate IKorssa<ROut> RecursiveMetaDefinition<RArg1, RArg2, ROut>(DynamicRoda<MetaFunction<RArg1, RArg2, ROut>> self, DynamicRoda<RArg1> arg1, DynamicRoda<RArg2> arg2)
    where ROut : class, Rog
    where RArg1 : class, Rog
    where RArg2 : class, Rog;

public delegate IKorssa<ROut> RecursiveMetaDefinition<RArg1, ROut>(DynamicRoda<MetaFunction<RArg1, ROut>> self, DynamicRoda<RArg1> arg1)
    where ROut : class, Rog
    where RArg1 : class, Rog;