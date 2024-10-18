
using Perfection;
using System.Collections.Generic;
namespace FourZeroOne.Core.Syntax
{
    using r = Resolutions;
    using ro = Resolutions.Objects;
    using Tokens;
    using Proxies;
    using Token;
    using Proxy;
    using Resolution;
    using ResObj = Resolution.IResolution;
    using IToken = Token.Unsafe.IToken;
    public sealed record RHint<R> where R : class, ResObj
    {
        public static RHint<R> Hint() => new();
    }
    public sealed record RHint<R1, R2>
        where R1 : class, ResObj
        where R2 : class, ResObj
    {
        public static RHint<R1, R2> Hint() => new();
    }
    public sealed record RHint<R1, R2, R3>
        where R1 : class, ResObj
        where R2 : class, ResObj
        where R3 : class, ResObj
    {
        public static RHint<R1, R2, R3> Hint() => new();
    }
    public sealed record RHint<R1, R2, R3, R4>
        where R1 : class, ResObj
        where R2 : class, ResObj
        where R3 : class, ResObj
        where R4 : class, ResObj
    {
        public static RHint<R1, R2, R3, R4> Hint() => new();
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
                public required IToken<r.Boxed.MetaFunction<R>> Then { get; init; }
                public required IToken<r.Boxed.MetaFunction<R>> Else { get; init; }
            }

            public sealed record SubEnvironment<R> where R : class, ResObj
            {
                public required IToken<IMulti<ResObj>> Environment { get; init; }
                public required IToken<R> SubToken { get; init; }
            }
        }
    }
    
    public static class CoreT
    {

        public static SubEnvironment<R> tSubEnvironment<R>(RHint<R> _, Structure.Token.SubEnvironment<R> block) where R : class, ResObj
        { return new(block.Environment, block.SubToken); }

        public static Fixed<r.Boxed.MetaFunction<ROut>> tMetaFunction<ROut>(RHint<ROut> _, Func<IToken<ROut>> tokenFunction) where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<ROut>>();
            return new(new() { SelfIdentifier = vs, Token = tokenFunction() });
        }
        public static Fixed<r.Boxed.MetaFunction<RArg1, ROut>> tMetaFunction<RArg1, ROut>(RHint<RArg1, ROut> _, Func<DynamicAddress<RArg1>, IToken<ROut>> tokenFunction)
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, ROut>>();
            var v1 = new DynamicAddress<RArg1>();
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, Token = tokenFunction(v1) });
        }
        public static Fixed<r.Boxed.MetaFunction<RArg1, RArg2, ROut>> tMetaFunction<RArg1, RArg2, ROut>(RHint<RArg1, RArg2, ROut> _, Func<DynamicAddress<RArg1>, DynamicAddress<RArg2>, IToken<ROut>> tokenFunction)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, ROut>>();
            var (v1, v2) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, Token = tokenFunction(v1, v2) });
        }
        public static Fixed<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>> tMetaFunction<RArg1, RArg2, RArg3, ROut>(RHint<RArg1, RArg2, RArg3, ROut> _, Func<DynamicAddress<RArg1>, DynamicAddress<RArg2>, DynamicAddress<RArg3>, IToken<ROut>> tokenFunction)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>>();
            var (v1, v2, v3) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>(), new DynamicAddress<RArg3>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, IdentifierC = v3, Token = tokenFunction(v1, v2, v3) });
        }
        public static Fixed<r.Boxed.MetaFunction<ROut>> tMetaRecursiveFunction<ROut>(RHint<ROut> _, Func<DynamicAddress<r.Boxed.MetaFunction<ROut>>, IToken<ROut>> tokenFunction) where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<ROut>>();
            return new(new() { SelfIdentifier = vs, Token = tokenFunction(vs) });
        }
        public static Fixed<r.Boxed.MetaFunction<RArg1, ROut>> tMetaRecursiveFunction<RArg1, ROut>(RHint<RArg1, ROut> _, Func<DynamicAddress<r.Boxed.MetaFunction<RArg1, ROut>>, DynamicAddress<RArg1>, IToken<ROut>> tokenFunction)
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, ROut>>();
            var v1 = new DynamicAddress<RArg1>();
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, Token = tokenFunction(vs, v1) });
        }
        public static Fixed<r.Boxed.MetaFunction<RArg1, RArg2, ROut>> tMetaRecursiveFunction<RArg1, RArg2, ROut>(RHint<RArg1, RArg2, ROut> _, Func<DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, ROut>>, DynamicAddress<RArg1>, DynamicAddress<RArg2>, IToken<ROut>> tokenFunction)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, ROut>>();
            var (v1, v2) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, Token = tokenFunction(vs, v1, v2) });
        }
        public static Fixed<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>> tMetaRecursiveFunction<RArg1, RArg2, RArg3, ROut>(RHint<RArg1, RArg2, RArg3, ROut> _, Func<DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>>, DynamicAddress<RArg1>, DynamicAddress<RArg2>, DynamicAddress<RArg3>, IToken<ROut>> tokenFunction)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>>();
            var (v1, v2, v3) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>(), new DynamicAddress<RArg3>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, IdentifierC = v3, Token = tokenFunction(vs, v1, v2, v3) });
        }

        public static Nolla<R> tNolla<R>(RHint<R> _) where R : class, ResObj
        { return new(); }

        public static Tokens.Multi.Union<R> tArrayOf<R>(RHint<R> _, List<IToken<R>> tokens) where R : class, ResObj
        { return new(tokens.Map(x => x.tYield())); }
        public static Tokens.Multi.Union<R> tUnion<R>(RHint<R> _, List<IToken<IMulti<R>>> sets) where R : class, ResObj
        { return new(sets); }
        public static Tokens.Multi.Intersection<R> tIntersection<R>(RHint<R> _, List<IToken<IMulti<R>>> sets) where R : class, ResObj
        { return new(sets); }
    }
    public static class _ExtensionsT
    {
        public static Tokens.IO.Select.One<R> tIO_SelectOne<R>(this IToken<IMulti<R>> source) where R : class, ResObj
        { return new(source); }
        public static Tokens.IO.Select.Multiple<R> tIO_SelectMany<R>(this IToken<IMulti<R>> source, IToken<ro.Number> count) where R : class, ResObj
        { return new(source, count); }

        public static Tokens.Execute<R> tExecute<R>(this IToken<r.Boxed.MetaFunction<R>> source) where R : class, ResObj
        { return new(source); }
        public static Tokens.Execute<RArg1, ROut> tExecuteWith<RArg1, ROut>(this IToken<r.Boxed.MetaFunction<RArg1, ROut>> source, Structure.Token.Args<RArg1> args)
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        { return new(source, new ToBoxedArgs<RArg1>(args.A)); }
        public static Tokens.Execute<RArg1, RArg2, ROut> tExecuteWith<RArg1, RArg2, ROut>(this IToken<r.Boxed.MetaFunction<RArg1, RArg2, ROut>> source, Structure.Token.Args<RArg1, RArg2> args)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        { return new(source, new ToBoxedArgs<RArg1, RArg2>(args.A, args.B)); }
        public static Tokens.Execute<RArg1, RArg2, RArg3, ROut> tExecuteWith<RArg1, RArg2, RArg3, ROut>(this IToken<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>> source, Structure.Token.Args<RArg1, RArg2, RArg3> args)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        { return new(source, new ToBoxedArgs<RArg1, RArg2, RArg3>(args.A, args.B, args.C)); }

        public static Fixed<r.Boxed.MetaFunction<ROut>> tMetaBoxed<ROut>(this IToken<ROut> token) where ROut : class, ResObj
        {
            return new(new() { SelfIdentifier = new(), Token = token });
        }
        public static DynamicAssign<R> tAsVariable<R>(this IToken<R> token, out DynamicAddress<R> ident) where R : class, ResObj
        {
            ident = new();
            return new(ident, token);
        }
        public static DynamicReference<R> tRef<R>(this DynamicAddress<R> ident) where R : class, ResObj
        { return new(ident); }
        
        public static IfElse<R> tIfTrue<R>(this IToken<ro.Bool> condition, RHint<R> _, Structure.Token.IfElse<R> block) where R : class, ResObj
        {
            return new(condition, block.Then, block.Else);
        }
        public static Tokens.Multi.Exclusion<R> tWithout<R>(this IToken<IMulti<R>> source, IToken<IMulti<R>> exclude) where R : class, ResObj
        { return new(source, exclude); }
        public static Tokens.Multi.Count tCount(this IToken<IMulti<ResObj>> source)
        { return new(source); }
        public static Tokens.Multi.Yield<R> tYield<R>(this IToken<R> token) where R : class, ResObj
        { return new(token); }
        public static Tokens.Multi.Union<R> tToMulti<R>(this IEnumerable<IToken<R>> tokens) where R : class, ResObj
        { return new(tokens.Map(x => x.tYield())); }
        public static Tokens.Multi.Union<R> tUnioned<R>(this IEnumerable<IToken<IMulti<R>>> tokens) where R : class, ResObj
        { return new(tokens); }
        public static Tokens.Multi.Contains<R> tContains<R>(this IToken<IMulti<R>> from, IToken<R> element) where R : class, ResObj
        { return new(from, element); }
        /// <summary>
        /// (1-based)
        /// </summary>
        public static Tokens.Multi.GetIndex<R> tGetIndex<R>(this IToken<IMulti<R>> token, IToken<ro.Number> index) where R : class, ResObj
        { return new(token, index); }
        public static Macros.Multi.Map<RIn, ROut> tMap<RIn, ROut>(this IToken<IMulti<RIn>> source, Func<DynamicAddress<RIn>, IToken<ROut>> mapFunction)
            where RIn : class, ResObj
            where ROut : class, ResObj
        {
            return new(source, CoreT.tr.Boxed.MetaFunction(new(), mapFunction));
        }

        public static Tokens.Number.Add tAdd(this IToken<ro.Number> a, IToken<ro.Number> b) 
        { return new(a, b); }
        public static Tokens.Number.Subtract tSubtract(this IToken<ro.Number> a, IToken<ro.Number> b) 
        { return new(a, b); }
        public static Tokens.Number.Multiply tMultiply(this IToken<ro.Number> a, IToken<ro.Number> b) 
        { return new(a, b); }
        public static Tokens.Number.Negate tNegative(this IToken<ro.Number> a)
        { return new(a); }
        public static Tokens.Number.Compare.GreaterThan tIsGreaterThan(this IToken<ro.Number> a, IToken<ro.Number> b)
        { return new(a, b); }
        public static Fixed<ro.Number> tFixed(this int value)
        { return new(value); }
        public static Fixed<R> tFixed<R>(this R value) where R : class, ResObj
        { return new(value); }
        public static Fixed<r.Multi<R>> tToConstMulti<R>(this IEnumerable<Tokens.Fixed<R>> values) where R : class, ResObj
        { return new(new() { Values = values.Map(x => x.Resolution) }); }

        public static Tokens.Component.Get<H, C> tGetComponent<H, C>(this IToken<H> holder, IComponentIdentifier<H, C> componentIdentifier)
            where H : class, IComposition<H>
            where C : class, ResObj
        { return new(componentIdentifier, holder); }
        public static Tokens.Component.With<H, C> tWithComponent<H, C>(this IToken<H> holder, IComponentIdentifier<H, C> componentIdentifier, IToken<C> component)
            where H : class, IComposition<H>
            where C : class, ResObj
        { return new(componentIdentifier, holder, component); }
        public static Tokens.Component.Without<H> tWithoutComponent<H>(this IToken<H> holder, Resolution.Unsafe.IComponentIdentifier<H> componentIdentifier)
            where H : class, IComposition<H>
        { return new(componentIdentifier, holder); }

        public static Tokens.Data.Insert<RAddress, RObj> tWriteTo<RAddress, RObj>(this IToken<RObj> subject, IToken<RAddress> address)
            where RAddress : class, IStateAddress<RObj>, ResObj
            where RObj : class, ResObj
        { return new(address, subject); }
        public static Tokens.Data.Get<RAddress, RObj> tGetData<RAddress, RObj>(this IToken<RAddress> address)
            where RAddress : class, IStateAddress<RObj>, ResObj
            where RObj : class, ResObj
        { return new(address); }
        public static Tokens.Data.Remove<RAddress> tRedact<RAddress>(this IToken<RAddress> address)
            where RAddress : class, Resolution.Unsafe.IStateAddress, ResObj
        { return new(address); }
    }
}