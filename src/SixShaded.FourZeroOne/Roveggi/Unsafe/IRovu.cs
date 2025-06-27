namespace SixShaded.FourZeroOne.Roveggi.Unsafe;

using FourZeroOne;
using Roveggi;

public interface IRovu<in C> : IRovu
    where C : IRovetu;

public interface IRovu;