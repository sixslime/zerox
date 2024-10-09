using Perfection;

namespace FourZeroOne.Core.ProxySyntax
{
    using Proxies;
    using Proxy;
    using IToken = Token.Unsafe.IToken;
    using r = Resolutions;
    using ro = Resolutions.Objects;
    using ResObj = Resolution.IResolution;
    using TokenSyntax;
    public interface IOriginalHint<TOrig, out TOrig_> where TOrig : IToken where TOrig_ : IToken { }
    public delegate IProxy<TOrig, R> ProxyBuilder<TOrig, R>(OriginalHint<TOrig> hint) where TOrig : IToken where R : class, ResObj;
    public sealed record OriginalHint<TOrig> : IOriginalHint<TOrig, TOrig> where TOrig : IToken { }
    namespace ProxyStructure
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
            public IProxy<TOrig, r.Boxed.MetaFunction<R>> Then { get; init; }
            public IProxy<TOrig, r.Boxed.MetaFunction<R>> Else { get; init; }
        }

        public sealed record Recursive<TOrig, RArg1, ROut>
            where TOrig : IToken
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            public required IProxy<TOrig, RArg1> A { get; init; }
            public required ProxyBuilder<Tokens.Recursive<RArg1, ROut>, ROut> RecursiveProxyStatement { get; init; }
        }
        public sealed record Recursive<TOrig, RArg1, RArg2, ROut>
            where TOrig : IToken
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            public required IProxy<TOrig, RArg1> A { get; init; }
            public required IProxy<TOrig, RArg2> B { get; init; }
            public required ProxyBuilder<Tokens.Recursive<RArg1, RArg2, ROut>, ROut> RecursiveProxyStatement { get; init; }
        }
        public sealed record Recursive<TOrig, RArg1, RArg2, RArg3, ROut>
            where TOrig : IToken
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        {
            public required IProxy<TOrig, RArg1> A { get; init; }
            public required IProxy<TOrig, RArg2> B { get; init; }
            public required IProxy<TOrig, RArg3> C { get; init; }
            public required ProxyBuilder<Tokens.Recursive<RArg1, RArg2, RArg3, ROut>, ROut> RecursiveProxyStatement { get; init; }
        }

        public sealed record RecursiveCall<RArg1, RArg2, RArg3, ROut>
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        {
            public IProxy<Tokens.Recursive<RArg1, RArg2, RArg3, ROut>, RArg1> A { get; init; }
            public IProxy<Tokens.Recursive<RArg1, RArg2, RArg3, ROut>, RArg2> B { get; init; }
            public IProxy<Tokens.Recursive<RArg1, RArg2, RArg3, ROut>, RArg3> C { get; init; }
        }
        public sealed record RecursiveCall<RArg1, RArg2, ROut>
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            public IProxy<Tokens.Recursive<RArg1, RArg2, ROut>, RArg1> A { get; init; }
            public IProxy<Tokens.Recursive<RArg1, RArg2, ROut>, RArg2> B { get; init; }
        }
        public sealed record RecursiveCall<RArg1, ROut>
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            public IProxy<Tokens.Recursive<RArg1, ROut>, RArg1> A { get; init; }
        }

        public sealed record SubEnvironment<TOrig, R> where TOrig : IToken where R : class, ResObj
        {
            public Proxy.IProxy<TOrig, Resolution.IMulti<ResObj>> EnvironmentProxy { get; init; }
            public IProxy<TOrig, R> SubProxy { get; init; }
        }
    }
    public static class CoreP
    {
        public static IProxy<TOrig, ROut> Statement<TOrig, ROut>(ProxyBuilder<TOrig, ROut> statement) where TOrig : Token.IToken<ROut> where ROut : class, ResObj
        { return statement(new()); }
        public static Rule.Rule<TOrig, ROut> RuleFor<TOrig, ROut>(ProxyBuilder<TOrig, ROut> statement) where TOrig : Token.IToken<ROut> where ROut : class, ResObj
        { return new(statement(new())); }
        public static ToBoxedFunction<TOrig, ROut> pMetaFunction<TOrig, ROut>(System.Func<IProxy<TOrig, ROut>> metaFunction)
            where TOrig : IToken
            where ROut : class, ResObj
        {
            return new(metaFunction());
        }
    }
    public static class _Extensions
    {
        public static Direct<TOrig, R> pDirect<TOrig, R>(this Token.IToken<R> token, OriginalHint<TOrig> _) where TOrig : IToken where R : class, ResObj
        { return new(token); }
        public static OriginalArg1<TOrig, R> pOriginalA<TOrig, R>(this IOriginalHint<TOrig, Token.Unsafe.IHasArg1<R>> _) where TOrig : Token.Unsafe.IHasArg1<R> where R : class, ResObj
        { return new(); }
        public static OriginalArg2<TOrig, R> pOriginalB<TOrig, R>(this IOriginalHint<TOrig, Token.Unsafe.IHasArg2<R>> _) where TOrig : Token.Unsafe.IHasArg2<R> where R : class, ResObj
        { return new(); }
        public static OriginalArg3<TOrig, R> pOriginalC<TOrig, R>(this IOriginalHint<TOrig, Token.Unsafe.IHasArg3<R>> _) where TOrig : Token.Unsafe.IHasArg3<R> where R : class, ResObj
        { return new(); }

        public static SubEnvironment<TOrig, R> pSubEnvironment<TOrig, R>(this OriginalHint<TOrig> _, TokenSyntax.RHint<R> __, ProxyStructure.SubEnvironment<TOrig, R> block) where TOrig : IToken where R : class, ResObj
        {
            return new(block.EnvironmentProxy)
            {
                SubTokenProxy = block.SubProxy
            };
        }
        public static Combiner<Tokens.Multi.Union<R>, TOrig, Resolution.IMulti<R>, r.Multi<R>> pArrayOf<TOrig, R>(this OriginalHint<TOrig> _, TokenSyntax.RHint<R> __, List<IProxy<TOrig, R>> array) where TOrig : IToken where R : class, ResObj
        {
            return array.pToMulti();
        }

        public static ToBoxedFunction<TOrig, ROut> pMetaFunction<TOrig, ROut>(this OriginalHint<TOrig> _, System.Func<IProxy<TOrig, ROut>> metaFunction)
            where TOrig : IToken
            where ROut : class, ResObj
        {
            return new(metaFunction());
        }
        public static ToBoxedFunction<TOrig, RArg1, ROut> pMetaFunction<TOrig, RArg1, ROut>(this OriginalHint<TOrig> _, System.Func<Token.VariableIdentifier<RArg1>, IProxy<TOrig, ROut>> metaFunction)
            where TOrig : IToken
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            var v1 = new Token.VariableIdentifier<RArg1>();
            return new(metaFunction(v1), v1);
        }
        public static ToBoxedFunction<TOrig, RArg1, RArg2, ROut> pMetaFunction<TOrig, RArg1, RArg2, ROut>(this OriginalHint<TOrig> _, System.Func<Token.VariableIdentifier<RArg1>, Token.VariableIdentifier<RArg2>, IProxy<TOrig, ROut>> metaFunction)
            where TOrig : IToken
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            var (v1, v2) = (new Token.VariableIdentifier<RArg1>(), new Token.VariableIdentifier<RArg2>());
            return new(metaFunction(v1, v2), v1, v2);
        }
        public static ToBoxedFunction<TOrig, RArg1, RArg2, RArg3, ROut> pMetaFunction<TOrig, RArg1, RArg2, RArg3, ROut>(this OriginalHint<TOrig> _, System.Func<Token.VariableIdentifier<RArg1>, Token.VariableIdentifier<RArg2>, Token.VariableIdentifier<RArg3>, IProxy<TOrig, ROut>> metaFunction)
            where TOrig : IToken
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        {
            var (v1, v2, v3) = (new Token.VariableIdentifier<RArg1>(), new Token.VariableIdentifier<RArg2>(), new Token.VariableIdentifier<RArg3>());
            return new(metaFunction(v1, v2, v3), v1, v2, v3);
        }

        public static Function<Tokens.Execute<ROut>, TOrig, r.Boxed.MetaFunction<ROut>, ROut> pExecute<TOrig, ROut>(this IProxy<TOrig, r.Boxed.MetaFunction<ROut>> function)
            where TOrig : IToken
            where ROut : class, ResObj
        {
            return new(function);
        }
        public static Function<Tokens.Execute<RArg1, ROut>, TOrig, r.Boxed.MetaFunction<RArg1, ROut>, r.Boxed.MetaArgs<RArg1>, ROut> pExecuteWith<TOrig, RArg1, ROut>(this IProxy<TOrig, r.Boxed.MetaFunction<RArg1, ROut>> function, ProxyStructure.Args<TOrig, RArg1> args)
            where TOrig : IToken
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            var argProxy = new Function<Tokens.ToBoxedArgs<RArg1>, TOrig, RArg1, r.Boxed.MetaArgs<RArg1>>(args.A);
            return new(function, argProxy);
        }
        public static Function<Tokens.Execute<RArg1, RArg2, ROut>, TOrig, r.Boxed.MetaFunction<RArg1, RArg2, ROut>, r.Boxed.MetaArgs<RArg1, RArg2>, ROut> pExecuteWith<TOrig, RArg1, RArg2, ROut>(this IProxy<TOrig, r.Boxed.MetaFunction<RArg1, RArg2, ROut>> function, ProxyStructure.Args<TOrig, RArg1, RArg2> args)
            where TOrig : IToken
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            var argProxy = new Function<Tokens.ToBoxedArgs<RArg1, RArg2>, TOrig, RArg1, RArg2, r.Boxed.MetaArgs<RArg1, RArg2>>(args.A, args.B);
            return new(function, argProxy);
        }
        public static Function<Tokens.Execute<RArg1, RArg2, RArg3, ROut>, TOrig, r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>, r.Boxed.MetaArgs<RArg1, RArg2, RArg3>, ROut> pExecuteWith<TOrig, RArg1, RArg2, RArg3, ROut>(this IProxy<TOrig, r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>> function, ProxyStructure.Args<TOrig, RArg1, RArg2, RArg3> args)
            where TOrig : IToken
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        {
            var argProxy = new Function<Tokens.ToBoxedArgs<RArg1, RArg2, RArg3>, TOrig, RArg1, RArg2, RArg3, r.Boxed.MetaArgs<RArg1, RArg2, RArg3>>(args.A, args.B, args.C);
            return new(function, argProxy);
        }

        public static RecursiveStart<TOrig, RArg1, ROut> pRecursive<TOrig, RArg1, ROut>(this OriginalHint<TOrig> _, ProxyStructure.Recursive<TOrig, RArg1, ROut> block)
            where TOrig : IToken
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            return new(block.A) { RecursiveProxy = block.RecursiveProxyStatement(new()) };
        }
        public static RecursiveStart<TOrig, RArg1, RArg2, ROut> pRecursive<TOrig, RArg1, RArg2, ROut>(this OriginalHint<TOrig> _, ProxyStructure.Recursive<TOrig, RArg1, RArg2, ROut> block)
            where TOrig : IToken
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            return new(block.A, block.B) { RecursiveProxy = block.RecursiveProxyStatement(new()) };
        }
        public static RecursiveStart<TOrig, RArg1, RArg2, RArg3, ROut> pRecursive<TOrig, RArg1, RArg2, RArg3, ROut>(this OriginalHint<TOrig> _, ProxyStructure.Recursive<TOrig, RArg1, RArg2, RArg3, ROut> block)
            where TOrig : IToken
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        {
            return new(block.A, block.B, block.C) { RecursiveProxy = block.RecursiveProxyStatement(new()) };
        }

        public static RecursiveCall<RArg1, ROut> pRecurseWith<RArg1, ROut>(this OriginalHint<Tokens.Recursive<RArg1, ROut>> _, ProxyStructure.RecursiveCall<RArg1, ROut> block)
         where RArg1 : class, ResObj
         where ROut : class, ResObj
        { return new(block.A); }
        public static RecursiveCall<RArg1, RArg2, ROut> pRecurseWith<RArg1, RArg2, ROut>(this OriginalHint<Tokens.Recursive<RArg1, RArg2, ROut>> _, ProxyStructure.RecursiveCall<RArg1, RArg2, ROut> block)
             where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        { return new(block.A, block.B); }
        public static RecursiveCall<RArg1, RArg2, RArg3, ROut> pRecurseWith<RArg1, RArg2, RArg3, ROut>(this OriginalHint<Tokens.Recursive<RArg1, RArg2, RArg3, ROut>> _, ProxyStructure.RecursiveCall<RArg1, RArg2, RArg3, ROut> block)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        { return new(block.A, block.B, block.C); }

        public static Variable<TOrig, R> pAsVariable<TOrig, R>(this IProxy<TOrig, R> value, out Token.VariableIdentifier<R> identifier) where TOrig : IToken where R : class, ResObj
        {
            identifier = new();
            return new(identifier, value);
        }
        public static IfElse<TOrig, R> pIfTrue<TOrig, R>(this IProxy<TOrig, ro.Bool> condition, TokenSyntax.RHint<R> _, ProxyStructure.IfElse<TOrig, R> block) where TOrig : IToken where R : class, ResObj
        {
            return new(condition)
            {
                PassProxy = block.Then,
                FailProxy = block.Else
            };
        }

        public static Function<Tokens.Multi.Exclusion<R>, TOrig, Resolution.IMulti<R>, Resolution.IMulti<R>, r.Multi<R>> pWithout<TOrig, R>(this IProxy<TOrig, Resolution.IMulti<R>> source, IProxy<TOrig, Resolution.IMulti<R>> values) where TOrig : IToken where R : class, ResObj
        { return new(source, values); }
        public static Function<Tokens.Multi.Count, TOrig, Resolution.IMulti<ResObj>, ro.Number> pCount<TOrig>(this IProxy<TOrig, Resolution.IMulti<ResObj>> source) where TOrig : IToken
        { return new(source); }
        public static Function<Tokens.Multi.Yield<R>, TOrig, R, r.Multi<R>> pYield<TOrig, R>(this IProxy<TOrig, R> source) where TOrig : IToken where R : class, ResObj
        { return new(source); }
        public static Combiner<Tokens.Multi.Union<R>, TOrig, Resolution.IMulti<R>, r.Multi<R>> pToMulti<TOrig, R>(this IEnumerable<IProxy<TOrig, R>> values) where TOrig : IToken where R : class, ResObj
        { return new(values.Map(x => x.pYield())); }
        public static Combiner<Tokens.Multi.Union<R>, TOrig, Resolution.IMulti<R>, r.Multi<R>> pUnioned<TOrig, R>(this IEnumerable<IProxy<TOrig, Resolution.IMulti<R>>> values) where TOrig : IToken where R : class, ResObj
        { return new(values); }

        public static Function<Tokens.Number.Add, TOrig, ro.Number, ro.Number, ro.Number> pAdd<TOrig>(this IProxy<TOrig, ro.Number> a, IProxy<TOrig, ro.Number> b) where TOrig : IToken
        { return new(a, b); }
        public static Function<Tokens.Number.Subtract, TOrig, ro.Number, ro.Number, ro.Number> pSubtract<TOrig>(this IProxy<TOrig, ro.Number> a, IProxy<TOrig, ro.Number> b) where TOrig : IToken
        { return new(a, b); }
        public static Function<Tokens.Number.Multiply, TOrig, ro.Number, ro.Number, ro.Number> pMultiply<TOrig>(this IProxy<TOrig, ro.Number> a, IProxy<TOrig, ro.Number> b) where TOrig : IToken
        { return new(a, b); }
        public static Function<Tokens.Number.Negate, TOrig, ro.Number, ro.Number> pNegative<TOrig>(this IProxy<TOrig, ro.Number> a) where TOrig : IToken
        { return new(a); }
        public static Function<Tokens.Number.Compare.GreaterThan, TOrig, ro.Number, ro.Number, ro.Bool> pIsGreaterThan<TOrig>(this IProxy<TOrig, ro.Number> a, IProxy<TOrig, ro.Number> b) where TOrig : IToken
        { return new(a, b); }

        public static Function<Tokens.IO.Select.One<R>, TOrig, Resolution.IMulti<R>, R> pIO_SelectOne<TOrig, R>(this IProxy<TOrig, Resolution.IMulti<R>> source) where TOrig : IToken where R : class, ResObj
        { return new(source); }
        public static Function<Tokens.IO.Select.Multiple<R>, TOrig, Resolution.IMulti<R>, ro.Number, r.Multi<R>> pIO_SelectMany<TOrig, R>(this IProxy<TOrig, Resolution.IMulti<R>> source, IProxy<TOrig, ro.Number> count) where TOrig : IToken where R : class, ResObj
        { return new(source, count); }

        public static Function<Tokens.Declare, TOrig, Resolution.Unsafe.IStateTracked, r.Actions.Declare> pDeclare<TOrig>(this IProxy<TOrig, Resolution.Unsafe.IStateTracked> subject) where TOrig : IToken
        { return new(subject); }
        public static Function<Tokens.Undeclare, TOrig, Resolution.Unsafe.IStateTracked, r.Actions.Undeclare> pUndeclare<TOrig>(this IProxy<TOrig, Resolution.Unsafe.IStateTracked> subject) where TOrig : IToken
        { return new(subject); }

        public static Function<Tokens.Component.Get<H, C>, TOrig, H, Resolution.IComponentIdentifier<C>, C> pGetComponent<TOrig, H, C>(this IProxy<TOrig, H> holder, IProxy<TOrig, Resolution.IComponentIdentifier<C>> identifier)
            where TOrig : IToken
            where H : class, Resolution.IHasComponents<H>
            where C : class, Resolution.IComponent<C, H>
        { return new(holder, identifier); }
        public static Function<Tokens.Component.Insert<H>, TOrig, H, Resolution.IMulti<Resolution.Unsafe.IComponentFor<H>>, r.Actions.Component.Insert<H>> pInsertComponents<TOrig, H, C>(this IProxy<TOrig, H> holder, IProxy<TOrig, Resolution.IMulti<Resolution.Unsafe.IComponentFor<H>>> components)
            where TOrig : IToken
            where H : class, Resolution.IHasComponents<H>
        { return new(holder, components); }
        public static Function<Tokens.Component.Remove<H>, TOrig, H, Resolution.IMulti<Resolution.Unsafe.IComponentIdentifier>, r.Actions.Component.Remove<H>> pRemoveComponents<TOrig, H, C>(this IProxy<TOrig, H> holder, IProxy<TOrig, Resolution.IMulti<Resolution.Unsafe.IComponentIdentifier>> identifier)
            where TOrig : IToken
            where H : class, Resolution.IHasComponents<H>
        { return new(holder, identifier); }

        public static Function<Tokens.Board.Coordinates.Of, TOrig, Resolution.Board.IPositioned, ro.Board.Coordinates> pGetPosition<TOrig>(this IProxy<TOrig, Resolution.Board.IPositioned> subject) where TOrig : IToken
        { return new(subject); }

        public static Function<Tokens.Board.Unit.Get.HP, TOrig, ro.Board.Unit, ro.Number> pGetHP<TOrig>(this IProxy<TOrig, ro.Board.Unit> subject) where TOrig : IToken
        { return new(subject); }
        public static Function<Tokens.Board.Unit.Get.Owner, TOrig, ro.Board.Unit, ro.Board.Player> pGetOwner<TOrig>(this IProxy<TOrig, ro.Board.Unit> subject) where TOrig : IToken
        { return new(subject); }


        public static Function<Tokens.Board.Unit.Set.Position, TOrig, ro.Board.Unit, ro.Board.Coordinates, r.Actions.Board.Unit.PositionChange> pSetPosition<TOrig>(this IProxy<TOrig, ro.Board.Unit> subject, IProxy<TOrig, ro.Board.Coordinates> setTo) where TOrig : IToken
        { return new(subject, setTo); }
        public static Function<Tokens.Board.Unit.Set.HP, TOrig, ro.Board.Unit, ro.Number, r.Actions.Board.Unit.HPChange> pSetHP<TOrig>(this IProxy<TOrig, ro.Board.Unit> subject, IProxy<TOrig, ro.Number> setTo) where TOrig : IToken
        { return new(subject, setTo); }
        public static Function<Tokens.Board.Unit.Set.Owner, TOrig, ro.Board.Unit, ro.Board.Player, r.Actions.Board.Unit.OwnerChange> pSetOwner<TOrig>(this IProxy<TOrig, ro.Board.Unit> subject, IProxy<TOrig, ro.Board.Player> setTo) where TOrig : IToken
        { return new(subject, setTo); }
    }
}