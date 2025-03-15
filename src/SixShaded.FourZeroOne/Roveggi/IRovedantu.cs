namespace SixShaded.FourZeroOne.Roveggi;

/// <summary>
///     Allows 'Get' 'Insert' and 'Redact' Korssas.
/// </summary>
public interface IRovedantu<out R> : IRovetu
    where R : class, Rog
{ }