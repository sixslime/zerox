namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs;

using Identifier;

public interface uPlayableAction : IConcreteRovetu
{
    public static readonly Rovu<uPlayableAction, IRoveggi<ActionTypes.uActionType>> TYPE = new("type");
    public static readonly Rovu<uPlayableAction, MetaFunction<Bool>> CONDITION = new("condition");
    public static readonly Rovu<uPlayableAction, MetaFunction<Rog>> STATEMENT = new("statement");
}