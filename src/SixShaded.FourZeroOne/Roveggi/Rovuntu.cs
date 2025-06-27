namespace SixShaded.FourZeroOne.Roveggi;

/// <summary>
///     o
/// </summary>
public interface IRovundantu<in C, R> : IRovetu
    where C : IRovetu
    where R : class, Rog;