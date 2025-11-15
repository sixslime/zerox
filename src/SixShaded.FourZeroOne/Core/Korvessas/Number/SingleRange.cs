namespace SixShaded.FourZeroOne.Core.Korvessas.Number;

using Korvessa.Defined;
using Roggis;
using Syntax;

public record SingleRange(IKorssa<Number> number) : Korvessa<Number, NumRange>(number)
{
    protected override RecursiveMetaDefinition<Number, NumRange> InternalDefinition() =>
        (_, iNum) =>
            iNum.kRef().kRangeTo(iNum.kRef());
}
