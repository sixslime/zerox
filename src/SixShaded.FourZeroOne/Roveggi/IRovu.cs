namespace SixShaded.FourZeroOne.Roveggi;

using SixShaded.FourZeroOne.Roveggi.Unsafe;

public interface IRovu<in C, in R> : IRovu<C> where C : IRovetu where R : class, Rog
{ }