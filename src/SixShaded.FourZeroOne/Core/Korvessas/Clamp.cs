namespace SixShaded.FourZeroOne.Core.Korvessas;

using Korvessa.Defined;
using Syntax;
using Roggis;
public record Clamp(IKorssa<Number> val, IKorssa<NumRange> range) : Korvessa<Number, NumRange, Number>(val, range)
{
    protected override RecursiveMetaDefinition<Number, NumRange, Number> InternalDefinition() =>
        (_, iVal, iRange) =>
            iVal.kRef()
                .kAtMost(iRange.kRef().kEnd())
                .kAtLeast(iRange.kRef().kStart());
}