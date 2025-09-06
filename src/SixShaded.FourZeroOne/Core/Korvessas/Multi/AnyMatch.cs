namespace SixShaded.FourZeroOne.Core.Korvessas.Multi;

using Korvessa.Defined;
using Roggis;
using Syntax;

public record AnyMatch<R>(IKorssa<IMulti<R>> multi, IKorssa<MetaFunction<R, Bool>> predicate) : Korvessa<IMulti<R>, MetaFunction<R, Bool>, Bool>(multi, predicate)
    where R : class, Rog
{
    protected override RecursiveMetaDefinition<IMulti<R>, MetaFunction<R, Bool>, Bool> InternalDefinition() =>
        (_, iMulti, iPredicate) =>
            iMulti.kRef()
                .kAllMatch(
                iX =>
                    iPredicate.kRef()
                        .kExecuteWith(
                        new()
                        {
                            A = iX.kRef()
                        })
                        .kNot())
                .kNot();
}