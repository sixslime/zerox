namespace SixShaded.FourZeroOne.Core.Korvessas.Number;

using Korvessa.Defined;
using Roggis;
using Syntax;

public record SingleRange(IKorssa<Korvessas.Number> number) : Korvessa<Korvessas.Number, NumRange>(number)
{
    protected override RecursiveMetaDefinition<Korvessas.Number, NumRange> InternalDefinition() =>
        (_, iNum) =>
            iNum.kRef().kRangeTo(iNum.kRef());
}
