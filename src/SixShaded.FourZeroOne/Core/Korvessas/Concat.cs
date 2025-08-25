namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public record Concat<R>(IKorssa<IMulti<R>> a, IKorssa<IMulti<R>> b) : Korvessa<IMulti<R>, IMulti<R>, Multi<R>>(a, b)
    where R : class, Rog
{
    protected override RecursiveMetaDefinition<IMulti<R>, IMulti<R>, Multi<R>> InternalDefinition() =>
        (_, iA, iB) =>
            Core.kMulti<IMulti<R>>(
                new()
                {
                    iA.kRef(),
                    iB.kRef()
                })
                .kFlatten();
}