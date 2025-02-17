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
    using Syntax;
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
                Definition = Core.tMetaFunction<IMulti<RIn>, MetaFunction<RIn, ROut>, r.Multi<ROut>>(
                    (multiI, mapFunctionI) =>
                        Core.tMetaRecursiveFunction<ro.Number, r.Multi<ROut>>(
                        (selfFunc, i) =>
                            i.tRef().tIsGreaterThan(multiI.tRef().tCount())
                            .t_IfTrue<r.Multi<ROut>>(new()
                            {
                                Then = Core.tNolla<r.Multi<ROut>>(),
                                Else = Core.tUnion<ROut>(
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
            Definition = Core.tMetaFunction<R, ro.Number, r.Multi<R>>(
                (valueI, countI) =>
                    Core.tMetaRecursiveFunction<ro.Number, r.Multi<R>>(
                    (selfFunc, i) =>
                        i.tRef().tIsGreaterThan(countI.tRef())
                        .t_IfTrue<r.Multi<R>>(new()
                        {
                            Then = Core.tNolla<r.Multi<R>>(),
                            Else = Core.tUnion<R>(
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
            Definition = Core.tMetaFunction<IMemoryObject<R>, MetaFunction<R, R>, r.Instructions.Assign<R>>(
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
            Definition = Core.tMetaFunction<ICompositionOf<C>, MetaFunction<R, R>, ICompositionOf<C>>(
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
            Definition = Core.tMetaFunction<R, MetaFunction<R>, R>(
                (valueI, fallbackI) =>
                    valueI.tRef().tExists().tIfTrueDirect<R>(new()
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