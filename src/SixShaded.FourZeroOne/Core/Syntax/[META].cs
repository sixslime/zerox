namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;

public static partial class Core
{
    public static Korssas.Fixed<MetaFunction<ROut>> kMetaFunction<ROut>(Func<IKorssa<ROut>> korssaFunction) where ROut : class, Rog
    {
        var vs = new DynamicAddress<MetaFunction<ROut>>();
        return new(new() { SelfIdentifier = vs, Korssa = korssaFunction() });
    }

    public static Korssas.Fixed<MetaFunction<RArg1, ROut>> kMetaFunction<RArg1, ROut>(Func<DynamicAddress<RArg1>, IKorssa<ROut>> korssaFunction)
        where RArg1 : class, Rog
        where ROut : class, Rog
    {
        var vs = new DynamicAddress<MetaFunction<RArg1, ROut>>();
        var v1 = new DynamicAddress<RArg1>();
        return new(new() { SelfIdentifier = vs, IdentifierA = v1, Korssa = korssaFunction(v1) });
    }

    public static Korssas.Fixed<MetaFunction<RArg1, RArg2, ROut>> kMetaFunction<RArg1, RArg2, ROut>(Func<DynamicAddress<RArg1>, DynamicAddress<RArg2>, IKorssa<ROut>> korssaFunction)
        where RArg1 : class, Rog
        where RArg2 : class, Rog
        where ROut : class, Rog
    {
        var vs = new DynamicAddress<MetaFunction<RArg1, RArg2, ROut>>();
        var (v1, v2) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>());
        return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, Korssa = korssaFunction(v1, v2) });
    }

    public static Korssas.Fixed<MetaFunction<RArg1, RArg2, RArg3, ROut>> kMetaFunction<RArg1, RArg2, RArg3, ROut>(Func<DynamicAddress<RArg1>, DynamicAddress<RArg2>, DynamicAddress<RArg3>, IKorssa<ROut>> korssaFunction)
        where RArg1 : class, Rog
        where RArg2 : class, Rog
        where RArg3 : class, Rog
        where ROut : class, Rog
    {
        var vs = new DynamicAddress<MetaFunction<RArg1, RArg2, RArg3, ROut>>();
        var (v1, v2, v3) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>(), new DynamicAddress<RArg3>());
        return new(new()
        {
            SelfIdentifier = vs,
            IdentifierA = v1,
            IdentifierB = v2,
            IdentifierC = v3,
            Korssa = korssaFunction(v1, v2, v3),
        });
    }

    public static Korssas.Fixed<MetaFunction<ROut>> kMetaFunctionRecursive<ROut>(Func<DynamicAddress<MetaFunction<ROut>>, IKorssa<ROut>> korssaFunction) where ROut : class, Rog
    {
        var vs = new DynamicAddress<MetaFunction<ROut>>();
        return new(new() { SelfIdentifier = vs, Korssa = korssaFunction(vs) });
    }

    public static Korssas.Fixed<MetaFunction<RArg1, ROut>> kMetaFunctionRecursive<RArg1, ROut>(Func<DynamicAddress<MetaFunction<RArg1, ROut>>, DynamicAddress<RArg1>, IKorssa<ROut>> korssaFunction)
        where RArg1 : class, Rog
        where ROut : class, Rog
    {
        var vs = new DynamicAddress<MetaFunction<RArg1, ROut>>();
        var v1 = new DynamicAddress<RArg1>();
        return new(new() { SelfIdentifier = vs, IdentifierA = v1, Korssa = korssaFunction(vs, v1) });
    }

    public static Korssas.Fixed<MetaFunction<RArg1, RArg2, ROut>> kMetaFunctionRecursive<RArg1, RArg2, ROut>(Func<DynamicAddress<MetaFunction<RArg1, RArg2, ROut>>, DynamicAddress<RArg1>, DynamicAddress<RArg2>, IKorssa<ROut>> korssaFunction)
        where RArg1 : class, Rog
        where RArg2 : class, Rog
        where ROut : class, Rog
    {
        var vs = new DynamicAddress<MetaFunction<RArg1, RArg2, ROut>>();
        var (v1, v2) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>());
        return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, Korssa = korssaFunction(vs, v1, v2) });
    }

    public static Korssas.Fixed<MetaFunction<RArg1, RArg2, RArg3, ROut>> kMetaFunctionRecursive<RArg1, RArg2, RArg3, ROut>(Func<DynamicAddress<MetaFunction<RArg1, RArg2, RArg3, ROut>>, DynamicAddress<RArg1>, DynamicAddress<RArg2>, DynamicAddress<RArg3>, IKorssa<ROut>> korssaFunction)
        where RArg1 : class, Rog
        where RArg2 : class, Rog
        where RArg3 : class, Rog
        where ROut : class, Rog
    {
        var vs = new DynamicAddress<MetaFunction<RArg1, RArg2, RArg3, ROut>>();
        var (v1, v2, v3) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>(), new DynamicAddress<RArg3>());
        return new(new()
        {
            SelfIdentifier = vs,
            IdentifierA = v1,
            IdentifierB = v2,
            IdentifierC = v3,
            Korssa = korssaFunction(vs, v1, v2, v3),
        });
    }
}

public static partial class KorssaSyntax
{
    public static Korssas.Execute<R> kExecute<R>(this IKorssa<MetaFunction<R>> source) where R : class, Rog => new(source);

    public static Korssas.Execute<RArg1, ROut> kExecuteWith<RArg1, ROut>(this IKorssa<MetaFunction<RArg1, ROut>> source, Structure.Korssa.Args<RArg1> args)
        where RArg1 : class, Rog
        where ROut : class, Rog =>
        new(source, new Korssas.ToBoxedArgs<RArg1>(args.A));

    public static Korssas.Execute<RArg1, RArg2, ROut> kExecuteWith<RArg1, RArg2, ROut>(this IKorssa<MetaFunction<RArg1, RArg2, ROut>> source, Structure.Korssa.Args<RArg1, RArg2> args)
        where RArg1 : class, Rog
        where RArg2 : class, Rog
        where ROut : class, Rog =>
        new(source, new Korssas.ToBoxedArgs<RArg1, RArg2>(args.A, args.B));

    public static Korssas.Execute<RArg1, RArg2, RArg3, ROut> kExecuteWith<RArg1, RArg2, RArg3, ROut>(this IKorssa<MetaFunction<RArg1, RArg2, RArg3, ROut>> source, Structure.Korssa.Args<RArg1, RArg2, RArg3> args)
        where RArg1 : class, Rog
        where RArg2 : class, Rog
        where RArg3 : class, Rog
        where ROut : class, Rog =>
        new(source, new Korssas.ToBoxedArgs<RArg1, RArg2, RArg3>(args.A, args.B, args.C));

    public static Korssas.Fixed<MetaFunction<ROut>> kMetaBoxed<ROut>(this IKorssa<ROut> korssa) where ROut : class, Rog => new(new() { SelfIdentifier = new(), Korssa = korssa });
}