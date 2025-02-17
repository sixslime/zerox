
using Perfection;
using System.Collections.Generic;
#nullable enable
namespace FourZeroOne.Core.Syntax
{
    using r = Resolutions;
    using t = Tokens;
    using ro = Resolutions.Objects;
    using Token;
    using Resolution;
    using Macro;
    using ResObj = Resolution.IResolution;
    
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
        

    }

    public static class Core
    {
        public static t.SubEnvironment<R> tSubEnvironment<R>(Structure.Token.SubEnvironment<R> block) where R : class, ResObj
        { return new(block.Environment, block.Value); }
        public static t.Multi.Union<ResObj> t_Env(params IToken<ResObj>[] environment)
        { return new(environment.Map(x => x.tYield())); }
        public static t.Fixed<r.Boxed.MetaFunction<ROut>> tMetaFunction<ROut>(Func<IToken<ROut>> tokenFunction) where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<ROut>>();
            return new(new() { SelfIdentifier = vs, Token = tokenFunction() });
        }
        public static t.Fixed<r.Boxed.MetaFunction<RArg1, ROut>> tMetaFunction<RArg1, ROut>(Func<DynamicAddress<RArg1>, IToken<ROut>> tokenFunction)
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, ROut>>();
            var v1 = new DynamicAddress<RArg1>();
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, Token = tokenFunction(v1) });
        }
        public static t.Fixed<r.Boxed.MetaFunction<RArg1, RArg2, ROut>> tMetaFunction<RArg1, RArg2, ROut>(Func<DynamicAddress<RArg1>, DynamicAddress<RArg2>, IToken<ROut>> tokenFunction)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, ROut>>();
            var (v1, v2) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, Token = tokenFunction(v1, v2) });
        }
        public static t.Fixed<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>> tMetaFunction<RArg1, RArg2, RArg3, ROut>(Func<DynamicAddress<RArg1>, DynamicAddress<RArg2>, DynamicAddress<RArg3>, IToken<ROut>> tokenFunction)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>>();
            var (v1, v2, v3) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>(), new DynamicAddress<RArg3>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, IdentifierC = v3, Token = tokenFunction(v1, v2, v3) });
        }
        public static t.Fixed<r.Boxed.MetaFunction<ROut>> tMetaRecursiveFunction<ROut>(Func<DynamicAddress<r.Boxed.MetaFunction<ROut>>, IToken<ROut>> tokenFunction) where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<ROut>>();
            return new(new() { SelfIdentifier = vs, Token = tokenFunction(vs) });
        }
        public static t.Fixed<r.Boxed.MetaFunction<RArg1, ROut>> tMetaRecursiveFunction<RArg1, ROut>(Func<DynamicAddress<r.Boxed.MetaFunction<RArg1, ROut>>, DynamicAddress<RArg1>, IToken<ROut>> tokenFunction)
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, ROut>>();
            var v1 = new DynamicAddress<RArg1>();
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, Token = tokenFunction(vs, v1) });
        }
        public static t.Fixed<r.Boxed.MetaFunction<RArg1, RArg2, ROut>> tMetaRecursiveFunction<RArg1, RArg2, ROut>(Func<DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, ROut>>, DynamicAddress<RArg1>, DynamicAddress<RArg2>, IToken<ROut>> tokenFunction)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, ROut>>();
            var (v1, v2) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, Token = tokenFunction(vs, v1, v2) });
        }
        public static t.Fixed<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>> tMetaRecursiveFunction<RArg1, RArg2, RArg3, ROut>(Func<DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>>, DynamicAddress<RArg1>, DynamicAddress<RArg2>, DynamicAddress<RArg3>, IToken<ROut>> tokenFunction)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        {
            var vs = new DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>>();
            var (v1, v2, v3) = (new DynamicAddress<RArg1>(), new DynamicAddress<RArg2>(), new DynamicAddress<RArg3>());
            return new(new() { SelfIdentifier = vs, IdentifierA = v1, IdentifierB = v2, IdentifierC = v3, Token = tokenFunction(vs, v1, v2, v3) });
        }

        public static t.Multi.Union<R> tMultiOf<R>(List<IToken<R>> t) where R : class, ResObj
        { return new(t.Map(x => x.tYield())); }
        public static t.Multi.Union<R> tUnion<R>(List<IToken<IMulti<R>>> sets) where R : class, ResObj
        { return new(sets); }
        public static t.Multi.Intersection<R> tIntersection<R>(List<IToken<IMulti<R>>> sets) where R : class, ResObj
        { return new(sets); }
        public static t.Nolla<R> tNolla<R>() where R : class, ResObj
        { return new(); }
        public static Macro<ICompositionOf<C>> tCompose<C>() where C : ICompositionType, new()
        {
            return Macros.Compose<C>.Construct();
        }

    }
    public static class _Extensions
    {
        public static t.IO.Select.One<R> tIOSelectOne<R>(this IToken<IMulti<R>> source) where R : class, ResObj
        { return new(source); }
        public static t.IO.Select.Multiple<R> tIOSelectMany<R>(this IToken<IMulti<R>> source, IToken<ro.Number> count) where R : class, ResObj
        { return new(source, count); }

        public static t.Execute<R> tExecute<R>(this IToken<r.Boxed.MetaFunction<R>> source) where R : class, ResObj
        { return new(source); }
        public static t.Execute<RArg1, ROut> tExecuteWith<RArg1, ROut>(this IToken<r.Boxed.MetaFunction<RArg1, ROut>> source, Structure.Token.Args<RArg1> args)
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        { return new(source, new t.ToBoxedArgs<RArg1>(args.A)); }
        public static t.Execute<RArg1, RArg2, ROut> tExecuteWith<RArg1, RArg2, ROut>(this IToken<r.Boxed.MetaFunction<RArg1, RArg2, ROut>> source, Structure.Token.Args<RArg1, RArg2> args)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        { return new(source, new t.ToBoxedArgs<RArg1, RArg2>(args.A, args.B)); }
        public static t.Execute<RArg1, RArg2, RArg3, ROut> tExecuteWith<RArg1, RArg2, RArg3, ROut>(this IToken<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>> source, Structure.Token.Args<RArg1, RArg2, RArg3> args)
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        { return new(source, new t.ToBoxedArgs<RArg1, RArg2, RArg3>(args.A, args.B, args.C)); }

        public static t.Fixed<r.Boxed.MetaFunction<ROut>> tMetaBoxed<ROut>(this IToken<ROut> token) where ROut : class, ResObj
        {
            return new(new() { SelfIdentifier = new(), Token = token });
        }

        public static t.Exists tExists(this IToken<ResObj> token)
        { return new(token); }

        public static t.DynamicAssign<R> tAsVariable<R>(this IToken<R> token, out DynamicAddress<R> ident) where R : class, ResObj
        {
            ident = new();
            return new(ident, token);
        }

        public static t.IfElse<R> tIfTrueDirect<R>(this IToken<ro.Bool> condition, Structure.Token.IfElse<r.Boxed.MetaFunction<R>> block) where R : class, ResObj
        { return new(condition, block.Then, block.Else); }
        public static t.Execute<R> t_IfTrue<R>(this IToken<ro.Bool> condition, Structure.Token.IfElse<R> block) where R : class, ResObj
        {
            return condition.tIfTrueDirect<R>(new()
            {
                Then = block.Then.tMetaBoxed(),
                Else = block.Else.tMetaBoxed()
            }).tExecute();
        }
        public static t.Multi.Exclusion<R> tWithout<R>(this IToken<IMulti<R>> source, IToken<IMulti<R>> exclude) where R : class, ResObj
        { return new(source, exclude); }
        public static t.Multi.Count tCount(this IToken<IMulti<ResObj>> source)
        { return new(source); }
        public static t.Multi.Yield<R> tYield<R>(this IToken<R> token) where R : class, ResObj
        { return new(token); }
        public static t.Multi.Union<R> tToMulti<R>(this IEnumerable<IToken<R>> t) where R : class, ResObj
        { return new(t.Map(x => x.tYield())); }
        public static t.Multi.Union<R> tUnion<R>(this IToken<IMulti<R>> left, IToken<IMulti<R>> right) where R : class, ResObj
        { return new([left, right]); }
        public static t.Multi.Union<R> tFlatten<R>(this IEnumerable<IToken<IMulti<R>>> t) where R : class, ResObj
        { return new(t); }
        public static t.Multi.Contains<R> tContains<R>(this IToken<IMulti<R>> from, IToken<R> element) where R : class, ResObj
        { return new(from, element); }
        public static t.Multi.GetIndex<R> tAtIndex<R>(this IToken<IMulti<R>> token, IToken<ro.Number> index) where R : class, ResObj
        { return new(token, index); }
        public static Macro<IMulti<RIn>, r.Boxed.MetaFunction<RIn, ROut>, r.Multi<ROut>> tMap<RIn, ROut>(this IToken<IMulti<RIn>> source, Func<DynamicAddress<RIn>, IToken<ROut>> mapFunction)
            where RIn : class, ResObj
            where ROut : class, ResObj
        { return Macros.Map<RIn, ROut>.Construct(source, Core.tMetaFunction(mapFunction)); }
        //public static p.Function<Macro<IMulti<RIn>, r.Boxed.MetaFunction<RIn, ROut>, r.Multi<ROut>>, TOrig, IMulti<RIn>, r.Boxed.MetaFunction<RIn, ROut>, r.Multi<ROut>> pMap<TOrig, RIn, ROut>(this IProxy<TOrig, IMulti<RIn>> values, IProxy<TOrig, r.Boxed.MetaFunction<RIn, ROut>> mapFunction)
        //    where TOrig : IToken where RIn : class, ResObj where ROut : class, ResObj
        //{ return new(values, mapFunction); }
        public static Macro<R, ro.Number, r.Multi<R>> tDuplicate<R>(this IToken<R> value, IToken<ro.Number> count)
            where R : class, ResObj
        { return Macros.Duplicate<R>.Construct(value, count); }

        //public static p.Function<tM.Multi.Duplicate<R>, TOrig, R, ro.Number, r.Multi<R>> pDuplicate<TOrig, R>(this IProxy<TOrig, R> value, IProxy<TOrig, ro.Number> count)
        //    where TOrig : IToken
        //    where R : class, ResObj
        //{ return new(value, count); }
        public static t.Number.Add tAdd(this IToken<ro.Number> a, IToken<ro.Number> b)
        { return new(a, b); }
        public static t.Number.Subtract tSubtract(this IToken<ro.Number> a, IToken<ro.Number> b)
        { return new(a, b); }
        public static t.Number.Multiply tMultiply(this IToken<ro.Number> a, IToken<ro.Number> b)
        { return new(a, b); }
        public static t.Number.Compare.GreaterThan tIsGreaterThan(this IToken<ro.Number> a, IToken<ro.Number> b)
        { return new(a, b); }

        public static t.Component.Get<H, C> tGetComponent<H, C>(this IToken<ICompositionOf<H>> holder, IComponentIdentifier<H, C> componentIdentifier)
            where H : ICompositionType
            where C : class, ResObj
        { return new(holder) { ComponentIdentifier = componentIdentifier }; }
        public static t.Component.With<H, C> tWithComponent<H, C>(this IToken<ICompositionOf<H>> holder, IComponentIdentifier<H, C> componentIdentifier, IToken<C> component)
            where H : ICompositionType
            where C : class, ResObj
        { return new(holder, component) { ComponentIdentifier = componentIdentifier }; }
        public static Macro<ICompositionOf<C>, r.Boxed.MetaFunction<R, R>, ICompositionOf<C>> tUpdateComponent<C, R>(this IToken<ICompositionOf<C>> holder, IComponentIdentifier<C, R> componentIdentifier, IToken<r.Boxed.MetaFunction<R, R>> changeFunc)
            where C : ICompositionType
            where R : class, ResObj
        { return Macros.UpdateComponent<C, R>.Construct(holder, changeFunc, componentIdentifier) ; }
        public static Macro<ICompositionOf<C>, r.Boxed.MetaFunction<R, R>, ICompositionOf<C>> tUpdateComponent<C, R>(this IToken<ICompositionOf<C>> holder, IComponentIdentifier<C, R> componentIdentifier, Func<DynamicAddress<R>, IToken<R>> changeFunc)
            where C : ICompositionType
            where R : class, ResObj
        { return Macros.UpdateComponent<C, R>.Construct(holder, Core.tMetaFunction(changeFunc), componentIdentifier); }
        public static t.Component.Without<H> tWithoutComponent<H>(this IToken<ICompositionOf<H>> holder, Resolution.Unsafe.IComponentIdentifier<H> componentIdentifier)
            where H : ICompositionType
        { return new(holder) { ComponentIdentifier = componentIdentifier }; }
        public static Macro<ICompositionOf<D>, R> tDecompose<D, R>(this IToken<ICompositionOf<D>> composition) where D : IDecomposableType<D, R>, new() where R : class, ResObj
        { return Macros.Decompose<D, R>.Construct(composition); }
        //public static p.Function<tM.Decompose<D>, TOrig, ICompositionOf<D>, ResObj> pDecompose<TOrig, D>(this IProxy<TOrig, ICompositionOf<D>> composition) where D : IDecomposableType<D>, new() where TOrig : IToken
        //{ return new(composition); }

        public static t.Component.With<r.MergeSpec<H>, C> t_WithMerged<H, C>(this IToken<ICompositionOf<r.MergeSpec<H>>> mergeObject, IComponentIdentifier<H, C> mergingIdentifier, IToken<C> component)
            where H : ICompositionType
            where C : class, ResObj
        { return mergeObject.tWithComponent(r.MergeSpec<H>.MERGE(mergingIdentifier), component); }
        public static t.Component.DoMerge<H> tMerge<H>(this IToken<ICompositionOf<H>> subject, IToken<ICompositionOf<r.MergeSpec<H>>> mergeObject)
            where H : ICompositionType
        { return new(subject, mergeObject); }
        public static t.Data.Insert<R> tDataWrite<R>(this IToken<IMemoryObject<R>> address, IToken<R> data)
            where R : class, ResObj
        { return new(address, data); }
        public static t.Data.Get<R> tDataGet<R>(this IToken<IMemoryObject<R>> address)
            where R : class, ResObj
        { return new(address); }
        public static t.Data.Remove tDataRedact(this IToken<IMemoryObject<ResObj>> address)
        { return new(address); }
        public static Macro<IMemoryObject<R>, r.Boxed.MetaFunction<R, R>, r.Instructions.Assign<R>> tDataUpdate<R>(this IToken<IMemoryObject<R>> address, IToken<r.Boxed.MetaFunction<R, R>> updateFunction)
            where R : class, ResObj
        { return Macros.UpdateMemoryObject<R>.Construct(address, updateFunction); }
        public static Macro<IMemoryObject<R>, r.Boxed.MetaFunction<R, R>, r.Instructions.Assign<R>> tDataUpdate<R>(this IToken<IMemoryObject<R>> address, Func<DynamicAddress<R>, IToken<R>> updateFunction)
            where R : class, ResObj
        { return Macros.UpdateMemoryObject<R>.Construct(address, Core.tMetaFunction(updateFunction)); }

        public static Macro<R, r.Boxed.MetaFunction<R>, R> tCatchNolla<R>(this IToken<R> value, IToken<r.Boxed.MetaFunction<R>> fallback)
            where R : class, ResObj
        { return Macros.CatchNolla<R>.Construct(value, fallback); }
        public static Macro<R, r.Boxed.MetaFunction<R>, R> tCatchNolla<R>(this IToken<R> value, Func<IToken<R>> fallback)
            where R : class, ResObj
        { return Macros.CatchNolla<R>.Construct(value, fallback().tMetaBoxed()); }
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
        public static t.Fixed<r.Multi<R>> tFixed<R>(this IEnumerable<R> values) where R : class, ResObj
        { return new(new() { Values = values.ToPSequence() }); }
        //public static t.Fixed<r.Instructions.RuleAdd> tAddRule(this Rule.IRule value)
        //{
        //    return new(new() { Rule = value });
        //}

        public static t.Fixed<r.Multi<R>> t_ToConstMulti<R>(this IEnumerable<t.Fixed<R>> values) where R : class, ResObj
        { return new(new() { Values = values.Map(x => x.Resolution).ToPSequence() }); }
        
    }
}