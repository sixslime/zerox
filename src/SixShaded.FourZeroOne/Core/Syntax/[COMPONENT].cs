using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixShaded.FourZeroOne.Core.Syntax
{
    using Resolutions;
    public static partial class Core
    {
        public static Macro<ICompositionOf<C>> tCompose<C>() where C : ICompositionType, new()
        {
            return Macros.Compose<C>.Construct();
        }
    }
    public static partial class TokenSyntax
    {
        public static Tokens.Component.Get<C, R> tGetComponent<C, R>(this IToken<ICompositionOf<C>> holder, IComponentIdentifier<C, R> componentIdentifier)
    where C : ICompositionType
    where R : class, Res
        { return new(holder) { ComponentIdentifier = componentIdentifier }; }
        public static Tokens.Component.With<C, R> tWithComponent<C, R>(this IToken<ICompositionOf<C>> holder, IComponentIdentifier<C, R> componentIdentifier, IToken<R> component)
            where C : ICompositionType
            where R : class, Res
        { return new(holder, component) { ComponentIdentifier = componentIdentifier }; }
        public static Macro<ICompositionOf<C>, MetaFunction<R, R>, ICompositionOf<C>> tUpdateComponent<C, R>(this IToken<ICompositionOf<C>> holder, IComponentIdentifier<C, R> componentIdentifier, IToken<MetaFunction<R, R>> changeFunc)
            where C : ICompositionType
            where R : class, Res
        { return Macros.UpdateComponent<C, R>.Construct(holder, changeFunc, componentIdentifier); }
        public static Macro<ICompositionOf<C>, MetaFunction<R, R>, ICompositionOf<C>> tUpdateComponent<C, R>(this IToken<ICompositionOf<C>> holder, IComponentIdentifier<C, R> componentIdentifier, Func<DynamicAddress<R>, IToken<R>> changeFunc)
            where C : ICompositionType
            where R : class, Res
        { return Macros.UpdateComponent<C, R>.Construct(holder, Core.tMetaFunction(changeFunc), componentIdentifier); }
        public static Tokens.Component.Without<H> tWithoutComponent<H>(this IToken<ICompositionOf<H>> holder, Resolution.Unsafe.IComponentIdentifier<H> componentIdentifier)
            where H : ICompositionType
        { return new(holder) { ComponentIdentifier = componentIdentifier }; }
        public static Macro<ICompositionOf<D>, R> tDecompose<D, R>(this IToken<ICompositionOf<D>> composition) where D : IDecomposableType<D, R>, new() where R : class, Res
        { return Macros.Decompose<D, R>.Construct(composition); }

        public static Tokens.Component.With<MergeSpec<C>, R> t_WithMerged<C, R>(this IToken<ICompositionOf<MergeSpec<C>>> mergeObject, IComponentIdentifier<C, R> mergingIdentifier, IToken<R> component)
            where C : ICompositionType
            where R : class, Res
        { return mergeObject.tWithComponent(MergeSpec<C>.MERGE(mergingIdentifier), component); }
        public static Tokens.Component.DoMerge<C> tMerge<C>(this IToken<ICompositionOf<C>> subject, IToken<ICompositionOf<MergeSpec<C>>> mergeObject)
            where C : ICompositionType
        { return new(subject, mergeObject); }

    }
}
