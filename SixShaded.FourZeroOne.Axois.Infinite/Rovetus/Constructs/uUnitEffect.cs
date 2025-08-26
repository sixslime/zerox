namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs;

public interface uUnitEffect : IConcreteRovetu
{
    public static readonly Rovu<uUnitEffect, IRoveggi<EffectTypes.uUnitEffectType>> TYPE = new("type");
    public static readonly Rovu<uUnitEffect, IRoveggi<Identifier.uPlayerIdentifier>> INFLICTED_BY = new("inflicted_by");
}