namespace SixShaded.FourZeroOne.Roveggi;

/// <summary>
///     o
/// </summary>
public interface IVarovu<in C, RKey, RVal>
    where C : IRovetu
    where RKey : class, Rog
    where RVal : class, Rog
{   
    public IRovu<C, RVal> GenerateRovu(RKey keyRoggi);
}