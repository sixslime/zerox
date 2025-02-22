using System.Collections.Generic;
#nullable enable
namespace SixShaded.FourZeroOne.Core.Syntax
{
    using r = Resolutions;
    using t = Tokens;
    using ro = Resolutions;
    using Resolution.Defined;
    using Rule.Defined.Proxies;
    using Rule.Defined.Matchers;
    namespace Structure
    {
        namespace Token
        {
            public sealed record Args<RArg1>
                where RArg1 : class, Res
            {
                public required IToken<RArg1> A { get; init; }
            }
            public sealed record Args<RArg1, RArg2>
                where RArg1 : class, Res
                where RArg2 : class, Res
            {
                public required IToken<RArg1> A { get; init; }
                public required IToken<RArg2> B { get; init; }
            }
            public sealed record Args<RArg1, RArg2, RArg3>
                where RArg1 : class, Res
                where RArg2 : class, Res
                where RArg3 : class, Res
            {
                public required IToken<RArg1> A { get; init; }
                public required IToken<RArg2> B { get; init; }
                public required IToken<RArg3> C { get; init; }
            }
            public sealed record IfElse<R> where R : class, Res
            {
                public required IToken<R> Then { get; init; }
                public required IToken<R> Else { get; init; }
            }

            public sealed record SubEnvironment<R> where R : class, Res
            {
                public required IToken<Res> Environment { get; init; }
                public required IToken<R> Value { get; init; }
            }
        }
        namespace Rule
        {
            using FourZeroOne.Rule;
            using FourZeroOne.Rule.Proxies;
            public interface IMatcherBuilder { }
            public sealed record MatcherBuilder<RVal> : IMatcherBuilder
                where RVal : class, Res
            { }
            public sealed record MatcherBuilder<RArg1, ROut> : IMatcherBuilder
                where RArg1 : class, Res
                where ROut : class, Res
            { }
            public sealed record MatcherBuilder<RArg1, RArg2, ROut> : IMatcherBuilder
                where RArg1 : class, Res
                where RArg2 : class, Res
                where ROut : class, Res
            { }
            public sealed record MatcherBuilder<RArg1, RArg2, RArg3, ROut> : IMatcherBuilder
                where RArg1 : class, Res
                where RArg2 : class, Res
                where RArg3 : class, Res
                where ROut : class, Res
            { }

            public sealed record Block<RVal>
                where RVal : class, Res
            {
                public required Func<MatcherBuilder<RVal>, IRuleMatcher<IHasNoArgs<RVal>>> Matches { get; init; }
                public required Func<DynamicAddress<OriginalProxy<RVal>>, IToken<RVal>> Definition { get; init; }
            }
            public sealed record Block<RArg1, ROut>
                where RArg1 : class, Res
                where ROut : class, Res
            {
                public required Func<MatcherBuilder<RArg1, ROut>, IRuleMatcher<IHasArgs<RArg1, ROut>>> Matches { get; init; }
                public required Func<DynamicAddress<OriginalProxy<ROut>>, DynamicAddress<ArgProxy<RArg1>>, IToken<ROut>> Definition { get; init; }
            }
            public sealed record Block<RArg1, RArg2, ROut>
                where RArg1 : class, Res
                where RArg2 : class, Res
                where ROut : class, Res
            {
                public required Func<MatcherBuilder<RArg1, RArg2, ROut>, IRuleMatcher<IHasArgs<RArg1, RArg2, ROut>>> Matches { get; init; }
                public required Func<DynamicAddress<OriginalProxy<ROut>>, DynamicAddress<ArgProxy<RArg1>>, DynamicAddress<ArgProxy<RArg2>>, IToken<ROut>> Definition { get; init; }
            }
            public sealed record Block<RArg1, RArg2, RArg3, ROut>
                where RArg1 : class, Res
                where RArg2 : class, Res
                where RArg3 : class, Res
                where ROut : class, Res
            {
                public required Func<MatcherBuilder<RArg1, RArg2, RArg3, ROut>, IRuleMatcher<IHasArgs<RArg1, RArg2, RArg3, ROut>>> Matches { get; init; }
                public required Func<DynamicAddress<OriginalProxy<ROut>>, DynamicAddress<ArgProxy<RArg1>>, DynamicAddress<ArgProxy<RArg2>>, DynamicAddress<ArgProxy<RArg3>>, IToken<ROut>> Definition { get; init; }
            }
        }
    }
    public static class Core
    {
        public static t.SubEnvironment<R> tSubEnvironment<R>(Structure.Token.SubEnvironment<R> block) where R : class, Res
        { return new(block.Environment, block.Value); }
        public static t.Multi.Union<Res> t_Env(params IToken<Res>[] environment)
        { return new(environment.Map(x => x.tYield())); }
        public static t.Fixed<r.Boxed.MetaFunction<ROut>> tMetaFunction<ROut>(Func<IToken<ROut>> tokenFunction) where ROut : class, Res
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<ROut>>();
            return new(new() { SelfIdentifier = vs, Token = tokenFunction() });
        }
        public static t.Fixed<r.Boxed.MetaFunction<RArg1, ROut>> tMetaFunction<RArg1, ROut>(Func<DynamicAddress<RArg1>, IToken<ROut>> tokenFunction)
            where RArg1 : class, Res
            where ROut : class, Res
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, ROut>>();
            var v1 = new DynamicAddress<RArg1>();
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, Token = tokenFunction(v1) });
        }
        public static t.Fixed<r.Boxed.MetaFunction<RArg1, RArg2, ROut>> tMetaFunction<RArg1, RArg2, ROut>(Func<DynamicAddress<RArg1>, DynamicAddress<RArg2>, IToken<ROut>> tokenFunction)
            where RArg1 : class, Res
            where RArg2 : class, Res
            where ROut : class, Res
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, ROut>>();
            var (v1, v2) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, Token = tokenFunction(v1, v2) });
        }
        public static t.Fixed<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>> tMetaFunction<RArg1, RArg2, RArg3, ROut>(Func<DynamicAddress<RArg1>, DynamicAddress<RArg2>, DynamicAddress<RArg3>, IToken<ROut>> tokenFunction)
            where RArg1 : class, Res
            where RArg2 : class, Res
            where RArg3 : class, Res
            where ROut : class, Res
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>>();
            var (v1, v2, v3) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>(), new DynamicAddress<RArg3>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, IdentifierC = v3, Token = tokenFunction(v1, v2, v3) });
        }
        public static t.Fixed<r.Boxed.MetaFunction<ROut>> tMetaRecursiveFunction<ROut>(Func<DynamicAddress<r.Boxed.MetaFunction<ROut>>, IToken<ROut>> tokenFunction) where ROut : class, Res
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<ROut>>();
            return new(new() { SelfIdentifier = vs, Token = tokenFunction(vs) });
        }
        public static t.Fixed<r.Boxed.MetaFunction<RArg1, ROut>> tMetaRecursiveFunction<RArg1, ROut>(Func<DynamicAddress<r.Boxed.MetaFunction<RArg1, ROut>>, DynamicAddress<RArg1>, IToken<ROut>> tokenFunction)
            where RArg1 : class, Res
            where ROut : class, Res
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, ROut>>();
            var v1 = new DynamicAddress<RArg1>();
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, Token = tokenFunction(vs, v1) });
        }
        public static t.Fixed<r.Boxed.MetaFunction<RArg1, RArg2, ROut>> tMetaRecursiveFunction<RArg1, RArg2, ROut>(Func<DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, ROut>>, DynamicAddress<RArg1>, DynamicAddress<RArg2>, IToken<ROut>> tokenFunction)
            where RArg1 : class, Res
            where RArg2 : class, Res
            where ROut : class, Res
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, ROut>>();
            var (v1, v2) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, Token = tokenFunction(vs, v1, v2) });
        }
        public static t.Fixed<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>> tMetaRecursiveFunction<RArg1, RArg2, RArg3, ROut>(Func<DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>>, DynamicAddress<RArg1>, DynamicAddress<RArg2>, DynamicAddress<RArg3>, IToken<ROut>> tokenFunction)
            where RArg1 : class, Res
            where RArg2 : class, Res
            where RArg3 : class, Res
            where ROut : class, Res
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>>();
            var (v1, v2, v3) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>(), new DynamicAddress<RArg3>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, IdentifierC = v3, Token = tokenFunction(vs, v1, v2, v3) });
        }

        public static t.Multi.Union<R> tMultiOf<R>(List<IToken<R>> t) where R : class, Res
        { return new(t.Map(x => x.tYield())); }
        public static t.Multi.Union<R> tUnionOf<R>(List<IToken<IMulti<R>>> sets) where R : class, Res
        { return new(sets); }
        public static t.Multi.Intersection<R> tIntersectionOf<R>(List<IToken<IMulti<R>>> sets) where R : class, Res
        { return new(sets); }
        public static t.Nolla<R> tNollaFor<R>() where R : class, Res
        { return new(); }
        public static Macro<ICompositionOf<C>> tCompose<C>() where C : ICompositionType, new()
        {
            return Compose<C>.Construct();
        }

        public static t.AddRule tAddRule<RVal>(Structure.Rule.Block<RVal> block)
            where RVal : class, Res
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<OriginalProxy<RVal>, RVal>>();
            var vo = new DynamicAddress<OriginalProxy<RVal>>();

            return new(new Rule.Define.RuleForValue<RVal>()
            {
                Definition = new() { SelfIdentifier = vs, IdentifierA = vo, Token = block.Definition(vo) },
                Matcher = block.Matches(new())
            });
        }
        public static t.AddRule tAddRule<RArg1, ROut>(Structure.Rule.Block<RArg1, ROut> block)
            where RArg1 : class, Res
            where ROut : class, Res
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ROut>>();
            var (vo, v1) = (new DynamicAddress<OriginalProxy<ROut>>(), new DynamicAddress<ArgProxy<RArg1>>());

            return new(new Rule.Define.RuleForFunction<RArg1, ROut>()
            {
                Definition = new() { SelfIdentifier = vs, IdentifierA = vo, IdentifierB = v1, Token = block.Definition(vo, v1) },
                Matcher = block.Matches(new())
            });
        }
        public static t.AddRule tAddRule<RArg1, RArg2, ROut>(Structure.Rule.Block<RArg1, RArg2, ROut> block)
            where RArg1 : class, Res
            where RArg2 : class, Res
            where ROut : class, Res
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ROut>>();
            var (vo, v1, v2) = (new DynamicAddress<OriginalProxy<ROut>>(), new DynamicAddress<ArgProxy<RArg1>>(), new DynamicAddress<ArgProxy<RArg2>>());

            return new(new Rule.Define.RuleForFunction<RArg1, RArg2, ROut>()
            {
                Definition = new() { SelfIdentifier = vs, IdentifierA = vo, IdentifierB = v1, IdentifierC = v2, Token = block.Definition(vo, v1, v2) },
                Matcher = block.Matches(new())
            });
        }
        public static t.AddRule tAddRule<RArg1, RArg2, RArg3, ROut>(Structure.Rule.Block<RArg1, RArg2, RArg3, ROut> block)
            where RArg1 : class, Res
            where RArg2 : class, Res
            where RArg3 : class, Res
            where ROut : class, Res
        {
            var vs = new DynamicAddress<r.Boxed.OverflowingMetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ArgProxy<RArg3>, ROut>>();
            var (vo, v1, v2, v3) = (new DynamicAddress<OriginalProxy<ROut>>(), new DynamicAddress<ArgProxy<RArg1>>(), new DynamicAddress<ArgProxy<RArg2>>(), new DynamicAddress<ArgProxy<RArg3>>());

            return new(new Rule.Define.RuleForFunction<RArg1, RArg2, RArg3, ROut>()
            {
                Definition = new() { SelfIdentifier = vs, IdentifierA = vo, IdentifierB = v1, IdentifierC = v2, IdentifierD = v3, Token = block.Definition(vo, v1, v2, v3) },
                Matcher = block.Matches(new())
            });
        }
    }

    public static class TokenSyntax
    {
        public static t.IO.Select.One<R> tIOSelectOne<R>(this IToken<IMulti<R>> source) where R : class, Res
        { return new(source); }
        public static t.IO.Select.Multiple<R> tIOSelectMany<R>(this IToken<IMulti<R>> source, IToken<Number> count) where R : class, Res
        { return new(source, count); }

        public static t.Execute<R> tExecute<R>(this IToken<r.Boxed.MetaFunction<R>> source) where R : class, Res
        { return new(source); }
        public static t.Execute<RArg1, ROut> tExecuteWith<RArg1, ROut>(this IToken<r.Boxed.MetaFunction<RArg1, ROut>> source, Structure.Token.Args<RArg1> args)
            where RArg1 : class, Res
            where ROut : class, Res
        { return new(source, new t.ToBoxedArgs<RArg1>(args.A)); }
        public static t.Execute<RArg1, RArg2, ROut> tExecuteWith<RArg1, RArg2, ROut>(this IToken<r.Boxed.MetaFunction<RArg1, RArg2, ROut>> source, Structure.Token.Args<RArg1, RArg2> args)
            where RArg1 : class, Res
            where RArg2 : class, Res
            where ROut : class, Res
        { return new(source, new t.ToBoxedArgs<RArg1, RArg2>(args.A, args.B)); }
        public static t.Execute<RArg1, RArg2, RArg3, ROut> tExecuteWith<RArg1, RArg2, RArg3, ROut>(this IToken<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>> source, Structure.Token.Args<RArg1, RArg2, RArg3> args)
            where RArg1 : class, Res
            where RArg2 : class, Res
            where RArg3 : class, Res
            where ROut : class, Res
        { return new(source, new t.ToBoxedArgs<RArg1, RArg2, RArg3>(args.A, args.B, args.C)); }

        public static t.Fixed<r.Boxed.MetaFunction<ROut>> tMetaBoxed<ROut>(this IToken<ROut> token) where ROut : class, Res
        {
            return new(new() { SelfIdentifier = new(), Token = token });
        }

        public static t.Exists tExists(this IToken<Res> token)
        { return new(token); }

        public static t.DynamicAssign<R> tAsVariable<R>(this IToken<R> token, out DynamicAddress<R> ident) where R : class, Res
        {
            ident = new();
            return new(ident, token);
        }

        public static t.IfElse<R> tIfTrueDirect<R>(this IToken<Bool> condition, Structure.Token.IfElse<r.Boxed.MetaFunction<R>> block) where R : class, Res
        { return new(condition, block.Then, block.Else); }
        public static t.Execute<R> t_IfTrue<R>(this IToken<Bool> condition, Structure.Token.IfElse<R> block) where R : class, Res
        {
            return condition.tIfTrueDirect<R>(new()
            {
                Then = block.Then.tMetaBoxed(),
                Else = block.Else.tMetaBoxed()
            }).tExecute();
        }
        public static t.Multi.Exclusion<R> tWithout<R>(this IToken<IMulti<R>> source, IToken<IMulti<R>> exclude) where R : class, Res
        { return new(source, exclude); }
        public static t.Multi.Count tCount(this IToken<IMulti<Res>> source)
        { return new(source); }
        public static t.Multi.Yield<R> tYield<R>(this IToken<R> token) where R : class, Res
        { return new(token); }
        public static t.Multi.Union<R> tToMulti<R>(this IEnumerable<IToken<R>> t) where R : class, Res
        { return new(t.Map(x => x.tYield())); }
        public static t.Multi.Union<R> tUnion<R>(this IToken<IMulti<R>> left, IToken<IMulti<R>> right) where R : class, Res
        { return new([left, right]); }
        public static t.Multi.Union<R> tFlatten<R>(this IEnumerable<IToken<IMulti<R>>> t) where R : class, Res
        { return new(t); }
        public static t.Multi.Contains<R> tContains<R>(this IToken<IMulti<R>> from, IToken<R> element) where R : class, Res
        { return new(from, element); }
        public static t.Multi.GetIndex<R> tAtIndex<R>(this IToken<IMulti<R>> token, IToken<Number> index) where R : class, Res
        { return new(token, index); }
        public static Macro<IMulti<RIn>, r.Boxed.MetaFunction<RIn, ROut>, r.Multi<ROut>> tMap<RIn, ROut>(this IToken<IMulti<RIn>> source, Func<DynamicAddress<RIn>, IToken<ROut>> mapFunction)
            where RIn : class, Res
            where ROut : class, Res
        { return Map<RIn, ROut>.Construct(source, Core.tMetaFunction(mapFunction)); }
        public static Macro<R, Number, r.Multi<R>> tDuplicate<R>(this IToken<R> value, IToken<Number> count)
            where R : class, Res
        { return Duplicate<R>.Construct(value, count); }

        public static t.Number.Add tAdd(this IToken<Number> a, IToken<Number> b)
        { return new(a, b); }
        public static t.Number.Subtract tSubtract(this IToken<Number> a, IToken<Number> b)
        { return new(a, b); }
        public static t.Number.Multiply tMultiply(this IToken<Number> a, IToken<Number> b)
        { return new(a, b); }
        public static t.Number.Compare.GreaterThan tIsGreaterThan(this IToken<Number> a, IToken<Number> b)
        { return new(a, b); }

        public static t.Component.Get<H, C> tGetComponent<H, C>(this IToken<ICompositionOf<H>> holder, IComponentIdentifier<H, C> componentIdentifier)
            where H : ICompositionType
            where C : class, Res
        { return new(holder) { ComponentIdentifier = componentIdentifier }; }
        public static t.Component.With<H, C> tWithComponent<H, C>(this IToken<ICompositionOf<H>> holder, IComponentIdentifier<H, C> componentIdentifier, IToken<C> component)
            where H : ICompositionType
            where C : class, Res
        { return new(holder, component) { ComponentIdentifier = componentIdentifier }; }
        public static Macro<ICompositionOf<C>, r.Boxed.MetaFunction<R, R>, ICompositionOf<C>> tUpdateComponent<C, R>(this IToken<ICompositionOf<C>> holder, IComponentIdentifier<C, R> componentIdentifier, IToken<r.Boxed.MetaFunction<R, R>> changeFunc)
            where C : ICompositionType
            where R : class, Res
        { return UpdateComponent<C, R>.Construct(holder, changeFunc, componentIdentifier) ; }
        public static Macro<ICompositionOf<C>, r.Boxed.MetaFunction<R, R>, ICompositionOf<C>> tUpdateComponent<C, R>(this IToken<ICompositionOf<C>> holder, IComponentIdentifier<C, R> componentIdentifier, Func<DynamicAddress<R>, IToken<R>> changeFunc)
            where C : ICompositionType
            where R : class, Res
        { return UpdateComponent<C, R>.Construct(holder, Core.tMetaFunction(changeFunc), componentIdentifier); }
        public static t.Component.Without<H> tWithoutComponent<H>(this IToken<ICompositionOf<H>> holder, Resolution.Unsafe.IComponentIdentifier<H> componentIdentifier)
            where H : ICompositionType
        { return new(holder) { ComponentIdentifier = componentIdentifier }; }
        public static Macro<ICompositionOf<D>, R> tDecompose<D, R>(this IToken<ICompositionOf<D>> composition) where D : IDecomposableType<D, R>, new() where R : class, Res
        { return Decompose<D, R>.Construct(composition); }

        public static t.Component.With<r.MergeSpec<H>, C> t_WithMerged<H, C>(this IToken<ICompositionOf<r.MergeSpec<H>>> mergeObject, IComponentIdentifier<H, C> mergingIdentifier, IToken<C> component)
            where H : ICompositionType
            where C : class, Res
        { return mergeObject.tWithComponent(r.MergeSpec<H>.MERGE(mergingIdentifier), component); }
        public static t.Component.DoMerge<H> tMerge<H>(this IToken<ICompositionOf<H>> subject, IToken<ICompositionOf<r.MergeSpec<H>>> mergeObject)
            where H : ICompositionType
        { return new(subject, mergeObject); }
        public static t.Data.Insert<R> tDataWrite<R>(this IToken<IMemoryObject<R>> address, IToken<R> data)
            where R : class, Res
        { return new(address, data); }
        public static t.Data.Get<R> tDataGet<R>(this IToken<IMemoryObject<R>> address)
            where R : class, Res
        { return new(address); }
        public static t.Data.Remove tDataRedact(this IToken<IMemoryObject<Res>> address)
        { return new(address); }
        public static Macro<IMemoryObject<R>, r.Boxed.MetaFunction<R, R>, r.Instructions.Assign<R>> tDataUpdate<R>(this IToken<IMemoryObject<R>> address, IToken<r.Boxed.MetaFunction<R, R>> updateFunction)
            where R : class, Res
        { return UpdateMemoryObject<R>.Construct(address, updateFunction); }
        public static Macro<IMemoryObject<R>, r.Boxed.MetaFunction<R, R>, r.Instructions.Assign<R>> tDataUpdate<R>(this IToken<IMemoryObject<R>> address, Func<DynamicAddress<R>, IToken<R>> updateFunction)
            where R : class, Res
        { return UpdateMemoryObject<R>.Construct(address, Core.tMetaFunction(updateFunction)); }

        public static Macro<R, r.Boxed.MetaFunction<R>, R> tCatchNolla<R>(this IToken<R> value, IToken<r.Boxed.MetaFunction<R>> fallback)
            where R : class, Res
        { return CatchNolla<R>.Construct(value, fallback); }
        public static Macro<R, r.Boxed.MetaFunction<R>, R> tCatchNolla<R>(this IToken<R> value, Func<IToken<R>> fallback)
            where R : class, Res
        { return CatchNolla<R>.Construct(value, fallback().tMetaBoxed()); }
        public static t.DynamicReference<R> tRef<R>(this IMemoryAddress<R> ident) where R : class, Res
        { return new(ident); }
        public static t.Fixed<Bool> tFixed(this bool value)
        { return new(value); }
        public static t.Fixed<Number> tFixed(this int value)
        { return new(value); }
        public static t.Fixed<NumRange> tFixed(this Range value)
        { return new(value); }
        public static t.Fixed<R> tFixed<R>(this R value) where R : class, Res
        { return new(value); }
        public static t.Fixed<r.Multi<R>> tFixed<R>(this IEnumerable<R> values) where R : class, Res
        { return new(new() { Values = values.ToPSequence() }); }

        public static t.Fixed<r.Multi<R>> t_ToConstMulti<R>(this IEnumerable<t.Fixed<R>> values) where R : class, Res
        { return new(new() { Values = values.Map(x => x.Resolution).ToPSequence() }); }

        public static RealizeProxy<R> tRealize<R>(this IToken<IProxy<R>> proxy)
            where R : class, Res
        { return new(proxy); }
        
    }
    public static class RuleMatcherSyntax
    {
        
        public static TypeMatcher<TMatch> mIsType<TMatch>(this Structure.Rule.IMatcherBuilder _)
            where TMatch : IToken<Res>
        { return new(); }

        public static MacroMatcher<RVal> mIsMacro<RVal>(this Structure.Rule.MatcherBuilder<RVal> _, string package, string identifier)
            where RVal : class, Res
        { return new() { Label = new() { Package = package, Identifier = identifier } }; }
        public static MacroMatcher<RArg1, ROut> mIsMacro<RArg1, ROut>(this Structure.Rule.MatcherBuilder<RArg1, ROut> _, string package, string identifier)
            where RArg1 : class, Res
            where ROut : class, Res
        { return new() { Label = new() { Package = package, Identifier = identifier } }; }
        public static MacroMatcher<RArg1, RArg2, ROut> mIsMacro<RArg1, RArg2, ROut>(this Structure.Rule.MatcherBuilder<RArg1, RArg2, ROut> _, string package, string identifier)
            where RArg1 : class, Res
            where RArg2 : class, Res
            where ROut : class, Res
        { return new() { Label = new() { Package = package, Identifier = identifier } }; }
        public static MacroMatcher<RArg1, RArg2, RArg3, ROut> mIsMacro<RArg1, RArg2, RArg3, ROut>(this Structure.Rule.MatcherBuilder<RArg1, RArg2, RArg3, ROut> _, string package, string identifier)
            where RArg1 : class, Res
            where RArg2 : class, Res
            where RArg3 : class, Res
            where ROut : class, Res
        { return new() { Label = new() { Package = package, Identifier = identifier } }; }

        public static AnyMatcher<IHasNoArgs<RVal>> mAny<RVal>(this Structure.Rule.MatcherBuilder<RVal> _, List<IRuleMatcher<IHasNoArgs<RVal>>> entries)
            where RVal : class, Res
        { return new() { Entries = entries.ToPSet() }; }

        public static AnyMatcher<IHasArgs<RArg1, ROut>> mAny<RArg1, ROut>(this Structure.Rule.MatcherBuilder<RArg1, ROut> _, List<IRuleMatcher<IHasArgs<RArg1, ROut>>> entries)
            where RArg1 : class, Res
            where ROut : class, Res
        { return new() { Entries = entries.ToPSet() }; }
        public static AnyMatcher<IHasArgs<RArg1, RArg2, ROut>> mAny<RArg1, RArg2, ROut>(this Structure.Rule.MatcherBuilder<RArg1, RArg2, ROut> _, List<IRuleMatcher<IHasArgs<RArg1, RArg2, ROut>>> entries)
            where RArg1 : class, Res
            where RArg2 : class, Res
            where ROut : class, Res
        { return new() { Entries = entries.ToPSet() }; }
        public static AnyMatcher<IHasArgs<RArg1, RArg2, RArg3, ROut>> mAny<RArg1, RArg2, RArg3, ROut>(this Structure.Rule.MatcherBuilder<RArg1, RArg2, RArg3, ROut> _, List<IRuleMatcher<IHasArgs<RArg1, RArg2, RArg3, ROut>>> entries)
            where RArg1 : class, Res
            where RArg2 : class, Res
            where RArg3 : class, Res
            where ROut : class, Res
        { return new() { Entries = entries.ToPSet() }; }

        public static AllMatcher<IHasNoArgs<RVal>> mAll<RVal>(this Structure.Rule.MatcherBuilder<RVal> _, List<IRuleMatcher<IHasNoArgs<RVal>>> entries)
            where RVal : class, Res
        { return new() { Entries = entries.ToPSet() }; }

        public static AllMatcher<IHasArgs<RArg1, ROut>> mAll<RArg1, ROut>(this Structure.Rule.MatcherBuilder<RArg1, ROut> _, List<IRuleMatcher<IHasArgs<RArg1, ROut>>> entries)
            where RArg1 : class, Res
            where ROut : class, Res
        { return new() { Entries = entries.ToPSet() }; }
        public static AllMatcher<IHasArgs<RArg1, RArg2, ROut>> mAll<RArg1, RArg2, ROut>(this Structure.Rule.MatcherBuilder<RArg1, RArg2, ROut> _, List<IRuleMatcher<IHasArgs<RArg1, RArg2, ROut>>> entries)
            where RArg1 : class, Res
            where RArg2 : class, Res
            where ROut : class, Res
        { return new() { Entries = entries.ToPSet() }; }
        public static AllMatcher<IHasArgs<RArg1, RArg2, RArg3, ROut>> mAll<RArg1, RArg2, RArg3, ROut>(this Structure.Rule.MatcherBuilder<RArg1, RArg2, RArg3, ROut> _, List<IRuleMatcher<IHasArgs<RArg1, RArg2, RArg3, ROut>>> entries)
            where RArg1 : class, Res
            where RArg2 : class, Res
            where RArg3 : class, Res
            where ROut : class, Res
        { return new() { Entries = entries.ToPSet() }; }
    }
}