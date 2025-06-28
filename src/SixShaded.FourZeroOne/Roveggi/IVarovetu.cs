namespace SixShaded.FourZeroOne.Roveggi;

/// <summary>
///     o
/// </summary>
public interface IVarovetu<RKey, RVal> : IRovetu
    where RKey : class, Rog
    where RVal : class, Rog
{ }