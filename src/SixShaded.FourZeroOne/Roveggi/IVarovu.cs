namespace SixShaded.FourZeroOne.Roveggi;

/// <summary>
///     o
/// </summary>
public interface IVarovu<in C, R> : IRovetu
    where C : IRovetu
    where R : class, Rog;