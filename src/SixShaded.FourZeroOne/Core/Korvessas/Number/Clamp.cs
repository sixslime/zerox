namespace SixShaded.FourZeroOne.Core.Korvessas.Number;

using Korvessa.Defined;
using Roggis;
using Syntax;

public record Clamp(IKorssa<Number> val, IKorssa<NumRange> range) : Korvessa<Number, NumRange, Number>(val, range)
{
    protected override RecursiveMetaDefinition<Number, NumRange, Number> InternalDefinition() =>
        (_, iVal, iRange) =>
            iVal.kRef()
                .kAtMost(iRange.kRef().kEnd())
                .kAtLeast(iRange.kRef().kStart());
}