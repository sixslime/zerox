namespace SixShaded.FourZeroOne.Axois.Infinite.Korssas;

using Handles;
using Helpers;
using MorseCode.ITask;
using HexObject = IRoveggi<u.Constructs.uRelativeCoordinate>;
using Hex = u.Constructs.uRelativeCoordinate;
public record LineIntersections(IKorssa<HexObject> from, IKorssa<HexObject> to) : Korssa.Defined.Function<HexObject, HexObject, Multi<HexObject>>(from, to)
{
    protected override ITask<IOption<Multi<HexObject>>> Evaluate(IKorssaContext _, IOption<HexObject> in1, IOption<HexObject> in2)
    {
        if (in1.RemapAs(Helpers.HexPos.FromRoveggi).Press().CheckNone(out var hexA) || in1.RemapAs(Helpers.HexPos.FromRoveggi).Press().CheckNone(out var hexB))
            return new None<Multi<HexObject>>().ToCompletedITask();


        throw new NotImplementedException();
    }
}