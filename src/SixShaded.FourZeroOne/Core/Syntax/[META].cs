namespace SixShaded.FourZeroOne.Core.Syntax;

using System.Security.Cryptography;
using Roggis;

public static partial class Core
{
    public static Korssas.DefineMetaFunction<ROut> kMetaFunction<ROut>(IEnumerable<Addr> captures, Func<IKorssa<ROut>> korssaFunction)
        where ROut : class, Rog =>
        kMetaFunctionRecursive<ROut>(captures, _ => korssaFunction());

    public static Korssas.DefineMetaFunction<RArg1, ROut> kMetaFunction<RArg1, ROut>(IEnumerable<Addr> captures, Func<DynamicAddress<RArg1>, IKorssa<ROut>> korssaFunction)
        where RArg1 : class, Rog
        where ROut : class, Rog =>
        kMetaFunctionRecursive<RArg1, ROut>(captures, (_, a) => korssaFunction(a));

    public static Korssas.DefineMetaFunction<RArg1, RArg2, ROut> kMetaFunction<RArg1, RArg2, ROut>(IEnumerable<Addr> captures, Func<DynamicAddress<RArg1>, DynamicAddress<RArg2>, IKorssa<ROut>> korssaFunction)
        where RArg1 : class, Rog
        where RArg2 : class, Rog
        where ROut : class, Rog =>
        kMetaFunctionRecursive<RArg1, RArg2, ROut>(captures, (_, a, b) => korssaFunction(a, b));

    public static Korssas.DefineMetaFunction<RArg1, RArg2, RArg3, ROut> kMetaFunction<RArg1, RArg2, RArg3, ROut>(IEnumerable<Addr> captures, Func<DynamicAddress<RArg1>, DynamicAddress<RArg2>, DynamicAddress<RArg3>, IKorssa<ROut>> korssaFunction)
        where RArg1 : class, Rog
        where RArg2 : class, Rog
        where RArg3 : class, Rog
        where ROut : class, Rog =>
        kMetaFunctionRecursive<RArg1, RArg2, RArg3, ROut>(captures, (_, a, b, c) => korssaFunction(a, b, c));

    public static Korssas.DefineMetaFunction<ROut> kMetaFunctionRecursive<ROut>(IEnumerable<Addr> captures, Func<DynamicAddress<MetaFunction<ROut>>, IKorssa<ROut>> korssaFunction)
        where ROut : class, Rog =>
        new(korssaFunction)
        {
            Captures = captures.ToArray()
        };

    public static Korssas.DefineMetaFunction<RArg1, ROut> kMetaFunctionRecursive<RArg1, ROut>(IEnumerable<Addr> captures, Func<DynamicAddress<MetaFunction<RArg1, ROut>>, DynamicAddress<RArg1>, IKorssa<ROut>> korssaFunction)
        where RArg1 : class, Rog
        where ROut : class, Rog =>
        new(korssaFunction)
        {
            Captures = captures.ToArray()
        };

    public static Korssas.DefineMetaFunction<RArg1, RArg2, ROut> kMetaFunctionRecursive<RArg1, RArg2, ROut>(IEnumerable<Addr> captures, Func<DynamicAddress<MetaFunction<RArg1, RArg2, ROut>>, DynamicAddress<RArg1>, DynamicAddress<RArg2>, IKorssa<ROut>> korssaFunction)
        where RArg1 : class, Rog
        where RArg2 : class, Rog
        where ROut : class, Rog =>
        new(korssaFunction)
        {
            Captures = captures.ToArray()
        };

    public static Korssas.DefineMetaFunction<RArg1, RArg2, RArg3, ROut> kMetaFunctionRecursive<RArg1, RArg2, RArg3, ROut>(IEnumerable<Addr> captures, Func<DynamicAddress<MetaFunction<RArg1, RArg2, RArg3, ROut>>, DynamicAddress<RArg1>, DynamicAddress<RArg2>, DynamicAddress<RArg3>, IKorssa<ROut>> korssaFunction)
        where RArg1 : class, Rog
        where RArg2 : class, Rog
        where RArg3 : class, Rog
        where ROut : class, Rog =>
        new(korssaFunction)
        {
            Captures = captures.ToArray()
        };
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

    public static Korssas.DefineMetaFunction<ROut> kMetaBoxed<ROut>(this IKorssa<ROut> korssa, IEnumerable<Addr> captures)
        where ROut : class, Rog =>
        Core.kMetaFunction<ROut>(captures, () => korssa);
}