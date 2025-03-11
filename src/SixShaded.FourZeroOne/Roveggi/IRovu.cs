namespace SixShaded.FourZeroOne.Roveggi;

using Unsafe;

public interface IRovu<in C, in R> : IRovu<C>
    where C : IRovetu
    where R : class, Rog
{ }