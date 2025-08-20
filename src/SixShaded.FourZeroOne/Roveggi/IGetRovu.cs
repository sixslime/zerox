namespace SixShaded.FourZeroOne.Roveggi;

public interface IGetRovu<in C, R> : Unsafe.IGetRovu<C>
    where C : IRovetu
    where R : class, Rog
{ }