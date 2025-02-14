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
    using Resolutions.Boxed;
    public static class Map<RIn, ROut>
            where RIn : class, ResObj
            where ROut : class, ResObj
    {
        public static Macro<IMulti<RIn>, MetaFunction<RIn, ROut>, r.Multi<ROut>> Construct(IToken<IMulti<RIn>> multi, IToken<MetaFunction<RIn, ROut>> mapFunction)
        {
            return new(multi, mapFunction)
            {
                Label = Package.Label("Map"),
                Definition = Core.tMetaFunction(RHint<IMulti<RIn>, MetaFunction<RIn, ROut>, r.Multi<ROut>>.HINT,
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
                                            new() { A = multiI.tRef().tAtIndex(i.tRef()) }).tYield(),
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
            Label = Package.Label("Duplicate"),
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
    public static class UpdateMemoryObject<R>
        where R : class, ResObj
    {
        public static Macro<IMemoryObject<R>, MetaFunction<R, R>, r.Instructions.Assign<R>> Construct(IToken<IMemoryObject<R>> address, IToken<MetaFunction<R, R>> updateFunction) => new(address, updateFunction)
        {
            Label = Package.Label("UpdateMemoryObject"),
            Definition = Core.tMetaFunction(RHint<IMemoryObject<R>, MetaFunction<R, R>, r.Instructions.Assign<R>>.HINT,
                (addressI, updateFunctionI) =>
                    addressI.tRef().tDataWrite(
                        updateFunctionI.tRef()
                        .tExecuteWith(new() { A = addressI.tRef().tDataGet() })))
                .Resolution
        };
    }

    public static class UpdateComponent<C, R>
        where C : ICompositionType
        where R : class, ResObj
    {
        public static Macro<ICompositionOf<C>, MetaFunction<R, R>, ICompositionOf<C>> Construct(IToken<ICompositionOf<C>> composition, IToken<MetaFunction<R, R>> updateFunction, IComponentIdentifier<C, R> component) => new(composition, updateFunction)
        {
            Label = Package.Label("UpdateComponent"),
            CustomData = [component],
            Definition = Core.tMetaFunction(RHint<ICompositionOf<C>, MetaFunction<R, R>, ICompositionOf<C>>.HINT,
                (compositionI, updateFunctionI) =>
                    compositionI.tRef().tWithComponent(component, 
                        updateFunctionI.tRef()
                        .tExecuteWith(new() { A = compositionI.tRef().tGetComponent(component) })))
                .Resolution
        };
    }
    public static class Compose<C>
        where C : ICompositionType, new()
    {
        public static Macro<ICompositionOf<C>> Construct() => new()
        {
            Label = Package.Label("Compose"),
            Definition = new t.Fixed<ICompositionOf<C>>(new CompositionOf<C>()).tMetaBoxed().Resolution
        };
    }
    public static class CatchNolla<R>
        where R : class, ResObj
    {
        public static Macro<R, MetaFunction<R>, R> Construct(IToken<R> value, IToken<MetaFunction<R>> fallback) => new(value, fallback)
        {
            Label = Package.Label("CatchNolla"),
            Definition = Core.tMetaFunction(RHint<R, MetaFunction<R>, R>.HINT,
                (valueI, fallbackI) =>
                    valueI.tRef().tExists().tIfTrueDirect(RHint<R>.HINT, new()
                    {
                        Then = valueI.tRef().tMetaBoxed(),
                        Else = fallbackI.tRef()
                    })
                    .tExecute())
                .Resolution
        };
    }
    public static class Package
    {
        public const string NAMESPACE = "CORE";
        internal static MacroLabel Label(string identifier) => new() { Namespace = NAMESPACE, Identifier = identifier };
    }
}