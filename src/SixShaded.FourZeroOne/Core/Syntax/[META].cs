

namespace SixShaded.FourZeroOne.Core.Syntax
{
    using Resolutions;
    public static partial class Core
    {
        public static Tokens.Fixed<MetaFunction<ROut>> tMetaFunction<ROut>(Func<IToken<ROut>> tokenFunction) where ROut : class, Res
        {
            var vs = new DynamicAddress<MetaFunction<ROut>>();
            return new(new() { SelfIdentifier = vs, Token = tokenFunction() });
        }
        public static Tokens.Fixed<MetaFunction<RArg1, ROut>> tMetaFunction<RArg1, ROut>(Func<DynamicAddress<RArg1>, IToken<ROut>> tokenFunction)
            where RArg1 : class, Res
            where ROut : class, Res
        {
            var vs = new DynamicAddress<MetaFunction<RArg1, ROut>>();
            var v1 = new DynamicAddress<RArg1>();
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, Token = tokenFunction(v1) });
        }
        public static Tokens.Fixed<MetaFunction<RArg1, RArg2, ROut>> tMetaFunction<RArg1, RArg2, ROut>(Func<DynamicAddress<RArg1>, DynamicAddress<RArg2>, IToken<ROut>> tokenFunction)
            where RArg1 : class, Res
            where RArg2 : class, Res
            where ROut : class, Res
        {
            var vs = new DynamicAddress<MetaFunction<RArg1, RArg2, ROut>>();
            var (v1, v2) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, Token = tokenFunction(v1, v2) });
        }
        public static Tokens.Fixed<MetaFunction<RArg1, RArg2, RArg3, ROut>> tMetaFunction<RArg1, RArg2, RArg3, ROut>(Func<DynamicAddress<RArg1>, DynamicAddress<RArg2>, DynamicAddress<RArg3>, IToken<ROut>> tokenFunction)
            where RArg1 : class, Res
            where RArg2 : class, Res
            where RArg3 : class, Res
            where ROut : class, Res
        {
            var vs = new DynamicAddress<MetaFunction<RArg1, RArg2, RArg3, ROut>>();
            var (v1, v2, v3) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>(), new DynamicAddress<RArg3>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, IdentifierC = v3, Token = tokenFunction(v1, v2, v3) });
        }
        public static Tokens.Fixed<MetaFunction<ROut>> tMetaRecursiveFunction<ROut>(Func<DynamicAddress<MetaFunction<ROut>>, IToken<ROut>> tokenFunction) where ROut : class, Res
        {
            var vs = new DynamicAddress<MetaFunction<ROut>>();
            return new(new() { SelfIdentifier = vs, Token = tokenFunction(vs) });
        }
        public static Tokens.Fixed<MetaFunction<RArg1, ROut>> tMetaRecursiveFunction<RArg1, ROut>(Func<DynamicAddress<MetaFunction<RArg1, ROut>>, DynamicAddress<RArg1>, IToken<ROut>> tokenFunction)
            where RArg1 : class, Res
            where ROut : class, Res
        {
            var vs = new DynamicAddress<MetaFunction<RArg1, ROut>>();
            var v1 = new DynamicAddress<RArg1>();
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, Token = tokenFunction(vs, v1) });
        }
        public static Tokens.Fixed<MetaFunction<RArg1, RArg2, ROut>> tMetaRecursiveFunction<RArg1, RArg2, ROut>(Func<DynamicAddress<MetaFunction<RArg1, RArg2, ROut>>, DynamicAddress<RArg1>, DynamicAddress<RArg2>, IToken<ROut>> tokenFunction)
            where RArg1 : class, Res
            where RArg2 : class, Res
            where ROut : class, Res
        {
            var vs = new DynamicAddress<MetaFunction<RArg1, RArg2, ROut>>();
            var (v1, v2) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, Token = tokenFunction(vs, v1, v2) });
        }
        public static Tokens.Fixed<MetaFunction<RArg1, RArg2, RArg3, ROut>> tMetaRecursiveFunction<RArg1, RArg2, RArg3, ROut>(Func<DynamicAddress<MetaFunction<RArg1, RArg2, RArg3, ROut>>, DynamicAddress<RArg1>, DynamicAddress<RArg2>, DynamicAddress<RArg3>, IToken<ROut>> tokenFunction)
            where RArg1 : class, Res
            where RArg2 : class, Res
            where RArg3 : class, Res
            where ROut : class, Res
        {
            var vs = new DynamicAddress<MetaFunction<RArg1, RArg2, RArg3, ROut>>();
            var (v1, v2, v3) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>(), new DynamicAddress<RArg3>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, IdentifierC = v3, Token = tokenFunction(vs, v1, v2, v3) });
        }
    }
    public static partial class TokenSyntax
    {
        public static Tokens.Execute<R> tExecute<R>(this IToken<MetaFunction<R>> source) where R : class, Res
        { return new(source); }
        public static Tokens.Execute<RArg1, ROut> tExecuteWith<RArg1, ROut>(this IToken<MetaFunction<RArg1, ROut>> source, Structure.Token.Args<RArg1> args)
            where RArg1 : class, Res
            where ROut : class, Res
        { return new(source, new Tokens.ToBoxedArgs<RArg1>(args.A)); }
        public static Tokens.Execute<RArg1, RArg2, ROut> tExecuteWith<RArg1, RArg2, ROut>(this IToken<MetaFunction<RArg1, RArg2, ROut>> source, Structure.Token.Args<RArg1, RArg2> args)
            where RArg1 : class, Res
            where RArg2 : class, Res
            where ROut : class, Res
        { return new(source, new Tokens.ToBoxedArgs<RArg1, RArg2>(args.A, args.B)); }
        public static Tokens.Execute<RArg1, RArg2, RArg3, ROut> tExecuteWith<RArg1, RArg2, RArg3, ROut>(this IToken<MetaFunction<RArg1, RArg2, RArg3, ROut>> source, Structure.Token.Args<RArg1, RArg2, RArg3> args)
            where RArg1 : class, Res
            where RArg2 : class, Res
            where RArg3 : class, Res
            where ROut : class, Res
        { return new(source, new Tokens.ToBoxedArgs<RArg1, RArg2, RArg3>(args.A, args.B, args.C)); }

        public static Tokens.Fixed<MetaFunction<ROut>> tMetaBoxed<ROut>(this IToken<ROut> token) where ROut : class, Res
        {
            return new(new() { SelfIdentifier = new(), Token = token });
        }
    }
}
