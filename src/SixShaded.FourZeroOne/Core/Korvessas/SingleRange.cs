namespace SixShaded.FourZeroOne.Core.Korvessas;

using Korvessa.Defined;
using Syntax;
using Roggis;
public record SingleRange(IKorssa<Number> number) : Korvessa<Number, NumRange>(number)
{
    protected override RecursiveMetaDefinition<Number, NumRange> InternalDefinition() =>
        (_, iNum) =>
            iNum.kRef().kRangeTo(iNum.kRef());
}
