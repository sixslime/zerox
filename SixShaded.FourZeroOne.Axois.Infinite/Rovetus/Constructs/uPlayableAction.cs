
namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs;

using Identifier;

public interface uPlayableAction : IConcreteRovetu
{
    public static readonly Rovu<uPlayableAction, IRoveggi<ActionTypes.uActionType>> TYPE = new(Axoi.Du, "type");
    public static readonly Rovu<uPlayableAction, MetaFunction<Bool>> CONDITION = new(Axoi.Du, "condition");
    public static readonly Rovu<uPlayableAction, MetaFunction<IRoveggi<Resolved.uResolvedAction>>> STATEMENT = new(Axoi.Du, "statement");
}