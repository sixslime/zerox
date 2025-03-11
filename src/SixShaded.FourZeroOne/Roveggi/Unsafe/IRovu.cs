namespace SixShaded.FourZeroOne.Roveggi.Unsafe;

using SixShaded.FourZeroOne;
using SixShaded.FourZeroOne.Roveggi;

public interface IRovu<in C> : IRovu where C : IRovetu;

public interface IRovu : IAxovendu;