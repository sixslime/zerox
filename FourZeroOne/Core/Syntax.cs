
using Perfection;
using System.Collections.Generic;
namespace FourZeroOne.Core.Syntax
{
    using r = Resolutions;
    using t = Tokens;
    using p = Proxies;
    using tM = Macros;
    using ro = Resolutions.Objects;
    using Token;
    using Proxy;
    using Resolution;
    using ResObj = Resolution.IResolution;
    using IToken = Token.Unsafe.IToken;
    public interface IOriginalHint<TOrig, out TOrig_> where TOrig : IToken where TOrig_ : IToken { }
    public delegate IProxy<TOrig, R> ProxyBuilder<TOrig, R>(OriginalHint<TOrig> hint) where TOrig : IToken where R : class, ResObj;
    public sealed record OriginalHint<TOrig> : IOriginalHint<TOrig, TOrig> where TOrig : IToken { }
    public sealed record RHint<R1>
        where R1 : class, ResObj
    {
        [Obsolete("Use the static property 'HINT'")]
        public static RHint<R1> Hint() => new();
        public static readonly RHint<R1> HINT = new();
    }
    public sealed record RHint<R1, R2>
        where R1 : class, ResObj
        where R2 : class, ResObj
    {
        [Obsolete("Use the static property 'HINT'")]
        public static RHint<R1, R2> Hint() => new();
        public static readonly RHint<R1, R2> HINT = new();
    }
    public sealed record RHint<R1, R2, R3>
        where R1 : class, ResObj
        where R2 : class, ResObj
        where R3 : class, ResObj
    {
        [Obsolete("Use the static property 'HINT'")]
        public static RHint<R1, R2, R3> Hint() => new();
        public static readonly RHint<R1, R2, R3> HINT = new();
    }
    public sealed record RHint<R1, R2, R3, R4>
        where R1 : class, ResObj
        where R2 : class, ResObj
        where R3 : class, ResObj
        where R4 : class, ResObj
    {
        [Obsolete("Use the static property 'HINT'")]
        public static RHint<R1, R2, R3, R4> Hint() => new();
        public static readonly RHint<R1, R2, R3, R4> HINT = new();
    }

    namespace Structure
    {
        namespace Token
        {
            public sealed record Args<RArg1>
                where RArg1 : class, ResObj
            {
                public required IToken<RArg1> A { get; init; }
            }
            public sealed record Args<RArg1, RArg2>
                where RArg1 : class, ResObj
                where RArg2 : class, ResObj
            {
                public required IToken<RArg1> A { get; init; }
                public required IToken<RArg2> B { get; init; }
            }
            public sealed record Args<RArg1, RArg2, RArg3>
                where RArg1 : class, ResObj
                where RArg2 : class, ResObj
                where RArg3 : class, ResObj
            {
                public required IToken<RArg1> A { get; init; }
                public required IToken<RArg2> B { get; init; }
                public required IToken<RArg3> C { get; init; }
            }
            public sealed record IfElse<R> where R : class, ResObj
            {
                public required IToken<R> Then { get; init; }
                public required IToken<R> Else { get; init; }
            }

            public sealed record SubEnvironment<R> where R : class, ResObj
            {
                public required IToken<ResObj> Environment { get; init; }
                public required IToken<R> Value { get; init; }
            }
        }
        namespace Proxy
        {
            public sealed record Args<TOrig, RArg1>
                where TOrig : IToken
                where RArg1 : class, ResObj
            {
                public required IProxy<TOrig, RArg1> A { get; init; }
            }
            public sealed record Args<TOrig, RArg1, RArg2>
                where TOrig : IToken
                where RArg1 : class, ResObj
                where RArg2 : class, ResObj
            {
                public required IProxy<TOrig, RArg1> A { get; init; }
                public required IProxy<TOrig, RArg2> B { get; init; }
            }
            public sealed record Args<TOrig, RArg1, RArg2, RArg3>
                where TOrig : IToken
                where RArg1 : class, ResObj
                where RArg2 : class, ResObj
                where RArg3 : class, ResObj
            {
                public required IProxy<TOrig, RArg1> A { get; init; }
                public required IProxy<TOrig, RArg2> B { get; init; }
                public required IProxy<TOrig, RArg3> C { get; init; }
            }

            public sealed record IfElse<TOrig, R> where TOrig : IToken where R : class, ResObj
            {
                public required IProxy<TOrig, R> Then { get; init; }
                public required IProxy<TOrig, R> Else { get; init; }
            }

            public sealed record SubEnvironment<TOrig, R> where TOrig : IToken where R : class, ResObj
            {
                public required IProxy<TOrig, ResObj> Environment { get; init; }
                public required IProxy<TOrig, R> Value { get; init; }
            }
        }

    }

    public static class Core
    {
        public static t.SubEnvironment<R> tSubEnvironment<R>(RHint<R> _, Structure.Token.SubEnvironment<R> block) where R : class, ResObj
        { return new(block.Environment, block.Value); }
        public static p.Function<t.SubEnvironment<R>, TOrig, ResObj, R, R> pSubEnvironment<TOrig, R>(this OriginalHint<TOrig> _, RHint<R> __, Structure.Proxy.SubEnvironment<TOrig, R> block) where TOrig : IToken where R : class, ResObj
        { return new(block.Environment, block.Value); }
        public static t.Multi.Union<ResObj> t_Env(params IToken<ResObj>[] environment)
        { return new(environment.Map(x => x.tYield())); }
        public static p.Combiner<t.Multi.Union<ResObj>, TOrig, IMulti<ResObj>, r.Multi<ResObj>> p_Env<TOrig>(this OriginalHint<TOrig> _, params IProxy<TOrig, ResObj>[] environment) where TOrig : IToken
        { return new(environment.Map(x => x.pYield())); }
        public static t.Fixed<r.Boxed.MetaFunction<ROut>> tMetaFunction<ROut>(RHint<ROut> _, Func<IToken<ROut>> tokenFunction) where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<ROut>>();
            return new(new() { SelfIdentifier = vs, Token = tokenFunction() });
        }
        public static p.ToBoxedFunction<TOrig, ROut> pMetaFunction<TOrig, ROut>(this OriginalHint<TOrig> _, RHint<ROut> __, System.Func<IProxy<TOrig, ROut>> metaFunction)
            where TOrig : IToken
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<ROut>>();
            return new(metaFunction(), vs);
        }
        public static t.Fixed<r.Boxed.MetaFunction<RArg1, ROut>> tMetaFunction<RArg1, ROut>(RHint<RArg1, ROut> _, Func<DynamicAddress<RArg1>, IToken<ROut>> tokenFunction)
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, ROut>>();
            var v1 = new DynamicAddress<RArg1>();
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, Token = tokenFunction(v1) });
        }
        public static p.ToBoxedFunction<TOrig, RArg1, ROut> pMetaFunction<TOrig, RArg1, ROut>(this OriginalHint<TOrig> _, RHint<RArg1, ROut> __, System.Func<DynamicAddress<RArg1>, IProxy<TOrig, ROut>> metaFunction)
            where TOrig : IToken
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, ROut>>();
            var v1 = new DynamicAddress<RArg1>();
            return new(metaFunction(v1), vs, v1);
        }
        public static t.Fixed<r.Boxed.MetaFunction<RArg1, RArg2, ROut>> tMetaFunction<RArg1, RArg2, ROut>(RHint<RArg1, RArg2, ROut> _, Func<DynamicAddress<RArg1>, DynamicAddress<RArg2>, IToken<ROut>> tokenFunction)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, ROut>>();
            var (v1, v2) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, Token = tokenFunction(v1, v2) });
        }
        public static p.ToBoxedFunction<TOrig, RArg1, RArg2, ROut> pMetaFunction<TOrig, RArg1, RArg2, ROut>(this OriginalHint<TOrig> _, RHint<RArg1, RArg2, ROut> __, System.Func<DynamicAddress<RArg1>, DynamicAddress<RArg2>, IProxy<TOrig, ROut>> metaFunction)
            where TOrig : IToken
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, ROut>>();
            var (v1, v2) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>());
            return new(metaFunction(v1, v2), vs, v1, v2);
        }
        public static t.Fixed<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>> tMetaFunction<RArg1, RArg2, RArg3, ROut>(RHint<RArg1, RArg2, RArg3, ROut> _, Func<DynamicAddress<RArg1>, DynamicAddress<RArg2>, DynamicAddress<RArg3>, IToken<ROut>> tokenFunction)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>>();
            var (v1, v2, v3) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>(), new DynamicAddress<RArg3>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, IdentifierC = v3, Token = tokenFunction(v1, v2, v3) });
        }
        public static p.ToBoxedFunction<TOrig, RArg1, RArg2, RArg3, ROut> pMetaFunction<TOrig, RArg1, RArg2, RArg3, ROut>(this OriginalHint<TOrig> _, RHint<RArg1, RArg2, RArg3, ROut> __, System.Func<DynamicAddress<RArg1>, DynamicAddress<RArg2>, DynamicAddress<RArg3>, IProxy<TOrig, ROut>> metaFunction)
            where TOrig : IToken
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>>();
            var (v1, v2, v3) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>(), new DynamicAddress<RArg3>());
            return new(metaFunction(v1, v2, v3), vs, v1, v2, v3);
        }
        public static t.Fixed<r.Boxed.MetaFunction<ROut>> tMetaRecursiveFunction<ROut>(RHint<ROut> _, Func<DynamicAddress<r.Boxed.MetaFunction<ROut>>, IToken<ROut>> tokenFunction) where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<ROut>>();
            return new(new() { SelfIdentifier = vs, Token = tokenFunction(vs) });
        }
        public static p.ToBoxedFunction<TOrig, ROut> pMetaRecursiveFunction<TOrig, ROut>(this OriginalHint<TOrig> _, RHint<ROut> __, System.Func<DynamicAddress<r.Boxed.MetaFunction<ROut>>, IProxy<TOrig, ROut>> metaFunction)
            where TOrig : IToken
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<ROut>>();
            return new(metaFunction(vs), vs);
        }
        public static t.Fixed<r.Boxed.MetaFunction<RArg1, ROut>> tMetaRecursiveFunction<RArg1, ROut>(RHint<RArg1, ROut> _, Func<DynamicAddress<r.Boxed.MetaFunction<RArg1, ROut>>, DynamicAddress<RArg1>, IToken<ROut>> tokenFunction)
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, ROut>>();
            var v1 = new DynamicAddress<RArg1>();
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, Token = tokenFunction(vs, v1) });
        }
        public static p.ToBoxedFunction<TOrig, RArg1, ROut> pMetaRecursiveFunction<TOrig, RArg1, ROut>(this OriginalHint<TOrig> _, RHint<RArg1, ROut> __, System.Func<DynamicAddress<r.Boxed.MetaFunction<RArg1, ROut>>, DynamicAddress<RArg1>, IProxy<TOrig, ROut>> metaFunction)
            where TOrig : IToken
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, ROut>>();
            var v1 = new DynamicAddress<RArg1>();
            return new(metaFunction(vs, v1), vs, v1);
        }
        public static t.Fixed<r.Boxed.MetaFunction<RArg1, RArg2, ROut>> tMetaRecursiveFunction<RArg1, RArg2, ROut>(RHint<RArg1, RArg2, ROut> _, Func<DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, ROut>>, DynamicAddress<RArg1>, DynamicAddress<RArg2>, IToken<ROut>> tokenFunction)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, ROut>>();
            var (v1, v2) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, Token = tokenFunction(vs, v1, v2) });
        }
        public static p.ToBoxedFunction<TOrig, RArg1, RArg2, ROut> pMetaRecursiveFunction<TOrig, RArg1, RArg2, ROut>(this OriginalHint<TOrig> _, RHint<RArg1, RArg2, ROut> __, System.Func<DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, ROut>>, DynamicAddress<RArg1>, DynamicAddress<RArg2>, IProxy<TOrig, ROut>> metaFunction)
            where TOrig : IToken
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, ROut>>();
            var (v1, v2) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>());
            return new(metaFunction(vs, v1, v2), vs, v1, v2);
        }
        public static t.Fixed<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>> tMetaRecursiveFunction<RArg1, RArg2, RArg3, ROut>(RHint<RArg1, RArg2, RArg3, ROut> _, Func<DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>>, DynamicAddress<RArg1>, DynamicAddress<RArg2>, DynamicAddress<RArg3>, IToken<ROut>> tokenFunction)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>>();
            var (v1, v2, v3) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>(), new DynamicAddress<RArg3>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, IdentifierC = v3, Token = tokenFunction(vs, v1, v2, v3) });
        }
        public static p.ToBoxedFunction<TOrig, RArg1, RArg2, RArg3, ROut> pMetaRecursiveFunction<TOrig, RArg1, RArg2, RArg3, ROut>(this OriginalHint<TOrig> _, RHint<RArg1, RArg2, RArg3, ROut> __, System.Func<DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>>, DynamicAddress<RArg1>, DynamicAddress<RArg2>, DynamicAddress<RArg3>, IProxy<TOrig, ROut>> metaFunction)
            where TOrig : IToken
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>>();
            var (v1, v2, v3) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>(), new DynamicAddress<RArg3>());
            return new(metaFunction(vs, v1, v2, v3), vs, v1, v2, v3);
        }

        public static t.Multi.Union<R> tMultiOf<R>(RHint<R> _, List<IToken<R>> t) where R : class, ResObj
        { return new(t.Map(x => x.tYield())); }
        public static p.Combiner<t.Multi.Union<R>, TOrig, IMulti<R>, r.Multi<R>> pMultiOf<TOrig, R>(this OriginalHint<TOrig> _, RHint<R> __, IEnumerable<IProxy<TOrig, R>> array) where TOrig : IToken where R : class, ResObj
        {
            return array.pToMulti();
        }
        public static t.Multi.Union<R> tUnion<R>(RHint<R> _, List<IToken<IMulti<R>>> sets) where R : class, ResObj
        { return new(sets); }
        public static p.Combiner<t.Multi.Union<R>, TOrig, IMulti<R>, r.Multi<R>> pUnion<TOrig, R>(this OriginalHint<TOrig> _, RHint<R> __, IEnumerable<IProxy<TOrig, IMulti<R>>> array) where TOrig : IToken where R : class, ResObj
        {
            return array.pFlatten();
        }
        public static t.Multi.Intersection<R> tIntersection<R>(RHint<R> _, List<IToken<IMulti<R>>> sets) where R : class, ResObj
        { return new(sets); }
        public static p.Combiner<t.Multi.Intersection<R>, TOrig, IMulti<R>, r.Multi<R>> pIntersection<TOrig, R>(this OriginalHint<TOrig> _, RHint<R> __, IEnumerable<IProxy<TOrig, IMulti<R>>> sets) where TOrig : IToken where R : class, ResObj
        {
            return new(sets);
        }

        public static t.Nolla<R> tNolla<R>(RHint<R> _) where R : class, ResObj
        { return new(); }
        public static tM.Compose<C> tCompose<C>() where C : ICompositionType, new()
        {
            return new();
        }

    }
    public static class MakeProxy
    {
        public static IProxy<TOrig, ROut> Statement<TOrig, ROut>(ProxyBuilder<TOrig, ROut> statement) where TOrig : Token.IToken<ROut> where ROut : class, ResObj
        { return statement(new()); }
        public static Rule.RuleBehavior<TOrig, ROutMatch> AsRule<TOrig, ROutMatch>(IEnumerable<string> requiredLabels, ProxyBuilder<TOrig, ROutMatch> statement) where TOrig : Token.IToken<ROutMatch> where ROutMatch : class, ResObj
        { return new() { RequiredLabels = requiredLabels.ToPSet(), Proxy = statement(new()) }; }
    }
    public static class _Extensions
    {
        public static t.IO.Select.One<R> tIOSelectOne<R>(this IToken<IMulti<R>> source) where R : class, ResObj
        { return new(source); }
        public static p.Function<t.IO.Select.One<R>, TOrig, IMulti<R>, R> pIOSelectOne<TOrig, R>(this IProxy<TOrig, IMulti<R>> source) where TOrig : IToken where R : class, ResObj
        { return new(source); }
        public static t.IO.Select.Multiple<R> tIOSelectMany<R>(this IToken<IMulti<R>> source, IToken<ro.Number> count) where R : class, ResObj
        { return new(source, count); }
        public static p.Function<t.IO.Select.Multiple<R>, TOrig, IMulti<R>, ro.Number, r.Multi<R>> pIOSelectMany<TOrig, R>(this IProxy<TOrig, IMulti<R>> source, IProxy<TOrig, ro.Number> count) where TOrig : IToken where R : class, ResObj
        { return new(source, count); }

        public static t.Execute<R> tExecute<R>(this IToken<r.Boxed.MetaFunction<R>> source) where R : class, ResObj
        { return new(source); }
        public static p.Function<t.Execute<ROut>, TOrig, r.Boxed.MetaFunction<ROut>, ROut> pExecute<TOrig, ROut>(this IProxy<TOrig, r.Boxed.MetaFunction<ROut>> function)
            where TOrig : IToken
            where ROut : class, ResObj
        {
            return new(function);
        }
        public static t.Execute<RArg1, ROut> tExecuteWith<RArg1, ROut>(this IToken<r.Boxed.MetaFunction<RArg1, ROut>> source, Structure.Token.Args<RArg1> args)
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        { return new(source, new t.ToBoxedArgs<RArg1>(args.A)); }
        public static p.Function<t.Execute<RArg1, ROut>, TOrig, r.Boxed.MetaFunction<RArg1, ROut>, r.Boxed.MetaArgs<RArg1>, ROut> pExecuteWith<TOrig, RArg1, ROut>(this IProxy<TOrig, r.Boxed.MetaFunction<RArg1, ROut>> function, Structure.Proxy.Args<TOrig, RArg1> args)
            where TOrig : IToken
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            var argProxy = new p.Function<t.ToBoxedArgs<RArg1>, TOrig, RArg1, r.Boxed.MetaArgs<RArg1>>(args.A);
            return new(function, argProxy);
        }
        public static t.Execute<RArg1, RArg2, ROut> tExecuteWith<RArg1, RArg2, ROut>(this IToken<r.Boxed.MetaFunction<RArg1, RArg2, ROut>> source, Structure.Token.Args<RArg1, RArg2> args)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        { return new(source, new t.ToBoxedArgs<RArg1, RArg2>(args.A, args.B)); }
        public static p.Function<t.Execute<RArg1, RArg2, ROut>, TOrig, r.Boxed.MetaFunction<RArg1, RArg2, ROut>, r.Boxed.MetaArgs<RArg1, RArg2>, ROut> pExecuteWith<TOrig, RArg1, RArg2, ROut>(this IProxy<TOrig, r.Boxed.MetaFunction<RArg1, RArg2, ROut>> function, Structure.Proxy.Args<TOrig, RArg1, RArg2> args)
            where TOrig : IToken
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            var argProxy = new p.Function<t.ToBoxedArgs<RArg1, RArg2>, TOrig, RArg1, RArg2, r.Boxed.MetaArgs<RArg1, RArg2>>(args.A, args.B);
            return new(function, argProxy);
        }
        public static t.Execute<RArg1, RArg2, RArg3, ROut> tExecuteWith<RArg1, RArg2, RArg3, ROut>(this IToken<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>> source, Structure.Token.Args<RArg1, RArg2, RArg3> args)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        { return new(source, new t.ToBoxedArgs<RArg1, RArg2, RArg3>(args.A, args.B, args.C)); }
        public static p.Function<t.Execute<RArg1, RArg2, RArg3, ROut>, TOrig, r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>, r.Boxed.MetaArgs<RArg1, RArg2, RArg3>, ROut> pExecuteWith<TOrig, RArg1, RArg2, RArg3, ROut>(this IProxy<TOrig, r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>> function, Structure.Proxy.Args<TOrig, RArg1, RArg2, RArg3> args)
            where TOrig : IToken
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        {
            var argProxy = new p.Function<t.ToBoxedArgs<RArg1, RArg2, RArg3>, TOrig, RArg1, RArg2, RArg3, r.Boxed.MetaArgs<RArg1, RArg2, RArg3>>(args.A, args.B, args.C);
            return new(function, argProxy);
        }

        public static t.Fixed<r.Boxed.MetaFunction<ROut>> tMetaBoxed<ROut>(this IToken<ROut> token) where ROut : class, ResObj
        {
            return new(new() { SelfIdentifier = new(), Token = token });
        }
        public static p.ToBoxedFunction<TOrig, ROut> pMetaBoxed<TOrig, ROut>(this IProxy<TOrig, ROut> proxy)
            where TOrig : IToken
            where ROut : class, ResObj
        {
            return new(proxy, new());
        }

        public static t.Exists tExists(this IToken<ResObj> token)
        { return new(token); }
        public static p.Function<t.Exists, TOrig, ResObj, ro.Bool> pExists<TOrig>(this IProxy<TOrig, ResObj> value) where TOrig : IToken
        { return new(value); }

        public static t.DynamicAssign<R> tAsVariable<R>(this IToken<R> token, out DynamicAddress<R> ident) where R : class, ResObj
        {
            ident = new();
            return new(ident, token);
        }
        public static p.SpecialCase.DynamicAssign<TOrig, R> pAsVariable<TOrig, R>(this IProxy<TOrig, R> value, out DynamicAddress<R> identifier) where TOrig : IToken where R : class, ResObj
        {
            identifier = new();
            return new(identifier, value);
        }

        public static t.IfElse<R> tIfTrueDirect<R>(this IToken<ro.Bool> condition, RHint<R> _, Structure.Token.IfElse<r.Boxed.MetaFunction<R>> block) where R : class, ResObj
        { return new(condition, block.Then, block.Else); }
        public static p.Function<t.IfElse<R>, TOrig, ro.Bool, r.Boxed.MetaFunction<R>, r.Boxed.MetaFunction<R>, r.Boxed.MetaFunction<R>> pIfTrueDirect<TOrig, R>(this IProxy<TOrig, ro.Bool> condition, RHint<R> _, Structure.Proxy.IfElse<TOrig, r.Boxed.MetaFunction<R>> block) where TOrig : IToken where R : class, ResObj
        { return new(condition, block.Then, block.Else); }
        public static t.Execute<R> t_IfTrue<R>(this IToken<ro.Bool> condition, RHint<R> hint, Structure.Token.IfElse<R> block) where R : class, ResObj
        {
            return condition.tIfTrueDirect(hint, new()
            {
                Then = block.Then.tMetaBoxed(),
                Else = block.Else.tMetaBoxed()
            }).tExecute();
        }
        public static p.Function<t.Execute<R>, TOrig, r.Boxed.MetaFunction<R>, R> p_IfTrue<TOrig, R>(this IProxy<TOrig, ro.Bool> condition, RHint<R> hint, Structure.Proxy.IfElse<TOrig, R> block)
            where TOrig : IToken
            where R : class, ResObj
        {
            return condition.pIfTrueDirect(hint, new()
            {
                Then = block.Then.pMetaBoxed(),
                Else = block.Else.pMetaBoxed()
            }).pExecute();
        }
        public static t.Multi.Exclusion<R> tWithout<R>(this IToken<IMulti<R>> source, IToken<IMulti<R>> exclude) where R : class, ResObj
        { return new(source, exclude); }
        public static p.Function<t.Multi.Exclusion<R>, TOrig, IMulti<R>, IMulti<R>, r.Multi<R>> pWithout<TOrig, R>(this IProxy<TOrig, IMulti<R>> source, IProxy<TOrig, IMulti<R>> values) where TOrig : IToken where R : class, ResObj
        { return new(source, values); }
        public static t.Multi.Count tCount(this IToken<IMulti<ResObj>> source)
        { return new(source); }
        public static p.Function<t.Multi.Count, TOrig, IMulti<ResObj>, ro.Number> pCount<TOrig>(this IProxy<TOrig, IMulti<ResObj>> source) where TOrig : IToken
        { return new(source); }
        public static t.Multi.Yield<R> tYield<R>(this IToken<R> token) where R : class, ResObj
        { return new(token); }
        public static p.Function<t.Multi.Yield<R>, TOrig, R, r.Multi<R>> pYield<TOrig, R>(this IProxy<TOrig, R> source) where TOrig : IToken where R : class, ResObj
        { return new(source); }
        public static t.Multi.Union<R> tToMulti<R>(this IEnumerable<IToken<R>> t) where R : class, ResObj
        { return new(t.Map(x => x.tYield())); }
        public static p.Combiner<t.Multi.Union<R>, TOrig, IMulti<R>, r.Multi<R>> pToMulti<TOrig, R>(this IEnumerable<IProxy<TOrig, R>> values) where TOrig : IToken where R : class, ResObj
        { return new(values.Map(x => x.pYield())); }
        public static t.Multi.Union<R> tUnion<R>(this IToken<IMulti<R>> left, IToken<IMulti<R>> right) where R : class, ResObj
        { return new([left, right]); }
        public static p.Combiner<t.Multi.Union<R>, TOrig, IMulti<R>, r.Multi<R>> pUnion<TOrig, R>(this IProxy<TOrig, IMulti<R>> left, IProxy<TOrig, IMulti<R>> right) where TOrig : IToken where R : class, ResObj
        { return new([left, right]); }
        public static t.Multi.Union<R> tFlatten<R>(this IEnumerable<IToken<IMulti<R>>> t) where R : class, ResObj
        { return new(t); }
        public static p.Combiner<t.Multi.Union<R>, TOrig, IMulti<R>, r.Multi<R>> pFlatten<TOrig, R>(this IEnumerable<IProxy<TOrig, IMulti<R>>> values) where TOrig : IToken where R : class, ResObj
        { return new(values); }
        public static t.Multi.Contains<R> tContains<R>(this IToken<IMulti<R>> from, IToken<R> element) where R : class, ResObj
        { return new(from, element); }
        public static p.Function<t.Multi.Contains<R>, TOrig, IMulti<R>, R, ro.Bool> pContains<TOrig, R>(this IProxy<TOrig, IMulti<R>> source, IProxy<TOrig, R> element) where TOrig : IToken where R : class, ResObj
        { return new(source, element); }
        public static t.Multi.GetIndex<R> tGetIndex<R>(this IToken<IMulti<R>> token, IToken<ro.Number> index) where R : class, ResObj
        { return new(token, index); }
        public static p.Function<t.Multi.GetIndex<R>, TOrig, IMulti<R>, ro.Number, R> pGetIndex<TOrig, R>(this IProxy<TOrig, IMulti<R>> values, IProxy<TOrig, ro.Number> index) where TOrig : IToken where R : class, ResObj
        { return new(values, index); }
        public static tM.Multi.Map<RIn, ROut> tMap<RIn, ROut>(this IToken<IMulti<RIn>> source, Func<DynamicAddress<RIn>, IToken<ROut>> mapFunction)
            where RIn : class, ResObj
            where ROut : class, ResObj
        { return new(source, Core.tMetaFunction(new(), mapFunction)); }
        public static p.Function<tM.Multi.Map<RIn, ROut>, TOrig, IMulti<RIn>, r.Boxed.MetaFunction<RIn, ROut>, r.Multi<ROut>> pMap<TOrig, RIn, ROut>(this IProxy<TOrig, IMulti<RIn>> values, IProxy<TOrig, r.Boxed.MetaFunction<RIn, ROut>> mapFunction)
            where TOrig : IToken where RIn : class, ResObj where ROut : class, ResObj
        { return new(values, mapFunction); }
        public static tM.Multi.Duplicate<R> tDuplicate<R>(this IToken<R> value, IToken<ro.Number> count)
            where R : class, ResObj
        { return new(value, count); }
        public static p.Function<tM.Multi.Duplicate<R>, TOrig, R, ro.Number, r.Multi<R>> pDuplicate<TOrig, R>(this IProxy<TOrig, R> value, IProxy<TOrig, ro.Number> count)
            where TOrig : IToken
            where R : class, ResObj
        { return new(value, count); }

        public static t.Number.Add tAdd(this IToken<ro.Number> a, IToken<ro.Number> b)
        { return new(a, b); }
        public static p.Function<t.Number.Add, TOrig, ro.Number, ro.Number, ro.Number> pAdd<TOrig>(this IProxy<TOrig, ro.Number> a, IProxy<TOrig, ro.Number> b) where TOrig : IToken
        { return new(a, b); }
        public static t.Number.Subtract tSubtract(this IToken<ro.Number> a, IToken<ro.Number> b)
        { return new(a, b); }
        public static p.Function<t.Number.Subtract, TOrig, ro.Number, ro.Number, ro.Number> pSubtract<TOrig>(this IProxy<TOrig, ro.Number> a, IProxy<TOrig, ro.Number> b) where TOrig : IToken
        { return new(a, b); }
        public static t.Number.Multiply tMultiply(this IToken<ro.Number> a, IToken<ro.Number> b)
        { return new(a, b); }
        public static p.Function<t.Number.Multiply, TOrig, ro.Number, ro.Number, ro.Number> pMultiply<TOrig>(this IProxy<TOrig, ro.Number> a, IProxy<TOrig, ro.Number> b) where TOrig : IToken
        { return new(a, b); }
        public static t.Number.Compare.GreaterThan tIsGreaterThan(this IToken<ro.Number> a, IToken<ro.Number> b)
        { return new(a, b); }
        public static p.Function<t.Number.Compare.GreaterThan, TOrig, ro.Number, ro.Number, ro.Bool> pIsGreaterThan<TOrig>(this IProxy<TOrig, ro.Number> a, IProxy<TOrig, ro.Number> b) where TOrig : IToken
        { return new(a, b); }

        public static t.Component.Get<H, C> tGetComponent<H, C>(this IToken<ICompositionOf<H>> holder, IComponentIdentifier<H, C> componentIdentifier)
            where H : ICompositionType
            where C : class, ResObj
        { return new(componentIdentifier, holder); }
        public static p.SpecialCase.Component.Get<TOrig, H, C> pGetComponent<TOrig, H, C>(this IProxy<TOrig, ICompositionOf<H>> holder, IComponentIdentifier<H, C> identifier)
            where TOrig : IToken
            where H : ICompositionType
            where C : class, ResObj
        { return new(identifier, holder); }
        public static t.Component.With<H, C> tWithComponent<H, C>(this IToken<ICompositionOf<H>> holder, IComponentIdentifier<H, C> componentIdentifier, IToken<C> component)
            where H : ICompositionType
            where C : class, ResObj
        { return new(componentIdentifier, holder, component); }
        public static p.SpecialCase.Component.With<TOrig, H, C> pWithComponent<TOrig, H, C>(this IProxy<TOrig, ICompositionOf<H>> holder, IComponentIdentifier<H, C> identifier, IProxy<TOrig, C> component)
            where TOrig : IToken
            where H : ICompositionType
            where C : class, ResObj
        { return new(identifier, holder, component); }
        public static tM.UpdateComponent<H, C> tUpdateComponent<H, C>(this IToken<ICompositionOf<H>> holder, IComponentIdentifier<H, C> componentIdentifier, IToken<r.Boxed.MetaFunction<C, C>> changeFunc)
            where H : ICompositionType
            where C : class, ResObj
        { return new(componentIdentifier, holder, changeFunc); }
        public static tM.UpdateComponent<H, C> tUpdateComponent<H, C>(this IToken<ICompositionOf<H>> holder, IComponentIdentifier<H, C> componentIdentifier, Func<DynamicAddress<C>, IToken<C>> changeFunc)
            where H : ICompositionType
            where C : class, ResObj
        { return new(componentIdentifier, holder, Core.tMetaFunction(RHint<C, C>.Hint(), changeFunc)); }
        public static p.SpecialCase.Component.Update<TOrig, H, C> pUpdateComponent<TOrig, H, C>(this IProxy<TOrig, ICompositionOf<H>> holder, IComponentIdentifier<H, C> identifier, IProxy<TOrig, r.Boxed.MetaFunction<C, C>> changeFunc)
            where TOrig : IToken
            where H : ICompositionType
            where C : class, ResObj
        { return new(identifier, holder, changeFunc); }
        public static p.SpecialCase.Component.Update<TOrig, H, C> pUpdateComponent<TOrig, H, C>(this IProxy<TOrig, ICompositionOf<H>> holder, IComponentIdentifier<H, C> identifier, Func<DynamicAddress<C>, IProxy<TOrig, C>> changeFunc)
            where TOrig : IToken
            where H : ICompositionType
            where C : class, ResObj
        { return new(identifier, holder, Core.pMetaFunction(new(), new(), changeFunc)); }
        public static t.Component.Without<H> tWithoutComponent<H>(this IToken<ICompositionOf<H>> holder, Resolution.Unsafe.IComponentIdentifier<H> componentIdentifier)
            where H : ICompositionType
        { return new(componentIdentifier, holder); }
        public static p.SpecialCase.Component.Without<TOrig, H> pWithoutComponent<TOrig, H>(this IProxy<TOrig, ICompositionOf<H>> holder, Resolution.Unsafe.IComponentIdentifier<H> identifier)
            where TOrig : IToken
            where H : ICompositionType
        { return new(identifier, holder); }
        public static tM.Decompose<D> tDecompose<D>(this IToken<ICompositionOf<D>> composition) where D : IDecomposableType<D>, new()
        { return new(composition); }
        public static p.Function<tM.Decompose<D>, TOrig, ICompositionOf<D>, ResObj> pDecompose<TOrig, D>(this IProxy<TOrig, ICompositionOf<D>> composition) where D : IDecomposableType<D>, new() where TOrig : IToken
        { return new(composition); }

        public static t.Component.With<r.MergeSpec<H>, C> t_WithMerged<H, C>(this IToken<ICompositionOf<r.MergeSpec<H>>> mergeObject, IComponentIdentifier<H, C> mergingIdentifier, IToken<C> component)
            where H : ICompositionType
            where C : class, ResObj
        { return mergeObject.tWithComponent(r.MergeSpec<H>.MERGE(mergingIdentifier), component); }
        public static p.SpecialCase.Component.With<TOrig, r.MergeSpec<H>, C> p_MergeComponent<TOrig, H, C>(this IProxy<TOrig, ICompositionOf<r.MergeSpec<H>>> mergeObject, IComponentIdentifier<H, C> mergingIdentifier, IProxy<TOrig, C> component)
            where TOrig : IToken
            where H : ICompositionType
            where C : class, ResObj
        { return mergeObject.pWithComponent(r.MergeSpec<H>.MERGE(mergingIdentifier), component); }
        public static t.Component.DoMerge<H> tMerge<H>(this IToken<ICompositionOf<H>> subject, IToken<ICompositionOf<r.MergeSpec<H>>> mergeObject)
            where H : ICompositionType
        { return new(subject, mergeObject); }
        public static p.Function<t.Component.DoMerge<H>, TOrig, ICompositionOf<H>, ICompositionOf<r.MergeSpec<H>>, ICompositionOf<H>> pMerge<TOrig, H>(this IProxy<TOrig, ICompositionOf<H>> subject, IProxy<TOrig, ICompositionOf<r.MergeSpec<H>>> mergeObject)
            where TOrig : IToken
            where H : ICompositionType
        { return new(subject, mergeObject); }
        public static t.Data.Insert<RAddress, RObj> tDataWrite<RAddress, RObj>(this IToken<RAddress> address, IToken<RObj> data)
            where RAddress : class, IStateAddress<RObj>, ResObj
            where RObj : class, ResObj
        { return new(address, data); }
        public static p.Function<t.Data.Insert<RAddress, RObj>, TOrig, RAddress, RObj, r.Instructions.Assign<RObj>> pDataWrite<TOrig, RAddress, RObj>(this IProxy<TOrig, RAddress> address, IProxy<TOrig, RObj> data)
            where TOrig : IToken
            where RAddress : class, IStateAddress<RObj>, ResObj
            where RObj : class, ResObj
        { return new(address, data); }
        public static t.Data.Get<RAddress, RObj> tDataRead<RAddress, RObj>(this IToken<RAddress> address, RHint<RObj> _)
            where RAddress : class, IStateAddress<RObj>, ResObj
            where RObj : class, ResObj
        { return new(address); }
        public static p.Function<t.Data.Get<RAddress, RObj>, TOrig, RAddress, RObj> pRead<TOrig, RAddress, RObj>(this IProxy<TOrig, RAddress> address, RHint<RObj> _)
            where TOrig : IToken
            where RAddress : class, IStateAddress<RObj>, ResObj
            where RObj : class, ResObj
        { return new(address); }
        public static t.Data.Remove<RAddress> tDataRedact<RAddress>(this IToken<RAddress> address)
            where RAddress : class, Resolution.Unsafe.IStateAddress, ResObj
        { return new(address); }
        public static p.Function<t.Data.Remove<RAddress>, TOrig, RAddress, r.Instructions.Redact> pDataRedact<TOrig, RAddress>(this IProxy<TOrig, RAddress> address)
            where TOrig : IToken
            where RAddress : class, Resolution.Unsafe.IStateAddress, ResObj
        { return new(address); }
        public static tM.UpdateStateObject<RAddress, RObj> tDataUpdate<RAddress, RObj>(this IToken<RAddress> address, IToken<r.Boxed.MetaFunction<RObj, RObj>> updateFunction)
            where RAddress : class, IStateAddress<RObj>, ResObj
            where RObj : class, ResObj
        { return new(address, updateFunction); }
        public static tM.UpdateStateObject<RAddress, RObj> tDataUpdate<RAddress, RObj>(this IToken<RAddress> address, RHint<RObj> _, Func<DynamicAddress<RObj>, IToken<RObj>> updateFunction)
            where RAddress : class, IStateAddress<RObj>, ResObj
            where RObj : class, ResObj
        { return new(address, Core.tMetaFunction(RHint<RObj, RObj>.Hint(), updateFunction)); }
        public static p.Function<tM.UpdateStateObject<RAddress, RObj>, TOrig, RAddress, r.Boxed.MetaFunction<RObj, RObj>, r.Instructions.Assign<RObj>> pDataUpdate<TOrig, RAddress, RObj>(this IProxy<TOrig, RAddress> address, RHint<RObj> _, IProxy<TOrig, r.Boxed.MetaFunction<RObj, RObj>> updateFunction)
            where TOrig : IToken
            where RAddress : class, IStateAddress<RObj>, ResObj
            where RObj : class, ResObj
        { return new(address, updateFunction); }
        public static p.Function<tM.UpdateStateObject<RAddress, RObj>, TOrig, RAddress, r.Boxed.MetaFunction<RObj, RObj>, r.Instructions.Assign<RObj>> pDataUpdate<TOrig, RAddress, RObj>(this IProxy<TOrig, RAddress> address, RHint<RObj> _, Func<DynamicAddress<RObj>, IProxy<TOrig, RObj>> updateFunction)
            where TOrig : IToken
            where RAddress : class, IStateAddress<RObj>, ResObj
            where RObj : class, ResObj
        { return new(address, Core.pMetaFunction(new OriginalHint<TOrig>(), RHint<RObj, RObj>.Hint(), updateFunction)); }

        public static t.DynamicReference<R> tRef<R>(this DynamicAddress<R> ident) where R : class, ResObj
        { return new(ident); }
        public static t.Fixed<ro.Bool> tFixed(this bool value)
        { return new(value); }
        public static t.Fixed<ro.Number> tFixed(this int value)
        { return new(value); }
        public static t.Fixed<ro.NumRange> tFixed(this Range value)
        { return new(value); }
        public static t.Fixed<R> tFixed<R>(this R value) where R : class, ResObj
        { return new(value); }
        public static t.Fixed<r.Instructions.RuleAdd> tAddRule(this Rule.IRule value)
        {
            return new(new() { Rule = value });
        }

        public static p.Direct<TOrig, R> pDirect<TOrig, R>(this Token.IToken<R> token, OriginalHint<TOrig> _) where TOrig : IToken where R : class, ResObj
        { return new(token); }
        public static p.This<TOrig, R> pThis<TOrig, R>(this IOriginalHint<TOrig, IToken<R>> _) where TOrig : IToken<R> where R : class, ResObj
        { return new(); }
        public static p.ThisFunction<TOrig, RArg1, ROut> pThisWith<TOrig, RArg1, ROut>(this IOriginalHint<TOrig, IFunction<RArg1, ROut>> _, Structure.Proxy.Args<TOrig, RArg1> args)
            where TOrig : IFunction<RArg1, ROut>
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        { return new(args.A); }
        public static p.ThisFunction<TOrig, RArg1, RArg2, ROut> pThisWith<TOrig, RArg1, RArg2, ROut>(this IOriginalHint<TOrig, IFunction<RArg1, RArg2, ROut>> _, Structure.Proxy.Args<TOrig, RArg1, RArg2> args)
            where TOrig : IFunction<RArg1, RArg2, ROut>
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        { return new(args.A, args.B); }
        public static p.ThisFunction<TOrig, RArg1, RArg2, RArg3, ROut> pThisWith<TOrig, RArg1, RArg2, RArg3, ROut>(this IOriginalHint<TOrig, IFunction<RArg1, RArg2, RArg3, ROut>> _, Structure.Proxy.Args<TOrig, RArg1, RArg2, RArg3> args)
            where TOrig : IFunction<RArg1, RArg2, RArg3, ROut>
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        { return new(args.A, args.B, args.C); }
        public static p.OriginalArg1<TOrig, R> pOriginalA<TOrig, R>(this IOriginalHint<TOrig, Token.Unsafe.IHasArg1<R>> _) where TOrig : Token.Unsafe.IHasArg1<R> where R : class, ResObj
        { return new(); }
        public static p.OriginalArg2<TOrig, R> pOriginalB<TOrig, R>(this IOriginalHint<TOrig, Token.Unsafe.IHasArg2<R>> _) where TOrig : Token.Unsafe.IHasArg2<R> where R : class, ResObj
        { return new(); }
        public static p.OriginalArg3<TOrig, R> pOriginalC<TOrig, R>(this IOriginalHint<TOrig, Token.Unsafe.IHasArg3<R>> _) where TOrig : Token.Unsafe.IHasArg3<R> where R : class, ResObj
        { return new(); }
        public static t.Fixed<r.Multi<R>> t_ToConstMulti<R>(this IEnumerable<t.Fixed<R>> values) where R : class, ResObj
        { return new(new() { Values = values.Map(x => x.Resolution).ToPSequence() }); }

        public static IOption<ro.Number> rAsRes(this int v) => new ro.Number() { Value = v }.AsSome();
        public static IOption<ro.Bool> rAsRes(this bool v) => new ro.Bool() { IsTrue = v }.AsSome();
        public static IOption<r.Multi<R>> rAsRes<R>(this IEnumerable<IOption<R>> v) where R : class, ResObj
        {
            return new r.Multi<R>() { Values = v.FilterMap(x => x).ToPSequence() }.AsSome();
        }

        public static T SetLabels<T>(this T token, params string[] labels) where T : IToken
        {
            return token.dLabels(_ => labels.ToPSet());
        }
        public static P SetAddedLabels<P>(this P proxy, params string[] labels) where P : Proxy.Unsafe.IProxy
        {
            return proxy.dLabels(_ => labels.ToPSet());
        }
        public static P SetRemovedLabels<P, TOrig, R>(this P proxy, params string[] labels) where P : Proxy.Unsafe.IThisProxy
        {
            return proxy.dLabelRemovals(_ => labels.ToPSet());
        }
    }
}