namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Data;

using Constructs.Ability;
using Identifier;

public interface uPlayerData : IRovetu
{
    public static readonly Rovu<uPlayerData, IRoveggi<uPlayerIdentifier>> ID = new(Axoi.Du, "id");
    public static readonly Rovu<uPlayerData, IMulti<IRoveggi<uAbility>>> HAND = new(Axoi.Du, "hand");
    public static readonly Rovu<uPlayerData, IMulti<IRoveggi<uAbility>>> STACK = new(Axoi.Du, "stack");
    public static readonly Rovu<uPlayerData, IMulti<IRoveggi<Constructs.uPlayableAction>>> POSSIBLE_ACTIONS = new(Axoi.Du, "possible_actions");
    public static readonly Rovu<uPlayerData, Number> ENERGY = new(Axoi.Du, "energy");
    public static readonly Rovu<uPlayerData, Number> CONTROL = new(Axoi.Du, "control");
}