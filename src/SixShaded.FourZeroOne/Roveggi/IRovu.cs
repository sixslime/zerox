namespace SixShaded.FourZeroOne.Roveggi;

using Unsafe;

public interface IRovu<in C, R> : IRovu<C>
    where C : IRovetu
    where R : class, Rog
{ }