namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Move;

public interface uPositionalMove : uMove, IConcreteRovetu
{
    /// <summary>
    /// (subject) -> possible move locations
    /// </summary>
    public static readonly Rovu<uPositionalMove, MetaFunction<IRoveggi<Identifier.uUnitIdentifier>, IMulti<IRoveggi<Identifier.uHexIdentifier>>>> MOVE_FUNCTION = new(Axoi.Du, "move_function");
}