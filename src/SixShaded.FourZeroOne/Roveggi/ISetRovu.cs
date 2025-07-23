namespace SixShaded.FourZeroOne.Roveggi;

public interface ISetRovu<in C, R> : Unsafe.ISetRovu<C>
    where C : IRovetu
    where R : class, Rog
{ }