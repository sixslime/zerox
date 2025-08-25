namespace SixShaded.FourZeroOne.Core.Korvessas;

using Korvessa.Defined;
using Syntax;
using Roggis;
public record Max(IKorssa<Number> a, IKorssa<Number> b) : Korvessa<Number, Number, Number>(a, b)
{
    protected override RecursiveMetaDefinition<Number, Number, Number> InternalDefinition() =>
        (_, iA, iB) =>
            iA.kRef()
                .kIsGreaterThan(iB.kRef())
                .kIfTrue<Number>(new()
                {
                    Then = iA.kRef(),
                    Else = iB.kRef()
                });
}
