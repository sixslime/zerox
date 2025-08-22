namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs;

public interface uUnitEffect : IConcreteRovetu
{
    public static readonly Rovu<uUnitEffect, IRoveggi<EffectTypes.uUnitEffectType>> TYPE = new(Axoi.Du, "type");
    public static readonly Rovu<uUnitEffect, IRoveggi<Identifier.uPlayerIdentifier>> INFLICTED_BY = new(Axoi.Du, "inflicted_by");
}