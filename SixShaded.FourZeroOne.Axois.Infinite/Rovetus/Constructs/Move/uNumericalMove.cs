namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Move;

public interface uNumericalMove : uMove, IConcreteRovetu
{
    public static readonly Rovu<uNumericalMove, NumRange> AMOUNT = new(Axoi.Du, "amount");
    /// <summary>
    /// default always true. <br></br>
    /// wall detection should probably be built in?
    /// </summary>
    public static readonly Rovu<uNumericalMove, MetaFunction<IRoveggi<Identifier.uHexCoordinate>, IRoveggi<Identifier.uHexCoordinate>, Bool>> PATH_CONDITION = new(Axoi.Du, "path_condition");
}
