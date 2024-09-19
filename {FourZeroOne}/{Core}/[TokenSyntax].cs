
using Perfection;
using System.Collections.Generic;
namespace FourZeroOne.Core.TokenSyntax
{
    using Tokens;
    using FourZeroOne.Token;
    using Resolutions;
    using r = Resolutions;
    using ResObj = Resolution.IResolution;
    using IToken = Token.Unsafe.IToken;
    namespace TokenStructure
    {
        public sealed record IfElse<R> where R : class, ResObj
        {
            public IToken<r.Action<R>> Then { get; init; }
            public IToken<r.Action<R>> Else { get; init; }
        }
        public sealed record SubEnvironment<R> where R : class, ResObj
        {
            public IToken<Resolution.IMulti<ResObj>> Environment { get; init; }
            public IToken<R> SubToken { get; init; }
        }
        public sealed record Recursive<RArg1, ROut>
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            public IToken<RArg1> A { get; init; }
            public System.Func<ProxySyntax.OriginalHint<Tokens.Recursive<RArg1, ROut>>, Proxy.IProxy<Tokens.Recursive<RArg1, ROut>, ROut>> RecursiveProxyStatement { get; init; }
        }
        public sealed record Recursive<RArg1, RArg2, ROut>
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            public IToken<RArg1> A { get; init; }
            public IToken<RArg2> B { get; init; }
            public System.Func<ProxySyntax.OriginalHint<Tokens.Recursive<RArg1, RArg2, ROut>>, Proxy.IProxy<Tokens.Recursive<RArg1, RArg2, ROut>, ROut>> RecursiveProxyStatement { get; init; }
        }
        public sealed record Recursive<RArg1, RArg2, RArg3, ROut>
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        {
            public IToken<RArg1> A { get; init; }
            public IToken<RArg2> B { get; init; }
            public IToken<RArg3> C { get; init; }
            public System.Func<ProxySyntax.OriginalHint<Tokens.Recursive<RArg1, RArg2, RArg3, ROut>>, Proxy.IProxy<Tokens.Recursive<RArg1, RArg2, RArg3, ROut>, ROut>> RecursiveProxyStatement { get; init; }
        }
    }
    public static class MakeToken
    {
        public static Tokens.Board.Unit.AllUnits AllUnits()
        { return new(); }
        public static Tokens.Board.Hex.AllHexes AllHexes()
        { return new(); }

        public static SubEnvironment<R> tSubEnvironment<R>(TokenStructure.SubEnvironment<R> block) where R : class, ResObj
        { return new(block.Environment, block.SubToken); }

        public static Recursive<RArg1, ROut> tRecursive<RArg1, ROut>(TokenStructure.Recursive<RArg1, ROut> block)
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        { return new(block.A, block.RecursiveProxyStatement(new())); }
        public static Recursive<RArg1, RArg2, ROut> tRecursive<RArg1, RArg2, ROut>(TokenStructure.Recursive<RArg1, RArg2, ROut> block)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        { return new(block.A, block.B, block.RecursiveProxyStatement(new())); }
        public static Recursive<RArg1, RArg2, RArg3, ROut> tRecursive<RArg1, RArg2, RArg3, ROut>(TokenStructure.Recursive<RArg1, RArg2, RArg3, ROut> block)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        { return new(block.A, block.B, block.C, block.RecursiveProxyStatement(new())); }

        public static Nolla<R> tNolla<R>() where R : class, ResObj
        { return new(); }

        public static Tokens.Multi.Union<R> tArrayOf<R>(List<IToken<R>> tokens) where R : class, ResObj
        { return new(tokens.Map(x => x.tYield())); }
        public static Tokens.Multi.Union<R> tUnion<R>(List<IToken<Multi<R>>> sets) where R : class, ResObj
        { return new(sets); }
        public static Tokens.Multi.Intersection<R> tIntersection<R>(List<IToken<Multi<R>>> sets) where R : class, ResObj
        { return new(sets); }
    }
    public static class _Extensions
    {
        public static Tokens.IO.Select.One<R> tIO_SelectOne<R>(this IToken<Multi<R>> source) where R : class, ResObj
        { return new(source); }
        public static Tokens.IO.Select.Multiple<R> tIO_SelectMany<R>(this IToken<Multi<R>> source, IToken<Number> count) where R : class, ResObj
        { return new(source, count); }

        public static Variable<R> tAs<R>(this IToken<R> token, out VariableIdentifier<R> ident) where R : class, ResObj
        {
            ident = new();
            return new Variable<R>(ident, token);
        }
        public static Reference<R> tRef<R>(this VariableIdentifier<R> ident) where R : class, ResObj
        { return new(ident); }
        
        public static IfElse<R> tIfTrue<R>(this IToken<Bool> condition, TokenStructure.IfElse<R> block) where R : class, ResObj
        {
            return new(condition, block.Then, block.Else);
        }
        public static Fixed<r.Action<R>> tAsAction<R>(this IToken<R> token) where R : class, ResObj
        {
            return new(new() { Token = token });
        }
        public static Tokens.Multi.Exclusion<R> tWithout<R>(this IToken<Multi<R>> source, IToken<Multi<R>> exclude) where R : class, ResObj
        { return new(source, exclude); }
        public static Tokens.Multi.Count tCount(this IToken<Resolution.IMulti<ResObj>> source)
        { return new(source); }
        public static Tokens.Multi.Yield<R> tYield<R>(this IToken<R> token) where R : class, ResObj
        { return new(token); }
        public static Tokens.Multi.Union<R> tToMulti<R>(this IEnumerable<IToken<R>> tokens) where R : class, ResObj
        { return new(tokens.Map(x => x.tYield())); }
        public static Tokens.Multi.Union<R> tUnioned<R>(this IEnumerable<IToken<Multi<R>>> tokens) where R : class, ResObj
        { return new(tokens); }

        public static Tokens.Number.Add tAdd(this IToken<Number> a, IToken<Number> b) 
        { return new(a, b); }
        public static Tokens.Number.Subtract tSubtract(this IToken<Number> a, IToken<Number> b) 
        { return new(a, b); }
        public static Tokens.Number.Multiply tMultiply(this IToken<Number> a, IToken<Number> b) 
        { return new(a, b); }
        public static Tokens.Number.Negate tNegative(this IToken<Number> a)
        { return new(a); }
        public static Tokens.Number.Compare.GreaterThan tIsGreaterThan(this IToken<Number> a, IToken<Number> b)
        { return new(a, b); }
        public static Fixed<Number> tConst(this int value)
        { return new(value); }

        public static Tokens.Board.Unit.Get.HP tGetHP(this IToken<r.Board.Unit> unit)
        { return new(unit); }
        
        
    }
}