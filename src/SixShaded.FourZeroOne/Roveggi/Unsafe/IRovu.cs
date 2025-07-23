namespace SixShaded.FourZeroOne.Roveggi.Unsafe;

using FourZeroOne;
using Roveggi;

public interface IRovu<in C> : IRovu, IGetRovu<C>
    where C : IRovetu;

public interface IRovu;