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
        public static class Map<RIn, ROut>
            where RIn : class, ResObj
            where ROut : class, ResObj
        {
            public static Macro<IMulti<RIn>, r.Boxed.MetaFunction<RIn, ROut>, r.Multi<ROut>> Construct(IToken<IMulti<RIn>> multi, IToken<r.Boxed.MetaFunction<RIn, ROut>> mapFunction)
            {
                return new(multi, mapFunction)
                {
                    Label = Package.Label("Multi.Duplicate"),
                    Definition = Core.tMetaFunction(RHint<IMulti<RIn>, r.Boxed.MetaFunction<RIn, ROut>, r.Multi<ROut>>.HINT,
                        (multiI, mapFunctionI) =>
                            Core.tMetaRecursiveFunction(RHint<ro.Number, r.Multi<ROut>>.HINT,
                            (selfFunc, i) =>
                                i.tRef().tIsGreaterThan(multiI.tRef().tCount())
                                .t_IfTrue(RHint<r.Multi<ROut>>.HINT, new()
                                {
                                    Then = Core.tNolla(RHint<r.Multi<ROut>>.HINT),
                                    Else = Core.tUnion(RHint<ROut>.HINT,
                                    [
                                        mapFunctionI.tRef().tExecuteWith(
                                            new() { A = multiI.tRef().tGetIndex(i.tRef()) }).tYield(),
                                        selfFunc.tRef().tExecuteWith(
                                            new() { A = i.tRef().tAdd(1.tFixed()) })
                                    ])
                                }))
                            .tExecuteWith(new() { A = 1.tFixed() }))
                        .Resolution
                };
            }
        }
        public static class Duplicate<R>
            where R : class, ResObj
        {
            public static Macro<R, ro.Number, r.Multi<R>> Construct(IToken<R> value, IToken<ro.Number> count) => new(value, count)
            {
                Label = Package.Label("Multi.Duplicate"),
                Definition = Core.tMetaFunction(RHint<R, ro.Number, r.Multi<R>>.HINT,
                    (valueI, countI) =>
                        Core.tMetaRecursiveFunction(RHint<ro.Number, r.Multi<R>>.HINT,
                        (selfFunc, i) =>
                            i.tRef().tIsGreaterThan(countI.tRef())
                            .t_IfTrue(RHint<r.Multi<R>>.HINT, new()
                            {
                                Then = Core.tNolla(RHint<r.Multi<R>>.HINT),
                                Else = Core.tUnion(RHint<R>.HINT,
                                [
                                    valueI.tRef().tYield(),
                                    selfFunc.tRef().tExecuteWith(new() { A = i.tRef().tAdd(1.tFixed()) })
                                ])
                            }))
                        .tExecuteWith(new() { A = 1.tFixed() }))
                    .Resolution
            };
        }
    }

    // now that macros are data driven, 'decompose' can be functionally replaced with handler macros
    public static class Decompose<D, R>
        where D : IDecomposableType<D, R>, new()
        where R : class, ResObj
    {
        public static Macro<ICompositionOf<D>, R> Construct(IToken<ICompositionOf<D>> composition) => new(composition)
        {
            Label = Package.Label("Decompose"),
            Definition = new D().DecompositionFunction
        };
    }
    public static class UpdateStateObject<A, R>
        where A : class, IStateAddress<R>, ResObj
        where R : class, ResObj
    {
        public static Macro<A, r.Boxed.MetaFunction<R, R>, r.Instructions.Assign<R>> Construct(IToken<A> address, IToken<r.Boxed.MetaFunction<R, R>> updateFunction) => new(address, updateFunction)
        {
            Label = Package.Label("UpdateStateObject"),
            Definition = Core.tMetaFunction(RHint<A, r.Boxed.MetaFunction<R, R>, r.Instructions.Assign<R>>.HINT,
                (addressI, updateFunctionI) =>
                    addressI.tRef().tDataWrite(
                        updateFunctionI.tRef()
                        .tExecuteWith(new() { A = addressI.tRef().tDataRead(RHint<R>.HINT) })))
                .Resolution
        };
    }

    public static class UpdateComponent<C, R>
        where C : ICompositionType
        where R : class, ResObj
    {
        public static Macro<ICompositionOf<C>, r.Boxed.MetaFunction<R, R>, ICompositionOf<C>> Construct(IToken<ICompositionOf<C>> composition, IToken<r.Boxed.MetaFunction<R, R>> updateFunction, IComponentIdentifier<C, R> component) => new(composition, updateFunction)
        {
            Label = Package.Label("UpdateComponent"),
            Definition
        };
    }
    public static class Package
    {
        public const string NAMESPACE = "CORE";
        internal static MacroLabel Label(string identifier) => new() { Namespace = NAMESPACE, Identifier = identifier };
    }
}