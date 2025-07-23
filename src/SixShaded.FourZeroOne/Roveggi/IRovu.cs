namespace SixShaded.FourZeroOne.Roveggi;

using Unsafe;

public interface IRovu<in C, R> : IRovu<C>, IGetRovu<C, R>
    where C : IRovetu
    where R : class, Rog
{ }