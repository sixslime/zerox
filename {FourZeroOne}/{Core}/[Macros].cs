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

    namespace Multi
    {
        public sealed record Map<RIn, ROut> : TwoArg<Resolution.IMulti<RIn>, r.Boxed.MetaFunction<RIn, ROut>, r.Multi<ROut>>
            where RIn : class, ResObj
            where ROut : class, ResObj
        {
            protected override IProxy<r.Multi<ROut>> InternalProxy => PROXY;
            public Map(IToken<Resolution.IMulti<RIn>> values, IToken<r.Boxed.MetaFunction<RIn, ROut>> mapFunction) : base(values, mapFunction) { }

            private readonly static IProxy<Map<RIn, ROut>, r.Multi<ROut>> PROXY = MakeProxy.Statement<Map<RIn, ROut>, r.Multi<ROut>>(P =>
            {
                return
                P.pSubEnvironment(RHint<r.Multi<ROut>>.Hint(), new()
                {
                    Environment =
                    P.pMultiOf(RHint<ResObj>.Hint(),
                    [
                        P.pOriginalA().pAsVariable(out var enumerable),
                    P.pOriginalB().pAsVariable(out var mapFunction)
                    ]),
                    Value =
                    Core.tMetaRecursiveFunction(RHint<r.Objects.Number, r.Multi<ROut>>.Hint(), (selfFunc, i) =>
                    {
                        return i.tRef().tIsGreaterThan(enumerable.tRef().tCount())
                        .tIfTrue(RHint<r.Multi<ROut>>.Hint(), new()
                        {
                            Then = Core.tNolla(RHint<r.Multi<ROut>>.Hint()).tMetaBoxed(),
                            Else =
                            Core.tUnion<ROut>(RHint<ROut>.Hint(),
                            [
                                mapFunction.tRef().tExecuteWith(new() { A = enumerable.tRef().tGetIndex(i.tRef()) }).tYield(),
                            selfFunc.tRef().tExecuteWith(new() { A = i.tRef().tAdd(1.tFixed()) })
                            ]).tMetaBoxed()

                        }).tExecute();
                    }).tExecuteWith(new() { A = 1.tFixed() }).pDirect(P)
                });
            });
            protected override IOption<string> CustomToString() => $"{Arg1}=>{Arg2}".AsSome();
        }
    }
    
    // TODO: make IfElse macro that executes the metafunction returned by the token IfElse. then rename the token IfElse to something else (and make it general maybe) :D.

    public sealed record UpdateStateObject<A, D> : TwoArg<A, r.Boxed.MetaFunction<D, D>, r.Instructions.Assign<D>> where A : class, Resolution.IStateAddress<D>, ResObj where D : class, ResObj
    {
        public UpdateStateObject(IToken<A> in1, IToken<r.Boxed.MetaFunction<D, D>> in2) : base(in1, in2) { }
        protected override IProxy<r.Instructions.Assign<D>> InternalProxy => PROXY;

        public readonly static IProxy<UpdateStateObject<A, D>, r.Instructions.Assign<D>> PROXY = MakeProxy.Statement<UpdateStateObject<A, D>, r.Instructions.Assign<D>>(P =>
        {
            return P.pSubEnvironment(RHint<r.Instructions.Assign<D>>.Hint(), new()
            {
                Environment = P.pOriginalA().pAsVariable(out var address).pYield(),
                Value = P.pOriginalB().pExecuteWith(new()
                {
                    A = address.tRef().tReadData(RHint<D>.Hint()).pDirect(P)
                }).pWriteTo(address.tRef().pDirect(P))
            });
        });
    }
    public sealed record Compose<C> : Macro<Resolution.CompositionOf<C>> where C : Resolution.ICompositionType, new()
    {
        protected override IProxy<Resolution.CompositionOf<C>> InternalProxy => PROXY;
        public readonly static IProxy<Compose<C>, Resolution.CompositionOf<C>> PROXY = MakeProxy.Statement<Compose<C>, Resolution.CompositionOf<C>>(P =>
        {
            return new Resolution.CompositionOf<C>().tFixed().pDirect(P);
        });
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
                Environment = P.pMultiOf(RHint<ResObj>.Hint(),
                [
                    P.pOriginalA().pAsVariable(out var value)
                ]),
                Value = value.tRef().tExists().pDirect(P).pIfTrue(RHint<R>.Hint(), new()
                {
                    Then = value.tRef().pDirect(P).pMetaBoxed(),
                    Else = P.pOriginalB().pMetaBoxed()
                })
                .pExecute()
            });
        });
    }

}