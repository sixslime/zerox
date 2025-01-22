using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Perfection;

namespace FourZeroOne.Core.Macros
{
    using Token;
    using ResObj = Resolution.IResolution;
    using Macro;
    using t = Core.Tokens;
    using r = Core.Resolutions;
    using FourZeroOne.Proxy;
    using Syntax;
    using FourZeroOne.Proxy.Unsafe;
    using Resolution;
    using ro = Core.Resolutions.Objects;
    namespace Multi
    {
        public sealed record Map<RIn, ROut> : TwoArg<IMulti<RIn>, r.Boxed.MetaFunction<RIn, ROut>, r.Multi<ROut>>
            where RIn : class, ResObj
            where ROut : class, ResObj
        {
            protected override IProxy<r.Multi<ROut>> InternalProxy => PROXY;
            public Map(IToken<IMulti<RIn>> values, IToken<r.Boxed.MetaFunction<RIn, ROut>> mapFunction) : base(values, mapFunction) { }

            // DEV - Consider not storing the map function as a variable, so that the Token *does* re-evaluate every iteration.
            private readonly static IProxy<Map<RIn, ROut>, r.Multi<ROut>> PROXY = MakeProxy.Statement<Map<RIn, ROut>, r.Multi<ROut>>(P =>
            {
                return
                P.pSubEnvironment(RHint<r.Multi<ROut>>.Hint(), new()
                {
                    Environment = P.p_Env(
                        P.pOriginalA().pAsVariable(out var enumerable),
                        P.pOriginalB().pAsVariable(out var mapFunction)),
                    Value =
                    Core.tMetaRecursiveFunction(RHint<ro.Number, r.Multi<ROut>>.Hint(),
                        (selfFunc, i) =>
                            i.tRef().tIsGreaterThan(enumerable.tRef().tCount())
                            .tIfTrueDirect(RHint<r.Multi<ROut>>.Hint(), new()
                            {
                                Then = Core.tNolla(RHint<r.Multi<ROut>>.Hint()).tMetaBoxed(),
                                Else = Core.tUnion(RHint<ROut>.Hint(),
                                [
                                    mapFunction.tRef().tExecuteWith(new() { A = enumerable.tRef().tGetIndex(i.tRef()) }).tYield(),
                                    selfFunc.tRef().tExecuteWith(new() { A = i.tRef().tAdd(1.tFixed()) })
                                ]).tMetaBoxed()
                            }).tExecute())
                    .tExecuteWith(new() { A = 1.tFixed() }).pDirect(P)
                });
            });
            protected override IOption<string> CustomToString() => $"{Arg1}=>{Arg2}".AsSome();
        }
        public sealed record Duplicate<R> : TwoArg<R, ro.Number, r.Multi<R>>
            where R : class, ResObj
        {
            public Duplicate(IToken<R> value, IToken<ro.Number> count) : base(value, count) { }
            protected override IProxy<r.Multi<R>> InternalProxy => PROXY;
            private readonly static IProxy<Duplicate<R>, r.Multi<R>> PROXY = MakeProxy.Statement<Duplicate<R>, r.Multi<R>>(
                P =>
                    P.pSubEnvironment(RHint<r.Multi<R>>.Hint(), new() {
                        Environment = P.p_Env(
                            P.pOriginalA().pAsVariable(out var value),
                            P.pOriginalB().pAsVariable(out var duplicateCount)),
                        Value =
                            Core.tMetaRecursiveFunction(RHint<ro.Number, r.Multi<R>>.Hint(),
                                (selfFunc, i) =>
                                    i.tRef().tIsGreaterThan(duplicateCount.tRef())
                                    .tIfTrueDirect(RHint<r.Multi<R>>.Hint(), new()
                                    {
                                        Then = Core.tNolla(RHint<r.Multi<R>>.Hint()).tMetaBoxed(),
                                        Else = Core.tUnion(RHint<R>.Hint(),
                                        [
                                            value.tRef().tYield(),
                                            selfFunc.tRef().tExecuteWith(new() { A = i.tRef().tAdd(1.tFixed()) })
                                        ]).tMetaBoxed()
                                    }).tExecute())
                            .tExecuteWith(new() { A = 1.tFixed() }).pDirect(P)
                    }
            ));
            protected override IOption<string> CustomToString() => $"{Arg1}**({Arg2})".AsSome();

        }
    }
    
    public sealed record Decompose<D> : OneArg<ICompositionOf<D>, ResObj> where D : IDecomposableType<D>, new()
    {
        public Decompose(IToken<ICompositionOf<D>> composition) : base(composition) { }

        // this is nightmare fuel.
        protected override IProxy<ResObj> InternalProxy => new D().DecompositionProxy;
        protected override IOption<string> CustomToString() => $"~{Arg1}".AsSome();
    }
    public sealed record UpdateStateObject<A, D> : TwoArg<A, r.Boxed.MetaFunction<D, D>, r.Instructions.Assign<D>> where A : class, IStateAddress<D>, ResObj where D : class, ResObj
    {
        public UpdateStateObject(IToken<A> in1, IToken<r.Boxed.MetaFunction<D, D>> in2) : base(in1, in2) { }
        protected override IProxy<r.Instructions.Assign<D>> InternalProxy => PROXY;

        public readonly static IProxy<UpdateStateObject<A, D>, r.Instructions.Assign<D>> PROXY = MakeProxy.Statement<UpdateStateObject<A, D>, r.Instructions.Assign<D>>(P =>
        {
            return P.pSubEnvironment(RHint<r.Instructions.Assign<D>>.Hint(), new()
            {
                Environment = P.pMultiOf(RHint<ResObj>.Hint(),
                [
                    P.pOriginalA().pAsVariable(out var address),
                    P.pOriginalB().pAsVariable(out var updateFunc)
                ]),
                Value = address.tRef().tDataWrite(updateFunc.tRef().tExecuteWith(new()
                {
                    A = address.tRef().tDataRead(RHint<D>.Hint())
                })).pDirect(P)
            });
        });
        protected override IOption<string> CustomToString() => $"{Arg1} <==! {Arg2}".AsSome();
    }

    public sealed record UpdateComponent<C, R> : Macro<ICompositionOf<C>>, Token.Unsafe.IHasArg1<ICompositionOf<C>>, Token.Unsafe.IHasArg2<r.Boxed.MetaFunction<R, R>>
        where C : ICompositionType where R : class, ResObj
    {
        public IComponentIdentifier<C, R> Identifier { get; private init; }
        protected override IProxy<ICompositionOf<C>> InternalProxy => MakeProxy.Statement<UpdateComponent<C, R>, ICompositionOf<C>>(
            P => P.pSubEnvironment(RHint<ICompositionOf<C>>.Hint(), new()
            {
                Environment = P.p_Env(P.pOriginalA().pAsVariable(out var comp), P.pOriginalB().pAsVariable(out var func)),
                Value = comp.tRef().tWithComponent(Identifier, func.tRef().tExecuteWith(new() { A = comp.tRef().tGetComponent(Identifier) })).pDirect(P)
            }));
        public IToken<ICompositionOf<C>> Arg1 { get; private init; }
        public IToken<r.Boxed.MetaFunction<R, R>> Arg2 { get; private init; }

        public UpdateComponent(IComponentIdentifier<C, R> identifier, IToken<ICompositionOf<C>> composition, IToken<r.Boxed.MetaFunction<R, R>> func)
        {
            Identifier = identifier;
            Arg1 = composition;
            Arg2 = func;
        }
        protected override IOption<string> CustomToString() => $"{Arg1}-{Identifier} <=! {Arg2}".AsSome();
    }
    public sealed record Compose<C> : Macro<ICompositionOf<C>> where C : ICompositionType, new()
    {
        protected override IProxy<ICompositionOf<C>> InternalProxy => PROXY;
        public readonly static IProxy<Compose<C>, ICompositionOf<C>> PROXY = MakeProxy.Statement<Compose<C>, ICompositionOf<C>>(P =>
        {
            return new CompositionOf<C>().tFixed().pDirect(P);
        });
        protected override IOption<string> CustomToString() => $"{typeof(C).Namespace!.Split(".")[^1]}.{typeof(C).Name}".AsSome();
    }
    public sealed record CatchNolla<R> : TwoArg<R, R, R> where R : class, ResObj
    {
        protected override IProxy<R> InternalProxy => PROXY;
        public CatchNolla(IToken<R> value, IToken<R> fallback) : base(value, fallback) { }
        public readonly static IProxy<CatchNolla<R>, R> PROXY = MakeProxy.Statement<CatchNolla<R>, R>(P =>
        {
            return
            P.pSubEnvironment(RHint<R>.Hint(), new()
            {
                Environment = P.pOriginalA().pAsVariable(out var value),
                Value = value.tRef().tExists().pDirect(P).pIfTrueDirect(RHint<R>.Hint(), new()
                {
                    Then = value.tRef().pDirect(P).pMetaBoxed(),
                    Else = P.pOriginalB().pMetaBoxed()
                })
                .pExecute()
            });
        });
        protected override IOption<string> CustomToString() => $"{Arg1} or {Arg2}".AsSome();
    }

}