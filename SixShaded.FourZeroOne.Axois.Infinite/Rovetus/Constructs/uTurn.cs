
namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs;

using Identifier;

public interface uTurn : IConcreteRovetu
{
    public static readonly Rovu<uTurn, MetaFunction<Rog>> PRE_TURN = new(Axoi.Du, "pre_turn");
    public static readonly Rovu<uTurn, MetaFunction<Rog>> POST_TURN = new(Axoi.Du, "post_turn");
    public static readonly Rovu<uTurn, IRoveggi<uPlayerIdentifier>> PLAYER = new(Axoi.Du, "player");
}