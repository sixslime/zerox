namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Ability;

public interface uAbility : IRovetu
{
    public static readonly Rovu<uAbility, MetaFunction<Rog>> ENVIRONMENT_PREMOD = new("environment_premod");
    public static readonly AbstractGetRovu<uAbility, IRoveggi<Types.uAbilityType>> TYPE = new("type");
}