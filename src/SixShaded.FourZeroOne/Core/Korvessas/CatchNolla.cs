namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public record CatchNolla<R>(IKorssa<R> value, IKorssa<MetaFunction<R>> fallback) : Korvessa<R, MetaFunction<R>, R>(value, fallback)
    where R : class, Rog
{
    protected override RecursiveMetaDefinition<R, MetaFunction<R>, R> InternalDefinition() =>
        (_, iValue, iFallback) =>
            iValue.kRef()
                .kExists()
                .kIfTrue<R>(
                new()
                {
                    Then = iValue.kRef(),
                    Else = iFallback.kRef().kExecute(),
                });
}