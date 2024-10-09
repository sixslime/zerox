
using Perfection;
using System.Collections.Generic;
namespace FourZeroOne.Core.TokenSyntax
{
    using Tokens;
    using FourZeroOne.Token;
    using Resolutions.Objects;
    using Resolutions.Actions;
    using Resolutions;
    using r = Resolutions;
    using ResObj = Resolution.IResolution;
    using IToken = Token.Unsafe.IToken;
    using Resolutions.Boxed;
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
    namespace TokenStructure
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
            public IToken<MetaFunction<R>> Then { get; init; }
            public IToken<MetaFunction<R>> Else { get; init; }
        }

        public sealed record SubEnvironment<R> where R : class, ResObj
        {
            public IToken<Resolution.IMulti<ResObj>> Environment { get; init; }
            public IToken<R> SubToken { get; init; }
        }
        [Obsolete("Will be removed. Use self referencing MetaFunctions.", true)]
        public sealed record Recursive<RArg1, ROut>
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            public IToken<RArg1> A { get; init; }
            public System.Func<ProxySyntax.OriginalHint<Tokens.Recursive<RArg1, ROut>>, Proxy.IProxy<Tokens.Recursive<RArg1, ROut>, ROut>> RecursiveProxyStatement { get; init; }
        }
        [Obsolete("Will be removed. Use self referencing MetaFunctions.", true)]
        public sealed record Recursive<RArg1, RArg2, ROut>
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            public IToken<RArg1> A { get; init; }
            public IToken<RArg2> B { get; init; }
            public System.Func<ProxySyntax.OriginalHint<Tokens.Recursive<RArg1, RArg2, ROut>>, Proxy.IProxy<Tokens.Recursive<RArg1, RArg2, ROut>, ROut>> RecursiveProxyStatement { get; init; }
        }
        [Obsolete("Will be removed. Use self referencing MetaFunctions.", true)]
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
    public static class CoreT
    {
        public static Tokens.Board.Unit.AllUnits tAllUnits()
        { return new(); }
        public static Tokens.Board.Hex.AllHexes tAllHexes()
        { return new(); }

        public static SubEnvironment<R> tSubEnvironment<R>(RHint<R> _, TokenStructure.SubEnvironment<R> block) where R : class, ResObj
        { return new(block.Environment, block.SubToken); }

        public static Fixed<MetaFunction<ROut>> tMetaFunction<ROut>(RHint<ROut> _, Func<VariableIdentifier<MetaFunction<ROut>>, IToken<ROut>> tokenFunction) where ROut : class, ResObj
        {
            var vs = new VariableIdentifier<MetaFunction<ROut>>();
            return new(new() { SelfIdentifier = vs, Token = tokenFunction(vs) });
        }
        public static Fixed<MetaFunction<RArg1, ROut>> tMetaFunction<RArg1, ROut>(RHint<RArg1, ROut> _, Func<VariableIdentifier<MetaFunction<RArg1, ROut>>, VariableIdentifier<RArg1>, IToken<ROut>> tokenFunction)
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new VariableIdentifier<MetaFunction<RArg1, ROut>>();
            var v1 = new VariableIdentifier<RArg1>();
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, Token = tokenFunction(vs, v1) });
        }
        public static Fixed<MetaFunction<RArg1, RArg2, ROut>> tMetaFunction<RArg1, RArg2, ROut>(RHint<RArg1, RArg2, ROut> _, Func<VariableIdentifier<MetaFunction<RArg1, RArg2, ROut>>, VariableIdentifier<RArg1>, VariableIdentifier<RArg2>, IToken<ROut>> tokenFunction)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new VariableIdentifier<MetaFunction<RArg1, RArg2, ROut>>();
            var (v1, v2) = (new VariableIdentifier<RArg1>(), new VariableIdentifier<RArg2>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, Token = tokenFunction(vs, v1, v2) });
        }
        public static Fixed<MetaFunction<RArg1, RArg2, RArg3, ROut>> tMetaFunction<RArg1, RArg2, RArg3, ROut>(RHint<RArg1, RArg2, RArg3, ROut> _, Func<VariableIdentifier<MetaFunction<RArg1, RArg2, RArg3, ROut>>, VariableIdentifier<RArg1>, VariableIdentifier<RArg2>, VariableIdentifier<RArg3>, IToken<ROut>> tokenFunction)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new VariableIdentifier<MetaFunction<RArg1, RArg2, RArg3, ROut>>();
            var (v1, v2, v3) = (new VariableIdentifier<RArg1>(), new VariableIdentifier<RArg2>(), new VariableIdentifier<RArg3>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, IdentifierC = v3, Token = tokenFunction(vs, v1, v2, v3) });
        }

        [Obsolete("Will be removed. Use self referencing MetaFunctions.", true)]
        public static Recursive<RArg1, ROut> tRecursive<RArg1, ROut>(TokenStructure.Recursive<RArg1, ROut> block)
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        { return new(block.A) { RecursiveProxy = block.RecursiveProxyStatement(new()) }; }
        [Obsolete("Will be removed. Use self referencing MetaFunctions.", true)]
        public static Recursive<RArg1, RArg2, ROut> tRecursive<RArg1, RArg2, ROut>(TokenStructure.Recursive<RArg1, RArg2, ROut> block)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        { return new(block.A, block.B) { RecursiveProxy = block.RecursiveProxyStatement(new()) }; }
        [Obsolete("Will be removed. Use self referencing MetaFunctions.", true)]
        public static Recursive<RArg1, RArg2, RArg3, ROut> tRecursive<RArg1, RArg2, RArg3, ROut>(TokenStructure.Recursive<RArg1, RArg2, RArg3, ROut> block)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        { return new(block.A, block.B, block.C) { RecursiveProxy = block.RecursiveProxyStatement(new()) }; }

        public static Nolla<R> tNolla<R>(RHint<R> _) where R : class, ResObj
        { return new(); }

        public static Tokens.Multi.Union<R> tArrayOf<R>(RHint<R> _, List<IToken<R>> tokens) where R : class, ResObj
        { return new(tokens.Map(x => x.tYield())); }
        public static Tokens.Multi.Union<R> tUnion<R>(RHint<R> _, List<IToken<Multi<R>>> sets) where R : class, ResObj
        { return new(sets); }
        public static Tokens.Multi.Intersection<R> tIntersection<R>(RHint<R> _, List<IToken<Multi<R>>> sets) where R : class, ResObj
        { return new(sets); }
    }
    public static class _Extensions
    {
        public static Tokens.IO.Select.One<R> tIO_SelectOne<R>(this IToken<Multi<R>> source) where R : class, ResObj
        { return new(source); }
        public static Tokens.IO.Select.Multiple<R> tIO_SelectMany<R>(this IToken<Multi<R>> source, IToken<Number> count) where R : class, ResObj
        { return new(source, count); }

        public static Tokens.Execute<R> tExecute<R>(this IToken<MetaFunction<R>> source) where R : class, ResObj
        { return new(source); }
        public static Tokens.Execute<RArg1, ROut> tExecuteWith<RArg1, ROut>(this IToken<MetaFunction<RArg1, ROut>> source, TokenStructure.Args<RArg1> args)
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        { return new(source, new ToBoxedArgs<RArg1>(args.A)); }
        public static Tokens.Execute<RArg1, RArg2, ROut> tExecuteWith<RArg1, RArg2, ROut>(this IToken<MetaFunction<RArg1, RArg2, ROut>> source, TokenStructure.Args<RArg1, RArg2> args)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        { return new(source, new ToBoxedArgs<RArg1, RArg2>(args.A, args.B)); }
        public static Tokens.Execute<RArg1, RArg2, RArg3, ROut> tExecuteWith<RArg1, RArg2, RArg3, ROut>(this IToken<MetaFunction<RArg1, RArg2, RArg3, ROut>> source, TokenStructure.Args<RArg1, RArg2, RArg3> args)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        { return new(source, new ToBoxedArgs<RArg1, RArg2, RArg3>(args.A, args.B, args.C)); }

        public static Fixed<MetaFunction<ROut>> tMetaBoxed<ROut>(this IToken<ROut> token) where ROut : class, ResObj
        {
            return new(new() { SelfIdentifier = new(), Token = token });
        }
        public static Variable<R> tAsVariable<R>(this IToken<R> token, out VariableIdentifier<R> ident) where R : class, ResObj
        {
            ident = new();
            return new Variable<R>(ident, token);
        }
        public static Reference<R> tRef<R>(this VariableIdentifier<R> ident) where R : class, ResObj
        { return new(ident); }
        
        public static IfElse<R> tIfTrue<R>(this IToken<Bool> condition, RHint<R> _, TokenStructure.IfElse<R> block) where R : class, ResObj
        {
            return new(condition, block.Then, block.Else);
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
        public static Fixed<R> tConst<R>(this R value) where R : class, ResObj
        { return new(value); }
        public static Fixed<r.Multi<R>> tToConstMulti<R>(this IEnumerable<Tokens.Fixed<R>> values) where R : class, ResObj
        { return new(new() { Values = values.Map(x => x.Resolution) }); }

        public static Tokens.Component.Get<H, C> tGetComponent<H, C>(this IToken<H> holder, IToken<Resolution.IComponentIdentifier<C>> componentIdentifier)
            where H : class, Resolution.IHasComponents<H>
            where C : class, Resolution.IComponent<C, H>
        { return new(holder, componentIdentifier); }
        public static Tokens.Component.Insert<H> tInsertComponents<H>(this IToken<H> holder, IToken<r.Multi<Resolution.Unsafe.IComponentFor<H>>> components)
            where H : class, Resolution.IHasComponents<H>
        { return new(holder, components); }
        public static Tokens.Component.Remove<H> tRemoveComponents<H>(this IToken<H> holder, IToken<r.Multi<Resolution.Unsafe.IComponentIdentifier>> components)
            where H : class, Resolution.IHasComponents<H>
        { return new(holder, components); }

        public static Tokens.Declare tDeclare(this IToken<Resolution.Unsafe.IStateTracked> subject)
        { return new(subject); }
        public static Tokens.Undeclare tUndeclare(this IToken<Resolution.Unsafe.IStateTracked> subject)
        { return new(subject); }

        public static Tokens.Board.Coordinates.Of tGetPosition(this IToken<Resolution.Board.IPositioned> subject)
        { return new(subject); }

        public static Tokens.Board.Hex.At tHexAt(this IToken<r.Objects.Board.Coordinates> coordinates)
        { return new(coordinates); }
        public static Tokens.Board.Hex.InArea tHexesAt(this IToken<Resolution.IMulti<r.Objects.Board.Coordinates>> coordinates)
        { return new(coordinates); }

        public static Tokens.Board.Unit.Get.HP tGetHP(this IToken<r.Objects.Board.Unit> unit)
        { return new(unit); }
        public static Tokens.Board.Unit.Get.Owner tGetOwner(this IToken<r.Objects.Board.Unit> unit)
        { return new(unit); }

        public static Tokens.Board.Unit.Set.Position tSetPosition(this IToken<r.Objects.Board.Unit> unit, IToken<r.Objects.Board.Coordinates> setTo)
        { return new(unit, setTo); }
        public static Tokens.Board.Unit.Set.HP tSetHP(this IToken<r.Objects.Board.Unit> unit, IToken<r.Objects.Number> setTo)
        { return new(unit, setTo); }
        public static Tokens.Board.Unit.Set.Owner tSetOwner(this IToken<r.Objects.Board.Unit> unit, IToken<r.Objects.Board.Player> setTo)
        { return new(unit, setTo); }
    }
}