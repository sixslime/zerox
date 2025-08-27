namespace SixShaded.FourZeroOne.Roveggi;

public interface IVarovu<in C, RKey, RVal> : Unsafe.IVarovu
    where C : IRovetu
    where RKey : class, Rog
    where RVal : class, Rog
{   
    public IRovu<C, RVal> GenerateRovu(RKey keyRoggi);
}