namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs;

public interface uCheckable : IRovetu
{
    public static readonly AbstractGetRovu<uCheckable, Bool> PASSED = new(Axoi.Du, "passed");
}